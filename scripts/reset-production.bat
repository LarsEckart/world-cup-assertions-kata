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
    goto detect_applied
)

:parse
if "%~1"=="" goto check
if /I "%~1"=="all" (
    set "SELECTED=%PATCH_5% %PATCH_4% %PATCH_3% %PATCH_2% %PATCH_1%"
    goto check
)
if "%~1"=="1" (
    set "SELECTED=%PATCH_1% !SELECTED!"
) else if "%~1"=="2" (
    set "SELECTED=%PATCH_2% !SELECTED!"
) else if "%~1"=="3" (
    set "SELECTED=%PATCH_3% !SELECTED!"
) else if "%~1"=="4" (
    set "SELECTED=%PATCH_4% !SELECTED!"
) else if "%~1"=="5" (
    set "SELECTED=%PATCH_5% !SELECTED!"
) else (
    call :usage
    popd
    exit /b 1
)
shift
goto parse

:detect_applied
for %%P in (%PATCH_5% %PATCH_4% %PATCH_3% %PATCH_2% %PATCH_1%) do (
    git apply -R --check "%%P" >nul 2>nul
    if not errorlevel 1 (
        set "SELECTED=!SELECTED! %%P"
    )
)
if "!SELECTED!"=="" (
    echo No production-code mutations are currently applied.
    popd
    exit /b 0
)
goto check

:check
for %%P in (%SELECTED%) do (
    git apply -R --check "%%P" >nul 2>nul
    if errorlevel 1 (
        echo Cannot reset %%P because that mutation is not currently applied.
        echo Run scripts\reset-production.bat with no arguments to reset only applied mutations.
        popd
        exit /b 1
    )
)

for %%P in (%SELECTED%) do (
    git apply -R "%%P"
    if errorlevel 1 (
        popd
        exit /b 1
    )
    echo Reverted %%P
)

popd
exit /b 0

:usage
echo Usage: scripts\reset-production.bat [all^|1^|2^|3^|4^|5]...
echo.
echo Reverts production-code mutations applied by break-production.bat.
echo With no arguments, reverts the mutations that are currently applied.
exit /b 0
