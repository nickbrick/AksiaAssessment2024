using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AksiaAssessment2024.Features.Transaction
{
    public class TransactionDao
    {
        private readonly IDbConnection _dbConnection;


        public TransactionDao(IDbConnection dbConnection)
        {
            _dbConnection= dbConnection;
        }
        public bool Exists(Guid id)
        {
            const string query = "SELECT COUNT(*) FROM Transactions WHERE Id = @Id";
            int count = _dbConnection.ExecuteScalar<int>(query, new { Id = id });
            return count > 0;
        }

        public IEnumerable<Transaction> GetAllOrderByInception(int page, int pageSize)
        {
            const string query = "SELECT * FROM Transactions ORDER BY Inception OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            return _dbConnection.Query<Transaction>(query, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
        }

        public Transaction? Get(Guid id)
        {
            const string query = "SELECT * FROM Transactions WHERE Id = @Id";
            return _dbConnection.QueryFirstOrDefault<Transaction>(query, new { Id = id });
        }

        public Transaction Insert(Transaction transaction)
        {
            const string query = @"INSERT INTO Transactions (Id, ApplicationName, Email, Filename, Url, Inception, Amount, Allocation) 
                                VALUES (@Id, @ApplicationName, @Email, @Filename, @Url, @Inception, @Amount, @Allocation);
                                SELECT * FROM Transactions WHERE Id = @Id;";
            return _dbConnection.QuerySingle<Transaction>(query, transaction);
        }

        public void Update(Transaction transaction)
        {
            const string query = @"
            UPDATE Transactions 
            SET 
                ApplicationName = @ApplicationName, 
                Email = @Email, 
                Filename = @Filename, 
                Url = @Url, 
                Inception = @Inception, 
                Amount = @Amount, 
                Allocation = @Allocation
            WHERE 
                Id = @Id;
            SELECT * FROM Transactions WHERE Id = @Id;";

            var updatedTransaction = _dbConnection.QuerySingleOrDefault<Transaction>(query, transaction);

            if (updatedTransaction == null)
                throw new Exception("Failed to update transaction.");

        }

        public IEnumerable<Transaction> Merge(IEnumerable<Transaction> transactions)
        {
            string sourceTable = Serialize(transactions);
            string query = @$"MERGE INTO Transactions AS target
                                USING ({sourceTable}) AS source
                                ON target.Id = source.Id
                                WHEN MATCHED THEN 
                                    UPDATE SET ApplicationName = source.ApplicationName,
                                               Email = source.Email,
                                               Filename = source.Filename,
                                               Url = source.Url,
                                               Inception = source.Inception,
                                               Amount = source.Amount,
                                               Allocation = source.Allocation
                                WHEN NOT MATCHED THEN
                                    INSERT (Id, ApplicationName, Email, Filename, Url, Inception, Amount, Allocation) 
                                    VALUES (source.Id, source.ApplicationName, source.Email, source.Filename, source.Url, source.Inception, source.Amount, source.Allocation);
                                ";
            return _dbConnection.Query<Transaction>(query);
        }
        private static string Serialize(IEnumerable<Transaction> transactions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (VALUES ");
            var rows = String.Join(",", transactions.Select(transaction => 
            $"('{transaction.Id}'," +
            $" '{transaction.ApplicationName}'," +
            $" '{transaction.Email}'," +
            $" '{transaction.Filename}'," +
            $" '{transaction.Url}'," +
            $" '{transaction.Inception.ToString("yyyy-MM-dd HH:mm:ss")}'," +
            $" {transaction.Amount}," +
            $" {transaction.Allocation})"));
            sb.Append(rows);
            sb.AppendLine(") AS s (Id, ApplicationName, Email, Filename, Url, Inception, Amount, Allocation)");
            return sb.ToString();
        }
        public void Delete(Guid id)
        {
            const string query = "DELETE FROM Transactions WHERE Id = @Id";
            _dbConnection.Execute(query, new { Id = id });
        }
    }
}
