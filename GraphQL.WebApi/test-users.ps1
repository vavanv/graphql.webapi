# Test User GraphQL Queries
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

Write-Host "Testing User GraphQL Queries..." -ForegroundColor Green

# Test 1: Get all users
Write-Host "`n1. Testing 'users' query..." -ForegroundColor Yellow
$query1 = @{
    query = "query { users { id username email firstName lastName isActive } }"
} | ConvertTo-Json

try {
    $response1 = Invoke-WebRequest -Uri "https://localhost:5001/graphql" -Method POST -Headers @{"Content-Type"="application/json"} -Body $query1 -UseBasicParsing
    Write-Host "Response:" -ForegroundColor Cyan
    Write-Host $response1.Content
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Get specific user
Write-Host "`n2. Testing 'user' query..." -ForegroundColor Yellow
$query2 = @{
    query = "query { user(username: `"admin`") { id username email firstName lastName isActive } }"
} | ConvertTo-Json

try {
    $response2 = Invoke-WebRequest -Uri "https://localhost:5001/graphql" -Method POST -Headers @{"Content-Type"="application/json"} -Body $query2 -UseBasicParsing
    Write-Host "Response:" -ForegroundColor Cyan
    Write-Host $response2.Content
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nTest completed!" -ForegroundColor Green