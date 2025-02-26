using Microsoft.AspNetCore.Mvc;

namespace webapp_backend.Controllers;

[Route("item")]
[ApiController]
public class TransactionController : Controller
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    
    [HttpGet("loadItems")]
    public async Task<IActionResult> Load()
    {
        var items = await _transactionService.LoadItemsAsync();

        return Ok(items);
    }

    [HttpGet("ItemView/{id}")]
    public async Task<IActionResult> ItemView(int id)
    {

        var item = await _transactionService.LoadItemViewAsync(id);
        if (item == null)
        {
            return View("Error");
        }

        return View(item);
    }

    /*
    [HttpPost("buyItem")]
    public async Task<IActionResult> BuyItem(User user, Item item)
    {
        await _transactionService.BuyItem(User )
    } */
}