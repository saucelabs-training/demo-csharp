Param(
[string]$rdcVodQaNativeAppApiKey,
[string]$rdcSauceDemoIosRdcApiKey,
[string]$rdcSauceDemoAndroidAppApiKey
)

Write-Output "sauce.rdc.VodQaNativeAppApiKey stored in Azure DevOps=>$rdcVodQaNativeAppApiKey"
Write-Output "sauce.rdc.SauceDemoIosRdcApiKey stored in Azure DevOps=>$rdcSauceDemoIosRdcApiKey"
Write-Output "sauce.rdc.SauceDemoAndroidAppApiKey stored in Azure DevOps=>$rdcSauceDemoAndroidAppApiKey"

[Environment]::SetEnvironmentVariable("VODQC_RDC_API_KEY", "$rdcVodQaNativeAppApiKey", "User")
[Environment]::SetEnvironmentVariable("SAUCE_DEMO_IOS_RDC_API_KEY", "$rdcSauceDemoIosRdcApiKey", "User")
[Environment]::SetEnvironmentVariable("RDC_SAUCE_DEMO_ANDROID_KEY", "$rdcSauceDemoIosRdcApiKey", "User")
