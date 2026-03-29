using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SynthPay.Ledger.Worker.Infrastructure.Persistence;

namespace SynthPay.Ledger.Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly LedgerDbContext _dbContext;

        public BalancesController(LedgerDbContext dbContext) => _dbContext = dbContext;

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetBalance(Guid accountId)
        {
            var balance = await _dbContext.Balances
                .FirstOrDefaultAsync(b => b.AccountId == accountId);

            if (balance == null)
            {
                return NotFound(new { Message = "Nenhum saldo encontrado para esta conta." });
            }

            return Ok(new
            {
                AccountId = balance.AccountId,
                Balance = balance.CurrentBalance,
                LastUpdatedAt = balance.LastUpdatedAt
            });
        }
    }
}
