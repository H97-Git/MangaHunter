#!/usr/bin/env bash
cd ~/Source/Rider-Projects/MangaHunter
dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer -c Release ./MangaHunter.API/*.csproj
dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer -c Release ./MangaHunter.BlazorServer/*.csproj
