#!/bin/sh
echo "== Publishing Nuget packages"
echo ""

echo "-- Pushing *.nupkg"
dotnet nuget push .build/nuget/**/*.nupkg -k "$NUGET_TOKEN" -sk "$NUGET_TOKEN" --skip-duplicate -s https://api.nuget.org/v3/index.json
echo ""

echo "-- Pushing *.snupkg"
dotnet nuget push .build/nuget/**/*.snupkg -k "$NUGET_TOKEN" -sk "$NUGET_TOKEN" --skip-duplicate
echo ""

echo "== Publication successfull"
