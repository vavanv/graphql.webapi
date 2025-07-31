# Test Password Hashing Logic
Write-Host "Testing Password Hashing Logic..." -ForegroundColor Green

$password = "admin123"
Write-Host "Password: $password" -ForegroundColor Yellow

# Hash the password using the same method as DbInitializer and AuthService
$bytes = [System.Text.Encoding]::UTF8.GetBytes($password)
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$hashBytes = $sha256.ComputeHash($bytes)
$hash = [Convert]::ToBase64String($hashBytes)

Write-Host "Generated Hash: $hash" -ForegroundColor Cyan

# Test verification
$testPassword = "admin123"
$testBytes = [System.Text.Encoding]::UTF8.GetBytes($testPassword)
$testSha256 = [System.Security.Cryptography.SHA256]::Create()
$testHashBytes = $testSha256.ComputeHash($testBytes)
$testHash = [Convert]::ToBase64String($testHashBytes)

Write-Host "Test Hash: $testHash" -ForegroundColor Cyan
Write-Host "Hashes Match: $($hash -eq $testHash)" -ForegroundColor Green

Write-Host "`nTest completed!" -ForegroundColor Green