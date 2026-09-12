#!/usr/bin/env python3
"""PreToolUse hook (Bash matcher): rewrites noisy install/build/test commands
so only the last 30 lines plus any error-looking lines reach the model context,
instead of the full raw output."""
import json
import re
import sys
import shlex
import base64

SEGMENT_SPLIT = re.compile(r"&&|\|\||[;|]")

# binary -> set of subcommands that make it noisy, or None if any invocation qualifies
RULES = {
    "npm": {"install", "ci", "test"},
    "yarn": {"install", "build", "test"},
    "pnpm": {"install", "build", "test"},
    "make": None,
    "cargo": {"build", "test"},
    "go": {"build", "test"},
    "mvn": None,
    "gradle": None,
    "./gradlew": None,
    "dotnet": {"build", "test"},
    "pip": {"install"},
    "pip3": {"install"},
}


def is_noisy(command: str) -> bool:
    for segment in SEGMENT_SPLIT.split(command):
        try:
            tokens = shlex.split(segment.strip())
        except ValueError:
            continue
        if not tokens:
            continue
        binary = tokens[0].split("/")[-1] if tokens[0] != "./gradlew" else "./gradlew"
        if binary not in RULES:
            continue
        allowed_subs = RULES[binary]
        if allowed_subs is None:
            return True
        if len(tokens) > 1 and tokens[1] in allowed_subs:
            return True
        if binary == "npm" and len(tokens) > 2 and tokens[1] == "run" and tokens[2] in {"build", "test"}:
            return True
    return False


def main():
    data = json.load(sys.stdin)
    command = data.get("tool_input", {}).get("command", "") or ""

    if not is_noisy(command):
        print("{}")
        return

    b64 = base64.b64encode(command.encode()).decode()
    wrapped = (
        "bash -c '"
        "set -o pipefail; "
        "LOG=$(mktemp); "
        f"echo {b64} | base64 -d | bash > \"$LOG\" 2>&1; "
        "STATUS=$?; "
        "TOTAL=$(wc -l < \"$LOG\"); "
        "ERR=$(grep -inE \"error|exception|fail|fatal\" \"$LOG\" | tail -50); "
        "if [ -n \"$ERR\" ]; then echo \"--- CHYBOVE RIADKY ---\"; echo \"$ERR\"; fi; "
        "echo \"--- POSLEDNYCH 30 RIADKOV (z celkovo $TOTAL) ---\"; "
        "tail -30 \"$LOG\"; "
        "rm -f \"$LOG\"; "
        "exit $STATUS"
        "'"
    )

    print(json.dumps({
        "hookSpecificOutput": {
            "hookEventName": "PreToolUse",
            "permissionDecision": "allow",
            "updatedInput": {"command": wrapped},
        }
    }))


if __name__ == "__main__":
    main()
