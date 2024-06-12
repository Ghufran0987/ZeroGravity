Add-Migration -Name ZeroGravityContext-v16  -Context ZeroGravityContext

-- Set ASPNETCORE_ENVIRONMENT before running update else it uses Development ENVIRONMENT as default
$env:ASPNETCORE_ENVIRONMENT='Production'
$env:ASPNETCORE_ENVIRONMENT='Development'
update-database -Context ZeroGravityContext


Note:
After Migration Run Sp and Trigger from DB Project SQL Folder One by One