$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
#$version = [System.Reflection.Assembly]::LoadFile("$root\SoSimpleDb\bin\$configuration\SoSimpleDb.dll").GetName().Version
#$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)
#$versionStr = "{0}.{1}.{2}" -f (1, 0, $env:APPVEYOR_BUILD_NUMBER)
$versionStr = $env:APPVEYOR_BUILD_VERSION

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\NuGet\SoSimpleDb.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\NuGet\SoSimpleDb.compiled.nuspec

& $root\NuGet\NuGet.exe pack $root\NuGet\SoSimpleDb.compiled.nuspec