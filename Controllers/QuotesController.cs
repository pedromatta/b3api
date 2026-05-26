using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public QuotesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuotes()
    {
        var quotes = await _dbContext.Quotes
            .Select(q => new
            {
                q.Ticker,
                q.Price,
                q.UpdatedAt
            })
            .ToListAsync();

        return Ok(quotes);
    }
}
