#!/usr/bin/env bash

set -e

cd Solution

dotnet restore

find . -name '*.Tests.csproj' -print0 | xargs -0 -I{} dotnet test --filter "TestCategory!=SQL-Server-Required" {}
