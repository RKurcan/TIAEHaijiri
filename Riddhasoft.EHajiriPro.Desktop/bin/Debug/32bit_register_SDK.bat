@echo off
cd %cd%

copy *.dll %windir%\system32\

regsvr32.exe %windir%\system32\zkemkeeper.dll

exit