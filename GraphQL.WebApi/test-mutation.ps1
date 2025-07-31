# Test GraphQL Mutation for Adding Customers
Write-Host "Testing GraphQL mutation for adding customers..." -ForegroundColor Green

# Disable SSL certificate validation for testing
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

$mutation = @"
{
  "query": "mutation(\$firstName: String!, \$lastName: String!, \$contact: String!, \$email: String!, \$dateOfBirth: DateTime!) { addCustomer(firstName: \$firstName, lastName: \$lastName, contact: \$contact, email: \$email, dateOfBirth: \$dateOfBirth) { id firstName lastName contact email dateOfBirth } }",
  "variables": {
    "firstName": "Test",
    "lastName": "Customer",
    "contact": "+1-555-9999",
    "email": "test.customer@email.com",
    "dateOfBirth": "1990-01-01T00:00:00.000Z"
  }
}
"@

try {
    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method POST -Body $mutation -ContentType "application/json"
    Write-Host "✅ GraphQL mutation is working!" -ForegroundColor Green
    Write-Host "New customer created successfully:" -ForegroundColor Yellow
    $response | ConvertTo-Json -Depth 3
} catch {
    Write-Host "❌ GraphQL mutation test failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}

Write-Host "`nTesting query to verify the new customer was added..." -ForegroundColor Green

$query = @"
{
  "query": "query { customers { id firstName lastName contact email dateOfBirth } }"
}
"@

try {
    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method POST -Body $query -ContentType "application/json"
    Write-Host "✅ Query test successful!" -ForegroundColor Green
    Write-Host "Total customers: $($response.data.customers.Count)" -ForegroundColor Yellow
} catch {
    Write-Host "❌ Query test failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}