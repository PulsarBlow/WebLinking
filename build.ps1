$ErrorActionPreference = "Stop"

Function ResolveAndForcePath([string]$relativePath) {
    mkdir -Force $relativePath | Out-Null
    return (Resolve-Path $relativePath).Path
}
Function WriteAndExecute([string]$command) {
    Write-Output $command
    Invoke-Expression $command
}
Function DefaultValue($value, $default) {
	if ($null -eq $value) { $default } else { $value }
}
Function EnsureTrailingSlash([string]$path) {
	if ($path[-1] -ne '/' -and $path[-1] -ne '\') { $path + '/' } else { $path }
}

$toolsLocation = DefaultValue $toolsLocation '.tools'
$toolsLocation = EnsureTrailingSlash $toolsLocation
$outputLocation = DefaultValue $outputLocation '.artifacts'
$outputLocation = EnsureTrailingSlash $outputLocation
$testProjectLocations = Get-ChildItem 'src' -Filter *.Tests.* | ForEach-Object FullName

Write-Output "tools         : $toolsLocation"
Write-Output "output        : $outputLocation"
Write-Output "test projects : " ($testProjectLocations -join ', ')

$toolsPath = ResolveAndForcePath $toolsLocation;
$outputPath = ResolveAndForcePath $outputLocation
$mergeFile = Join-Path $outputPath -childpath 'coverage.json'
$uploadFile = Join-Path $outputPath -childpath 'coverage.opencover.xml'

Remove-Item ($outputPath + '*') -Force -Recurse

Write-Output "tools         : $toolsPath"
Write-Output "output        : $outputPath"
Write-Output "mergeFile     : $mergeFile"
Write-Output "uploadFile    : $uploadFile"
Write-Output "test projects : " ($testProjectLocations -join ', ')

# Build solution
WriteAndExecute 'dotnet build --no-incremental --configuration Release'

# Build packages
WriteAndExecute "dotnet pack --configuration Release --no-restore --output $outputPath"

Push-Location
try {
    # Run tests & collect coverage
    foreach ($testProjectLocation in $testProjectLocations) {
        Set-Location $testProjectLocation
        WriteAndExecute "dotnet test /p:CollectCoverage=true /p:Include=`"[WebLinking.*]*`" /p:ExcludeByAttribute=System.Diagnostics.DebuggerNonUserCodeAttribute /p:CoverletOutput=`"${outputPath}`" /p:MergeWith=`"${mergeFile}`" /p:CoverletOutputFormat=opencover%2Cjson ${dotnetTestArgs}"
    }

    WriteAndExecute "dotnet tool install dotnet-reportgenerator-globaltool --tool-path `"${toolsPath}`""
    WriteAndExecute ". `"${toolsPath}reportgenerator`" -reports:`"${uploadFile}`" -targetdir:`"${outputPath}`""
    Set-Location $outputPath
    ./index.htm

} finally { Pop-Location }

