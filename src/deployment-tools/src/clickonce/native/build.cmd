@if not defined _echo @echo off
setlocal

:SetupArgs
:: Initialize the args that will be passed to cmake
set __nativeWindowsDir=%~dp0Windows
set __CMakeBinDir=""
set __IntermediatesDir=""
set __BuildArch=x64
set __appContainer=""
set __VCBuildArch=x86_amd64
set CMAKE_BUILD_TYPE=Debug
set "__LinkArgs= "
set "__LinkLibraries= "
set __PortableBuild=0
set __IncrementalNativeBuild=0
set __DotnetInstallDir=%~dp0..\..\..\.dotnet
set __AppHostLibDir=""

:Arg_Loop
if [%1] == [] goto :ToolsVersion
if /i [%1] == [Release]        ( set CMAKE_BUILD_TYPE=Release&&shift&goto Arg_Loop)
if /i [%1] == [Debug]          ( set CMAKE_BUILD_TYPE=Debug&&shift&goto Arg_Loop)

if /i [%1] == [AnyCPU]         ( set __BuildArch=x64&&set __VCBuildArch=x86_amd64&&shift&goto Arg_Loop)
if /i [%1] == [x86]            ( set __BuildArch=x86&&set __VCBuildArch=x86&&shift&goto Arg_Loop)
if /i [%1] == [arm]            ( set __BuildArch=arm&&set __VCBuildArch=x86_arm&&shift&goto Arg_Loop)
if /i [%1] == [x64]            ( set __BuildArch=x64&&set __VCBuildArch=x86_amd64&&shift&goto Arg_Loop)
if /i [%1] == [amd64]          ( set __BuildArch=x64&&set __VCBuildArch=x86_amd64&&shift&goto Arg_Loop)
if /i [%1] == [arm64]          ( set __BuildArch=arm64&&set __VCBuildArch=x86_arm64&&shift&goto Arg_Loop)

if /i [%1] == [portable]       ( set __PortableBuild=1&&shift&goto Arg_Loop)
if /i [%1] == [rid]            ( set __TargetRid=%2&&shift&&shift&goto Arg_Loop)
if /i [%1] == [toolsetDir]     ( set "__ToolsetDir=%2"&&shift&&shift&goto Arg_Loop)
if /i [%1] == [nativever]      ( set __NativeVersion=%2&&shift&&shift&goto Arg_Loop)
if /i [%1] == [netcorepkgver]  ( set __NetCorePkgVersion=%2&&shift&&shift&goto Arg_Loop)
if /i [%1] == [commit]         ( set __CommitSha=%2&&shift&&shift&goto Arg_Loop)

if /i [%1] == [incremental-native-build] ( set __IncrementalNativeBuild=1&&shift&goto Arg_Loop)
if /i [%1] == [dotnetInstallDir]     ( set __DotnetInstallDir=%~2&&shift&&shift&goto Arg_Loop)
if /i [%1] == [appHostLibDir]     ( set __AppHostLibDir=%~2&&shift&&shift&goto Arg_Loop)
if /i [%1] == [rootDir]        ( set __rootDir=%2&&shift&&shift&goto Arg_Loop)

shift
goto :Arg_Loop

:ToolsVersion

if defined VisualStudioVersion goto :RunVCVars

set _VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if exist %_VSWHERE% (
  for /f "usebackq tokens=*" %%i in (`%_VSWHERE% -latest -prerelease -property installationPath`) do set _VSCOMNTOOLS=%%i\Common7\Tools
)
if not exist "%_VSCOMNTOOLS%" set _VSCOMNTOOLS=%VS140COMNTOOLS%
if not exist "%_VSCOMNTOOLS%" goto :MissingVersion

set VSCMD_START_DIR="%~dp0"
call "%_VSCOMNTOOLS%\VsDevCmd.bat"

:RunVCVars
if "%VisualStudioVersion%"=="17.0" (
    goto :VS2022
)

:MissingVersion
:: Can't find VS 2017, 2019, 2022
echo Error: Visual Studio 2017, 2019 or 2022 required
echo        Please see https://github.com/dotnet/runtime/tree/main/docs/installer/building/windows-instructions.md for build instructions.
exit /b 1

