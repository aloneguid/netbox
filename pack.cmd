msbuild src/Aloneguid.Support.sln /p:Configuration=Release
del *.nupkg
nuget pack support.nuspec