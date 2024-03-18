using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using AksiaAssessment2024.Helpers;
using AksiaAssessment2024.Features.Transaction;

namespace AksiaAssessment2024.Features.Sync
{
    [Route("[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly TransactionDao _transactionDao;
        private readonly CsvParser _csvParser;

        public SyncController(TransactionDao transactionDAO, CsvParser csvParserService)
        {
            _transactionDao = transactionDAO;
            _csvParser = csvParserService;
        }

        [HttpPost("[action]")]
        public IActionResult Transactions(IFormFile csv)
        {
            if (csv == null || csv.Length == 0)
                return BadRequest("CSV file is missing or empty.");
            try
            {
                var transactions = _csvParser.Parse<Transaction.Transaction>(csv.OpenReadStream());
                _transactionDao.Merge(transactions);

                return Ok("Transactions synchronized successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error syncing transactions: {ex.Message}");
            }
        }
    }
}
