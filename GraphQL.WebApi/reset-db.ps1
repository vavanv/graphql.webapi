# Reset Database and Test Login
Write-Host "🔄 Resetting GraphQL Web API Database..." -ForegroundColor Yellow

# Navigate to the GraphQL API project
Set-Location "GraphQL.WebApi"

# Remove existing migrations
Write-Host "📁 Removing existing migrations..." -ForegroundColor Cyan
dotnet ef migrations remove --force

# Add new migration
Write-Host "📝 Adding new migration..." -ForegroundColor Cyan
dotnet ef migrations add InitialCreate

# Update database
Write-Host "🗄️ Updating database..." -ForegroundColor Cyan
dotnet ef database update

Write-Host "✅ Database reset complete!" -ForegroundColor Green
Write-Host ""
Write-Host "🔑 Test Credentials:" -ForegroundColor Yellow
Write-Host "   Username: admin" -ForegroundColor White
Write-Host "   Password: admin123" -ForegroundColor White
Write-Host ""
Write-Host "   Username: user" -ForegroundColor White
Write-Host "   Password: user123" -ForegroundColor White
Write-Host ""
Write-Host "🌐 Start the applications:" -ForegroundColor Yellow
Write-Host "   1. GraphQL API: cd GraphQL.WebApi && dotnet run" -ForegroundColor White
Write-Host "   2. MVC App: cd GraphQL.WebApi.Mvc && dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "📱 Access URLs:" -ForegroundColor Yellow
Write-Host "   GraphQL API: https://localhost:5001/graphql" -ForegroundColor White
Write-Host "   MVC App: https://localhost:5231" -ForegroundColor White