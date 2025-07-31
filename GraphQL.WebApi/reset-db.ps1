# Reset Database and Ensure Proper Seeding
Write-Host "Resetting Database..." -ForegroundColor Green

# Remove the database file
$dbPath = "GraphQL.WebApi.db"
if (Test-Path $dbPath) {
    Remove-Item $dbPath -Force
    Write-Host "Removed existing database: $dbPath" -ForegroundColor Yellow
}

# Remove migrations
$migrationsPath = "Migrations"
if (Test-Path $migrationsPath) {
    Remove-Item $migrationsPath -Recurse -Force
    Write-Host "Removed existing migrations" -ForegroundColor Yellow
}

# Create new migration
Write-Host "Creating new migration..." -ForegroundColor Yellow
dotnet ef migrations add InitialCreate

# Update database
Write-Host "Updating database..." -ForegroundColor Yellow
dotnet ef database update

Write-Host "Database reset completed!" -ForegroundColor Green
Write-Host "Now start the API to seed the database with users and customers." -ForegroundColor Cyan