@echo off

set directory=..\publish\

msbuild /t:Clean,Build ^
        /p:Configuration=Release ^
        /p:TargetFrameworkVersion=v3.5 ^
        /p:OutputPath="..\..\..\app\binaries" ^
        /p:PublishProfile=FolderProfile ^
        /p:SkipInvalidConfigurations=true ^
        /p:PlatformTarget=x86 ^
..\src\SmartOpt\SmartOpt\ >> nul

rmdir /s /q ..\app\binariesapp.publish

echo @echo off > ..\app\SmartOpt.cmd
echo start ./binaries/SmartOpt.exe >> ..\app\SmartOpt.cmd
