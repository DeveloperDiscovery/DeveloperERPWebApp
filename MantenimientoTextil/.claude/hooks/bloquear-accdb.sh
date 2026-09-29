#!/usr/bin/env bash
# PreToolUse: impide editar archivos .accdb (artefacto generado).
input=$(cat)
if echo "$input" | grep -qiE '"file_path"[[:space:]]*:[[:space:]]*"[^"]*\.(accdb|laccdb)"'; then
  echo "Bloqueado: el .accdb es un artefacto generado. Cambia db/schema/*.sql." >&2
  exit 2
fi
exit 0
