# AksiaAssessment2024
## Overview

Create a restful API using .NET that will parse a CSV file with transaction data from various applications. The API should be able to:

- Read a CSV file ✅ (Examples provided), validate the data ❌ and persist the information ✅.
  - File can include either new transactions or update existing transactions.✅
  - Feel free to choose any (not in-memory) database you see fit. ℹ Dapper over SQL Server
- Fetch all transactions paginated✅, using the transaction’s date as sorting field✅. User should be able to select page✅ and/or number of transactions per page✅.
  - ⭐ Page links
  - ⭐ Configurable max page size
- Delete a transaction.✅
- Upsert a transaction.✅
- Fetch a transaction.✅

## Data Specification

| Name             | Details                                                                                               
|------------------|-------------------------------------------------------------------------------------------------------
| Id               | Mandatory, a unique identifier for the transaction. GUID, assigned by the API upon saving the transaction. 
| ApplicationName  | Mandatory, the name of the application that was used to record the transaction. Cannot exceed 200 characters. 
| Email            | Mandatory, the user’s email. Cannot exceed 200 characters.                                           
| Filename         | Optional, the name of a file attached to the transaction. Cannot exceed 300 characters. Valid extensions: png, mp3, tiff, xls, pdf. 
| Url              | Optional, the URL of the external application. When provided, should be a valid URL.                  
| Inception        | Mandatory, the transaction’s date. Should be in the past.                                            
| Amount           | Mandatory, amount with currency. An existing transaction cannot change currency.                     
| Allocation       | Optional, a positive decimal between 0-100.                                                          

## CSV

CSV file will include headers.

## Requirements

- Application should be structured into logical layers. ℹ Feature-based structure rather than MVC. Separation of concerns dependency injection all the good stuff
- Final code should be production ready.❌ Not even ready
- Errors should be handled gracefully.❌ No time for that
