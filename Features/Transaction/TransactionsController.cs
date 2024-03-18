using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AksiaAssessment2024.Helpers;
namespace AksiaAssessment2024.Features.Transaction
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionDao _transactionDao;
        private readonly int _maxPageSize;


        public TransactionsController(TransactionDao transactionDAO, IConfiguration configuration)
        {
            _transactionDao = transactionDAO;
            if (configuration.GetSection("MaxPageSize").Value != null)
                _maxPageSize = configuration.GetValue<int>("MaxPageSize");
            else
                _maxPageSize = 10;
        }

        // b. Fetch all transactions paginated, using the transaction’s date as sorting field.
        // User should be able to select page and/or number of transactions per page
        [HttpGet]
        public ActionResult<PaginatedResponse<Transaction>> Get(int page = 1, int pageSize = 2)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, _maxPageSize);
            var transactions = _transactionDao.GetAllOrderByInception(page, pageSize);
            return Ok(new PaginatedResponse<Transaction>(transactions, this.Request, nameof(page), page));
        }

        // e. Fetch a transaction
        [HttpGet("{id}")]
        public ActionResult<Transaction> Get(Guid id)
        {
            var transaction = _transactionDao.Get(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        // d. Upsert a transaction
        [HttpPost]
        public ActionResult<Transaction> Upsert(Transaction transaction)
        {
            if (transaction.Id == null)
            {
                transaction.Id = Guid.NewGuid();
                return Insert(transaction);
            }
            else
                return Update(transaction);
        }
        private ActionResult<Transaction> Insert(Transaction transaction)
        {
            var createdTransaction = _transactionDao.Insert(transaction);
            return CreatedAtAction(nameof(Upsert), new { id = createdTransaction.Id }, createdTransaction);
        }

        private ActionResult Update(Transaction transaction)
        {
            if (!_transactionDao.Exists(transaction.Id.Value))
                return NotFound();

            _transactionDao.Update(transaction);
            return NoContent();
        }
        // c.Delete a transaction
        [HttpDelete("{id}")]
        public ActionResult DeleteTransaction(Guid id)
        {
            
            if (!_transactionDao.Exists(id))
                return NotFound();

            _transactionDao.Delete(id);
            return NoContent();
        }
    }
}
