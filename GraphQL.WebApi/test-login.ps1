# Test Login and Password Hashing
Write-Host "🔐 Testing Login Functionality..." -ForegroundColor Yellow

# Test password hashing
Write-Host "📝 Testing password hashing..." -ForegroundColor Cyan

$password = "admin123"
$bytes = [System.Text.Encoding]::UTF8.GetBytes($password)
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$hashBytes = $sha256.ComputeHash($bytes)
$hash = [Convert]::ToBase64String($hashBytes)

Write-Host "Password: $password" -ForegroundColor White
Write-Host "Hash: $hash" -ForegroundColor White
Write-Host ""

# Test GraphQL API connection
Write-Host "🌐 Testing GraphQL API connection..." -ForegroundColor Cyan

try {
    $query = @"
    query {
        users {
            username
            email
            firstName
            lastName
            isActive
        }
    }
"@

    $body = @{
        query = $query
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method Post -Body $body -ContentType "application/json" -SkipCertificateCheck

    if ($response.data.users) {
        Write-Host "✅ GraphQL API is accessible!" -ForegroundColor Green
        Write-Host "📊 Found users:" -ForegroundColor Yellow
        foreach ($user in $response.data.users) {
            Write-Host "   - $($user.username) ($($user.firstName) $($user.lastName))" -ForegroundColor White
        }
    } else {
        Write-Host "❌ No users found in database" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Error connecting to GraphQL API: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "💡 Make sure the GraphQL API is running on https://localhost:5001" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🔑 Expected login credentials:" -ForegroundColor Yellow
Write-Host "   Username: admin, Password: admin123" -ForegroundColor White
Write-Host "   Username: user, Password: user123" -ForegroundColor White
Write-Host ""
Write-Host "🌐 MVC App URL: https://localhost:5231" -ForegroundColor Yellow 