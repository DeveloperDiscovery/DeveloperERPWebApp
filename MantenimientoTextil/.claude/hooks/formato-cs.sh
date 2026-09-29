#!/usr/bin/env bash
# PostToolUse: compila tras editar .cs (si dotnet está disponible).
input=$(cat)
echo "$input" | grep -qE '"file_path"[[:space:]]*:[[:space:]]*"[^"]*\.cs"' || exit 0
command -v dotnet >/dev/null 2>&1 || exit 0
cd "$CLAUDE_PROJECT_DIR" && dotnet build MantenimientoTextil.sln -nologo -v q 2>&1 | tail -15
exit 0
