del *.nupkg
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe ".\MetacoClient\MetacoClient.csproj" -p:Configuration=Release

.\.nuget\NuGet.exe pack .\MetacoClient\MetacoClient.csproj -Prop Configuration=Release

forfiles /m *.nupkg /c "cmd /c .\.nuget\NuGet.exe push @FILE FEipouFE987F2BBO3UD8387A64Hhh -source http://metacobuildserver.cloudapp.net:81/" 
(((dir *.nupkg).Name) -match "[0-9]+?\.[0-9]+?\.[0-9]+?\.[0-9]+")
$ver = $Matches.Item(0)
