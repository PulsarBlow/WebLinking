# Contributing

Contributions are highly welcome!
However, except for very small changes, kindly file an issue and let's have a discussion before you open a pull request.

## Building The Project

Clone this repo:

```bash
git clone https://github.com/pulsarblow/weblinking
```

Change directory to repo root:

```bash
cd weblinking
```

Build a release and test it:

```bash
./scripts/test-release.sh
```

This will result in the following:

-   Restore all NuGet packages required for building
-   Build a release and run tests against it
-   Compute coverage (into `./.build` temporary directory)
