#!/usr/bin/env bash
set -euo pipefail

echo "Packing MyLib into local artifacts..."
mkdir -p artifacts
dotnet pack Parent/src/MyLib/MyLib.csproj -c Release -o artifacts

echo "Adding local artifacts folder as NuGet source (name 'localfeed')..."
dotnet nuget add source ./artifacts -n localfeed || true

echo "Building ChildApp.Package.csproj using the local package feed..."
dotnet restore ./ChildApp/ChildApp.Package.csproj
dotnet build ./ChildApp/ChildApp.Package.csproj
dotnet run --project ./ChildApp/ChildApp.Package.csproj

echo "Removing local NuGet source (localfeed)..."
dotnet nuget remove source localfeed || true

echo "Done"
