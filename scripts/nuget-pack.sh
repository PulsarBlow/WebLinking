#!/bin/sh
echo "== Packaging Nuget packages"
echo ""

echo "-- Initializing"

# Set GIT_COMMIT if not already ambiant (env)
if [ -z "$GIT_COMMIT" ]; then
  echo "GIT_COMMIT not ambiant. Computing it now."
  GIT_COMMIT=$(git rev-parse HEAD)
fi
echo "GIT_COMMIT=$GIT_COMMIT"
echo ""

# Set GIT_BRANCH if not already ambiant (env)
if [ -z "$GIT_BRANCH" ]; then
  echo "GIT_BRANCH not ambiant. Computing it now."
  GIT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
fi
echo "GIT_BRANCH=$GIT_BRANCH"
echo ""

echo "-- Packaging"
rm -rf .build/nuget
dotnet pack -o .build/nuget --include-symbols -c Release -p:RepositoryBranch=$GIT_BRANCH -p:RepositoryCommit=$GIT_COMMIT
echo ""

echo "== Packaging successfull"
echo ""
