@echo off
setlocal enabledelayedexpansion

pushd "%~dp0.." || exit /b 1

set "PATCH_1=scripts\mutations\01-allow-duplicate-shirt-numbers.patch"
set "PATCH_2=scripts\mutations\02-add-manuel-neuer.patch"
set "PATCH_3=scripts\mutations\03-remove-united-states-goalkeeper.patch"
set "PATCH_4=scripts\mutations\04-invert-game-points.patch"
set "PATCH_5=scripts\mutations\05-allow-points-for-team-that-did-not-play.patch"
set "SELECTED="

if "%~1"=="" (
    set "SELECTED=%PATCH_1% %PATCH_2% %PATCH_3% %PATCH_4% %PATCH_5%"
    goto check
)

:parse
if "%~1"=="" goto check
if /I "%~1"=="all" (
    set "SELECTED=%PATCH_1% %PATCH_2% %PATCH_3% %PATCH_4% %PATCH_5%"
    goto check
)
if "%~1"=="1" (
    set "SELECTED=!SELECTED! %PATCH_1%"
) else if "%~1"=="2" (
    set "SELECTED=!SELECTED! %PATCH_2%"
) else if "%~1"=="3" (
    set "SELECTED=!SELECTED! %PATCH_3%"
) else if "%~1"=="4" (
    set "SELECTED=!SELECTED! %PATCH_4%"
) else if "%~1"=="5" (
    set "SELECTED=!SELECTED! %PATCH_5%"
) else (
    call :usage
    popd
    exit /b 1
)
shift
goto parse

:check
for %%P in (%SELECTED%) do (
    git apply --check "%%P"
    if errorlevel 1 (
        popd
        exit /b 1
    )
)

for %%P in (%SELECTED%) do (
    git apply "%%P"
    if errorlevel 1 (
        popd
        exit /b 1
    )
    echo Applied %%P
)

popd
exit /b 0

:usage
echo Usage: scripts\break-production.bat [all^|1^|2^|3^|4^|5]...
echo.
echo Applies small production-code mutations so the kata tests fail.
echo With no arguments, applies all mutations.
exit /b 0
