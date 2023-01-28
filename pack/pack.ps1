If (Test-Path .\Areas) {
	Remove-Item -Path .\Areas -Recurse -Force
}

Copy-Item -Path ..\src\Areas -Destination .\Areas -Recurse -Force

Get-ChildItem -Path *.cs -Recurse -File | ForEach-Object {
    Rename-Item $_ ($_.FullName + ".pp") -Force
}

Get-ChildItem -Path *.cshtml -Recurse -File | ForEach-Object {
    Rename-Item $_ ($_.FullName + ".pp") -Force
}

Get-ChildItem -Path *.cs.pp -Recurse -File | ForEach-Object {
    $content = Get-Content -Path $_
    $content | ForEach-Object { $_ -replace "RouteDebugger.Areas", '$rootnamespace$.Areas' } | Set-Content $_
}

..\src\.nuget\NuGet.exe pack BlueFenix.WebApiRouteDebugger.nuspec