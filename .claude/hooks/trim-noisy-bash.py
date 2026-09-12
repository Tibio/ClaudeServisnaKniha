#!/usr/bin/env python3
"""PreToolUse hook (Bash matcher): rewrites noisy install/build/test commands
so only the last 30 lines plus any error-looking lines reach the model context,
instead of the full raw output."""
import json
import sys
import base64

NOISY_MARKERS = (
    "npm install", "npm ci", "npm run build", "npm test", "npm run test",
    "yarn install", "yarn build", "yarn test",
    "pnpm install", "pnpm build", "pnpm test",
    "make ", "cargo build", "cargo test",
    "go build", "go test",
    "mvn ", "gradle", "dotnet build", "dotnet test",
    "pip install",
)


def main():
    data = json.load(sys.stdin)
    command = data.get("tool_input", {}).get("command", "") or ""

    if not any(marker in command for marker in NOISY_MARKERS):
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
