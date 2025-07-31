# Test GraphQL API Connection
Write-Host "Testing GraphQL API connection..." -ForegroundColor Green

# Disable SSL certificate validation for testing
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

$query = @"
{
  "query": "query { customers { id firstName lastName contact email dateOfBirth } }"
}
"@

try {
    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method POST -Body $query -ContentType "application/json"
    Write-Host "✅ GraphQL API is working!" -ForegroundColor Green
    Write-Host "Response received successfully" -ForegroundColor Yellow
    $response | ConvertTo-Json -Depth 3
} catch {
    Write-Host "❌ GraphQL API test failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}

Write-Host "`nTesting MVC application..." -ForegroundColor Green
try {
    $mvcResponse = Invoke-WebRequest -Uri "https://localhost:5231"
    Write-Host "✅ MVC application is accessible!" -ForegroundColor Green
    Write-Host "Status: $($mvcResponse.StatusCode)" -ForegroundColor Yellow
} catch {
    Write-Host "❌ MVC application test failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}