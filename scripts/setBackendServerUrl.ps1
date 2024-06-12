param (
    [String]$ServerUrl = "",
    [String]$settingsFile = "./ZeroGravity.Mobile/ZeroGravity.Mobile/Contract/Constants/Common.cs",
    [String]$projDirPath = "./"
)
Write-Host "***** Environment Variables *****"
Write-Host "ServerUrl: $ServerUrl"
Write-Host "settingsFile: $settingsFile"
Write-Host "projDirPath: $projDirPath"

Write-Host "***** Change to project Directory *****"
Set-Location $projDirPath

Write-Host "***** replace variables *****"

(Get-Content $settingsFile -Raw) -replace '(?m)(^\s+public static string ServerUrl = ")(.+)(";)' , "`$1$ServerUrl`$3" | Set-Content $settingsFile

Write-Host "***** output changed file *****"
Get-Content $settingsFile