TargetFramework=$1
ProjectName=$2

"..\..\oqtane.framework-6.1.2-Source\oqtane.package\nuget.exe" pack %ProjectName%.nuspec -Properties targetframework=%TargetFramework%;projectname=%ProjectName%
cp -f "*.nupkg" "..\..\oqtane.framework-6.1.2-Source\Oqtane.Server\Packages\"