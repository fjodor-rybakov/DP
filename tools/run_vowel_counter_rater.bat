@echo off
start dotnet TextRankCalc\TextRankCalc.dll
setlocal enabledelayedexpansion
set cnt=0
for /f "usebackq tokens=1*" %%a in ("config.txt") do (
	set "a=%%a"& set "b=%%b"
	if !cnt!==2 (for /l %%n in (1,1,!b!) do (start dotnet VowelConsCounter\VowelConsCounter.dll))
	if !cnt!==3 (for /l %%n in (1,1,!b!) do (start dotnet VowelConsRater\VowelConsRater.dll))
	set /a cnt+=1
)
