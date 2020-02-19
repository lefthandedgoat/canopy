#!/usr/bin/env bash

set -eu
set -o pipefail

echo "Restoring dotnet tools..."
dotnet tool restore

PAKET_SKIP_RESTORE_TARGETS=true FAKE_DETAILED_ERRORS=true dotnet fake build -t "$@"
