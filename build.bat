cls 
@echo off 

set buildDirX64=%cd%\Build\x64

if not exist "%buildDirX64%" (
    mkdir "%buildDirX64%"
)

set buildDirX86=%cd%\Build\x86

if not exist "%buildDirX86%" (
    mkdir "%buildDirX86%"
)

echo ***************************************

echo Building Inputs.NET (x86)...
devenv "%cd%\src\Inputs\Inputs.csproj" /Clean "Release|x86"
devenv "%cd%\src\Inputs\Inputs.csproj" /Rebuild "Release|x86"

echo ***************************************

echo Building Inputs.NET (x64)...
devenv "%cd%\src\Inputs\Inputs.csproj" /Clean "Release|x64"
devenv "%cd%\src\Inputs\Inputs.csproj" /Rebuild "Release|x64"

echo ***************************************

echo Done.

timeout /t 5