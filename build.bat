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
		
		xcopy %work_dir%\run.bat %work_dir%\build\%version%
		xcopy %work_dir%\stop.bat %work_dir%\build\%version%
		xcopy %work_dir%\config.txt %work_dir%\build\%version%
	) else (
		cd..
		echo This version already exists.
	)
) else (
	echo Version are required.
)

cd..
cd..