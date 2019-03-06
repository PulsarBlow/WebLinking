# Contributing

Contributions are highly welcome, however, except for very small changes, kindly file an issue and let's have a discussion before you open a pull request.

## Building The Project

Clone this repo:

```bash
git clone https://github.com/pulsarblow/weblinking
```

Change directory to repo root:

```bash
cd weblinking
```

Execute build script:

```bash
.\build.ps1
```

This will result in the following:

-   Restore all NuGet packages required for building
-   Build and publish all projects. Final binaries are placed into `<repo_root>\.artifacts\<Configuration>`
-   Build and run tests
-   Compute coverage
