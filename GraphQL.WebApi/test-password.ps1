# Test Password Hashing
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

Write-Host "Testing Password Hashing..." -ForegroundColor Green

# Test 1: Get admin user from database
Write-Host "`n1. Getting admin user from database..." -ForegroundColor Yellow
$query1 = @{
    query = "query { user(username: `"admin`") { id username email passwordHash firstName lastName isActive } }"
} | ConvertTo-Json

try {
    $response1 = Invoke-WebRequest -Uri "https://localhost:5003/graphql" -Method POST -Headers @{"Content-Type"="application/json"} -Body $query1 -UseBasicParsing
    Write-Host "Response:" -ForegroundColor Cyan
    Write-Host $response1.Content

    # Parse the response to get the password hash
    $data = $response1.Content | ConvertFrom-Json
    if ($data.data.user) {
        $storedHash = $data.data.user.passwordHash
        Write-Host "`nStored password hash: $storedHash" -ForegroundColor Yellow
    }
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Hash the password locally and compare
Write-Host "`n2. Testing local password hashing..." -ForegroundColor Yellow
$password = "admin123"
$bytes = [System.Text.Encoding]::UTF8.GetBytes($password)
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$hashBytes = $sha256.ComputeHash($bytes)
$localHash = [Convert]::ToBase64String($hashBytes)
Write-Host "Local password hash: $localHash" -ForegroundColor Yellow

Write-Host "`nTest completed!" -ForegroundColor Green