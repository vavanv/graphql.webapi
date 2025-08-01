# Test Users in Database
Write-Host "🔍 Testing Users in Database..." -ForegroundColor Yellow

try {
    $query = @"
    query {
        users {
            id
            username
            email
            firstName
            lastName
            isActive
            passwordHash
        }
    }
"@

    $body = @{
        query = $query
    } | ConvertTo-Json

    Write-Host "🌐 Connecting to GraphQL API..." -ForegroundColor Cyan
    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method Post -Body $body -ContentType "application/json" -SkipCertificateCheck

    if ($response.data.users) {
        Write-Host "✅ GraphQL API is accessible!" -ForegroundColor Green
        Write-Host "📊 Found users:" -ForegroundColor Yellow
        foreach ($user in $response.data.users) {
            Write-Host "   - ID: $($user.id)" -ForegroundColor White
            Write-Host "     Username: $($user.username)" -ForegroundColor White
            Write-Host "     Email: $($user.email)" -ForegroundColor White
            Write-Host "     Name: $($user.firstName) $($user.lastName)" -ForegroundColor White
            Write-Host "     Active: $($user.isActive)" -ForegroundColor White
            Write-Host "     Password Hash: $($user.passwordHash)" -ForegroundColor White
            Write-Host ""
        }
    } else {
        Write-Host "❌ No users found in database" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Error connecting to GraphQL API: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "💡 Make sure the GraphQL API is running on https://localhost:5001" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🔑 Testing password hashing..." -ForegroundColor Yellow
$password = "admin123"
$bytes = [System.Text.Encoding]::UTF8.GetBytes($password)
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$hashBytes = $sha256.ComputeHash($bytes)
$hash = [Convert]::ToBase64String($hashBytes)

Write-Host "Password: $password" -ForegroundColor White
Write-Host "Expected Hash: $hash" -ForegroundColor White 