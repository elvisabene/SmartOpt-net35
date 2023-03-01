@echo off

set directory=..\publish\

msbuild /t:Clean,Build ^
        /p:Configuration=Release ^
        /p:TargetFrameworkVersion=v3.5 ^
        /p:OutputPath="..\..\..\publish\bin" ^
        /p:PublishProfile=FolderProfile ^
        /p:SkipInvalidConfigurations=true ^
        /p:PlatformTarget=x86 ^
..\src\SmartOpt\SmartOpt\ >> nul

rmdir /s /q ..\publish\binapp.publish
