@echo off
set version=%1
set path_build=D:\work\DP\build
set path_frontend=D:\work\DP\Frontend
set path_backend=D:\work\DP\Backend
set path_frontend_publish=%path_frontend%\bin\Debug\netcoreapp2.2\publish
set path_backend_publish=%path_backend%\bin\Debug\netcoreapp2.2\publish

if defined version (
	cd %path_frontend%
	dotnet publish
	cd %path_backend%
	dotnet publish
	
	cd..
	if not exist %path_build% (
		mkdir build
	)
	cd build
	if not exist %version% (
		mkdir %version%
		cd %version%
	
		mkdir Frontend
		cd Frontend
		xcopy %path_frontend_publish% /S
		cd..
		mkdir Backend
		cd Backend
		xcopy %path_backend_publish% /S
		
		cd..
		echo start dotnet Frontend\Frontend.dll > run.bat
		echo start dotnet Backend\Backend.dll >> run.bat

		echo taskkill /IM dotnet.exe /F > stop.bat
	) else (
		cd..
		echo This version already exists.
	)
) else ( 
	echo Version are required.
)