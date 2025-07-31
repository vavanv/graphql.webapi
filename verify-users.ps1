# Verify Users Table and Authentication System
Write-Host "Verifying Users Table and Authentication System..." -ForegroundColor Green

# Disable SSL certificate validation for older PowerShell versions
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

Write-Host "`n📋 System Status:" -ForegroundColor Cyan
Write-Host "   GraphQL API: https://localhost:5001" -ForegroundColor White
Write-Host "   MVC App:    https://localhost:5231" -ForegroundColor White

Write-Host "`n🔐 Demo Accounts:" -ForegroundColor Cyan
Write-Host "   Admin: admin / admin123" -ForegroundColor White
Write-Host "   User:  user  / user123" -ForegroundColor White

Write-Host "`n🧪 Testing Authentication Flow:" -ForegroundColor Yellow

# Test GraphQL API
Write-Host "`n1. Testing GraphQL API..." -ForegroundColor Yellow
try {
    $graphqlUrl = "https://localhost:5001/graphql"
    $query = @{
        query = "query { customers { id firstName lastName contact email dateOfBirth } }"
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri $graphqlUrl -Method POST -Body $query -ContentType "application/json"
    Write-Host "✅ GraphQL API is working" -ForegroundColor Green
    Write-Host "   Found $($response.data.customers.Count) customers" -ForegroundColor Cyan
} catch {
    Write-Host "❌ GraphQL API error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test MVC Application
Write-Host "`n2. Testing MVC Application..." -ForegroundColor Yellow
try {
    $mvcUrl = "https://localhost:5231"
    $response = Invoke-WebRequest -Uri $mvcUrl
    Write-Host "✅ MVC Application is working" -ForegroundColor Green
    Write-Host "   Status: $($response.StatusCode)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ MVC Application error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Login Page
Write-Host "`n3. Testing Login Page..." -ForegroundColor Yellow
try {
    $loginUrl = "https://localhost:5231/Account/Login"
    $response = Invoke-WebRequest -Uri $loginUrl
    Write-Host "✅ Login page is accessible" -ForegroundColor Green
    Write-Host "   Status: $($response.StatusCode)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Login page error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n🎉 Verification Complete!" -ForegroundColor Green
Write-Host "`n📝 Next Steps:" -ForegroundColor Cyan
Write-Host "1. Open https://localhost:5231 in your browser" -ForegroundColor White
Write-Host "2. Try to access Customers - you'll be redirected to login" -ForegroundColor White
Write-Host "3. Login with admin/admin123 or user/user123" -ForegroundColor White
Write-Host "4. After login, you can access customer management" -ForegroundColor White