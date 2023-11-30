set sln=%1
set prj=%2
set out=%3
set key=%4

cd ..\.setting
for %%f in (*.config) do (call :create_app "%%f")

rmdir /s /q %out% 

goto :eof

:create_app
set cfg=%~nx1
set exe=%~n1
for /f "tokens=1 delims=." %%n in ("%cfg%") do (set own=%%n)
for /f "tokens=2 delims=." %%n in ("%cfg%") do (set app=%%n)
set dir=.%own%.%app%
mkdir ..\..\%dir%
copy /Y "%out%*.*" "..\..\%dir%\*.*"
copy /Y "%1" "..\..\%dir%\"
move ..\..\%dir%\Jetproger.Service.exe ..\..\%dir%\%own%.%app%.exe
move ..\..\%dir%\Jetproger.Service.pdb ..\..\%dir%\%own%.%app%.pdb 
exit /b

:eof