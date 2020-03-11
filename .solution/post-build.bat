set sln=%1
set prj=%2
set out=%3
set key=%4
set ngs=F:\Work\Jetproger\Jetproger.Nuget\Jetproger.Nuget\Packages\
set ngr=C:\inetpub\wwwroot\Jetproger.Nuget\Packages\

copy /Y "%out%%key%.dll" "%sln%%key%.dll"
copy /Y "%prj%%key%.nuspec" "%sln%%key%.nuspec"
"%VS140COMNTOOLS%nuget.exe" pack "%sln%%key%.nuspec"
del /Q /S "%sln%%key%.dll"
del /Q /S "%sln%%key%.nuspec"

copy /Y "%prj%%key%.1.0.0.nupkg" "%ngs%%key%.1.0.0.nupkg"
copy /Y "%prj%%key%.1.0.0.nupkg" "%ngr%%key%.1.0.0.nupkg"
del /Q /S "%prj%%key%.1.0.0.nupkg"
