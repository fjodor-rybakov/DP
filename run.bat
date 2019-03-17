@echo off
start redis-server
set current_dir=%cd%
cd /D C:\Program Files\Redis
start redis-server conf\redis_8001.conf
start redis-server conf\redis_8002.conf
cd /D %current_dir%
start dotnet Frontend\Frontend.dll
start dotnet Backend\Backend.dll
start dotnet TextListener\TextListener.dll
start dotnet TextRankCalc\TextRankCalc.dll
setlocal enabledelayedexpansion
set cnt=0
for /f "usebackq tokens=1*" %%a in ("config.txt") do (
	set "a=%%a"& set "b=%%b"
	if !cnt!==2 (
		for /l %%n in (1,1,!b!) do (
			start dotnet VowelConsCounter\VowelConsCounter.dll
		)
	)
	if !cnt!==3 (
		for /l %%n in (1,1,!b!) do (
			start dotnet VowelConsRater\VowelConsRater.dll
		)
	)
	set /a cnt+=1
)
