# Test Login Fix
Write-Host "Testing Login Fix..." -ForegroundColor Yellow

# Disable SSL certificate validation
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

try {
    # Test getting user with password hash
    $query = @"
    query {
        user(username: "admin") {
            id
            username
            email
            passwordHash
            firstName
            lastName
            isActive
        }
    }
"@

    $body = @{
        query = $query
    } | ConvertTo-Json

    Write-Host "Testing GraphQL API for admin user..." -ForegroundColor Cyan
    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method Post -Body $body -ContentType "application/json"

    if ($response.data.user) {
        $user = $response.data.user
        Write-Host "User found:" -ForegroundColor Green
        Write-Host "  Username: $($user.username)" -ForegroundColor White
        Write-Host "  Password Hash: $($user.passwordHash)" -ForegroundColor White

        # Test password hashing
        $password = "admin123"
        $bytes = [System.Text.Encoding]::UTF8.GetBytes($password)
        $sha256 = [System.Security.Cryptography.SHA256]::Create()
        $hashBytes = $sha256.ComputeHash($bytes)
        $expectedHash = [Convert]::ToBase64String($hashBytes)

        Write-Host "  Expected Hash: $expectedHash" -ForegroundColor White
        Write-Host "  Hash Match: $($user.passwordHash -eq $expectedHash)" -ForegroundColor White

        if ($user.passwordHash -eq $expectedHash) {
            Write-Host "SUCCESS: Password hashing is working correctly!" -ForegroundColor Green
        } else {
            Write-Host "ERROR: Password hashes don't match!" -ForegroundColor Red
        }
    } else {
        Write-Host "No user found" -ForegroundColor Red
    }
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "Now try logging in at: https://localhost:5231" -ForegroundColor Yellow
Write-Host "Username: admin" -ForegroundColor White
Write-Host "Password: admin123" -ForegroundColor White