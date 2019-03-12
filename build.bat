@echo off
set version=%1
set work_dir=D:\work\DP

set path_build=%work_dir%\build
set path_frontend=%work_dir%\Frontend
set path_backend=%work_dir%\Backend
set path_text_lisener=%work_dir%\TextListener
set path_text_rank_calc=%work_dir%\TextRankCalc
set path_vowel_cons_counter=%work_dir%\VowelConsCounter
set path_vowel_cons_rater=%work_dir%\VowelConsRater

set path_frontend_publish=%path_frontend%\bin\Debug\netcoreapp2.2\publish
set path_backend_publish=%path_backend%\bin\Debug\netcoreapp2.2\publish
set path_text_lisener_publish=%path_text_lisener%\bin\Debug\netcoreapp2.2\publish
set path_text_rank_calc_publish=%path_text_rank_calc%\bin\Debug\netcoreapp2.2\publish
set path_vowel_cons_counter_publish=%path_vowel_cons_counter%\bin\Debug\netcoreapp2.2\publish
set path_vowel_cons_rater_publish=%path_vowel_cons_rater%\bin\Debug\netcoreapp2.2\publish

if defined version (
	cd %path_frontend%
	dotnet publish
	cd %path_backend%
	dotnet publish
	cd %path_text_lisener%
	dotnet publish
	cd %path_text_rank_calc%
	dotnet publish
	cd %path_vowel_cons_counter%
	dotnet publish
	cd %path_vowel_cons_rater%
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
		mkdir TextListener
		cd TextListener
		xcopy %path_text_lisener_publish% /S
		cd..
		mkdir TextRankCalc
		cd TextRankCalc
		xcopy %path_text_rank_calc_publish% /S
		cd..
		mkdir VowelConsCounter
		cd VowelConsCounter
		xcopy %path_vowel_cons_counter_publish% /S
		cd..
		mkdir VowelConsRater
		cd VowelConsRater
		xcopy %path_vowel_cons_rater_publish% /S
		cd..
		
		echo port_frontend:5001 > config.txt
		echo port_backend:5000 >> config.txt

		echo start redis-server > run.bat
		echo start dotnet Frontend\Frontend.dll >> run.bat
		echo start dotnet Backend\Backend.dll >> run.bat
		echo start dotnet TextListener\TextListener.dll >> run.bat
		echo start dotnet TextRankCalc\TextRankCalc.dll >> run.bat
		echo start dotnet VowelConsCounter\VowelConsCounter.dll >> run.bat
		echo start dotnet VowelConsRater\VowelConsRater.dll >> run.bat
		
		echo taskkill /IM dotnet.exe /F > stop.bat
		echo taskkill /IM redis-server.exe /F >> stop.bat
	) else (
		cd..
		echo This version already exists.
	)
) else ( 
	echo Version are required.
)