:VS2022
:: Setup vars for VS2022
set __PlatformToolset=v143
set __VSVersion=17 2022
:: Set the environment for the native build
call "%VS170COMNTOOLS%..\..\VC\Auxiliary\Build\vcvarsall.bat" %__VCBuildArch%

:SetupDirs
if [%__rootDir%] == [] (
    echo Root directory must be provided via the rootDir parameter.
    exit /b 1
)

set __binDir=%__rootDir%\artifacts\bin
set __objDir=%__rootDir%\artifacts\obj

:: Setup to cmake the native components
echo Commencing build of corehost
echo.

if %__CMakeBinDir% == "" (
    set "__CMakeBinDir=%__binDir%\%__TargetRid%.%CMAKE_BUILD_TYPE%"
)

if %__IntermediatesDir% == "" (
    set "__IntermediatesDir=%__objDir%\%__TargetRid%.%CMAKE_BUILD_TYPE%\native"
)
set "__ResourcesDir=%__objDir%\%__TargetRid%.%CMAKE_BUILD_TYPE%\nativeResourceFiles"
set "__CMakeBinDir=%__CMakeBinDir:\=/%"
set "__IntermediatesDir=%__IntermediatesDir:\=/%"


:: Check that the intermediate directory exists so we can place our cmake build tree there
if /i "%__IncrementalNativeBuild%" == "1" goto CreateIntermediates
if exist "%__IntermediatesDir%" rd /s /q "%__IntermediatesDir%"

:CreateIntermediates
if not exist "%__IntermediatesDir%" md "%__IntermediatesDir%"

if exist "%VSINSTALLDIR%DIA SDK" goto GenVSSolution
echo Error: DIA SDK is missing at "%VSINSTALLDIR%DIA SDK". ^
Did you install all the requirements for building on Windows, including the "Desktop Development with C++" workload? ^
Please see https://github.com/dotnet/runtime/blob/main/docs/workflow/requirements/windows-requirements.md ^
Another possibility is that you have a parallel installation of Visual Studio and the DIA SDK is there. In this case it ^
may help to copy its "DIA SDK" folder into "%VSINSTALLDIR%" manually, then try again.
exit /b 1

:GenVSSolution
:: Regenerate the VS solution

echo Calling "%__nativeWindowsDir%\gen-buildsys-win.bat %~dp0 "%__VSVersion%" %__BuildArch% %__CommitSha% %__NativeVersion% %__NetCorePkgVersion% "%__DotnetInstallDir%" "%__AppHostLibDir%" %__PortableBuild%"
pushd "%__IntermediatesDir%"
call "%__nativeWindowsDir%\gen-buildsys-win.bat" %~dp0 "%__VSVersion%" %__BuildArch% %__CommitSha% %__NativeVersion% %__NetCorePkgVersion% "%__DotnetInstallDir%" "%__AppHostLibDir%" %__PortableBuild%
popd

:CheckForProj
:: Check that the project created by Cmake exists
if exist "%__IntermediatesDir%\INSTALL.vcxproj" goto BuildNativeProj
goto :Failure

:BuildNativeProj
:: Build the project created by Cmake
set __msbuildArgs=/p:Platform=%__BuildArch% /p:PlatformToolset="%__PlatformToolset%"

SET __NativeBuildArgs=/t:rebuild
if /i "%__IncrementalNativeBuild%" == "1" SET __NativeBuildArgs=

echo msbuild "%__IntermediatesDir%\INSTALL.vcxproj" %__NativeBuildArgs% /m /p:Configuration=%CMAKE_BUILD_TYPE% %__msbuildArgs%
call msbuild "%__IntermediatesDir%\INSTALL.vcxproj" %__NativeBuildArgs% /m /p:Configuration=%CMAKE_BUILD_TYPE% %__msbuildArgs%
IF ERRORLEVEL 1 (
    goto :Failure
)
echo Done building Native components
exit /B 0

:Failure
:: Build failed
echo Failed to generate native component build project!
exit /b 1
