#!/usr/bin/env bash
set -e
set -o pipefail

source .env
mono .paket/paket.exe restore
mono packages/build/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
