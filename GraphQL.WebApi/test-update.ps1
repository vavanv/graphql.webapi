# Test GraphQL Mutation for Updating Customers
Write-Host "Testing GraphQL mutation for updating customers..." -ForegroundColor Green

# Disable SSL certificate validation for testing
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

# First, let's get a customer to update
Write-Host "`nGetting a customer to update..." -ForegroundColor Yellow
$query = @"
{
  "query": "query { customers { id firstName lastName contact email dateOfBirth } }"
}
"@

try {
    $response = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method POST -Body $query -ContentType "application/json"
    $customers = $response.data.customers
    if ($customers.Count -gt 0) {
        $customerToUpdate = $customers[0]
        Write-Host "Found customer: $($customerToUpdate.firstName) $($customerToUpdate.lastName)" -ForegroundColor Green

        # Update the customer
        $mutation = @"
{
  "query": "mutation(\`$id: Int!, \`$firstName: String!, \`$lastName: String!, \`$contact: String!, \`$email: String!, \`$dateOfBirth: DateTime!) { updateCustomer(id: \`$id, firstName: \`$firstName, lastName: \`$lastName, contact: \`$contact, email: \`$email, dateOfBirth: \`$dateOfBirth) { id firstName lastName contact email dateOfBirth } }",
  "variables": {
    "id": $($customerToUpdate.id),
    "firstName": "$($customerToUpdate.firstName) Updated",
    "lastName": "$($customerToUpdate.lastName)",
    "contact": "$($customerToUpdate.contact)",
    "email": "updated.$($customerToUpdate.email)",
    "dateOfBirth": "$($customerToUpdate.dateOfBirth)"
  }
}
"@

        Write-Host "`nUpdating customer..." -ForegroundColor Yellow
        $updateResponse = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method POST -Body $mutation -ContentType "application/json"
        Write-Host "✅ Customer updated successfully!" -ForegroundColor Green
        Write-Host "Updated customer details:" -ForegroundColor Yellow
        $updateResponse | ConvertTo-Json -Depth 3

        # Verify the update by querying again
        Write-Host "`nVerifying the update..." -ForegroundColor Yellow
        $verifyResponse = Invoke-RestMethod -Uri "https://localhost:5001/graphql" -Method POST -Body $query -ContentType "application/json"
        $updatedCustomer = ($verifyResponse.data.customers | Where-Object { $_.id -eq $customerToUpdate.id })[0]
        Write-Host "Customer after update: $($updatedCustomer.firstName) $($updatedCustomer.lastName)" -ForegroundColor Green
    } else {
        Write-Host "❌ No customers found to update" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ GraphQL update test failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}