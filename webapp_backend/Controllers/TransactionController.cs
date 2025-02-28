using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace webapp_backend.Controllers;

[Route("item")]
[ApiController]
public class TransactionController : Controller
{
    private readonly TransactionService _transactionService;
    private readonly UserService _userService;

    public TransactionController(TransactionService transactionService, UserService userService)
    {
        _transactionService = transactionService;
        _userService = userService;
    }

    
    [HttpGet("loadItems")]
    public async Task<IActionResult> Load()
    {
        var items = await _transactionService.LoadItemsAsync();

        return Ok(items);
    }

    [HttpGet("itemView/{id}")]
    public async Task<IActionResult> ItemView(int id)
    {

        var item = await _transactionService.LoadItemViewAsync(id);
        if (item == null)
        {
            return View("Error");
        }

        return View(item);
    }

    
    [HttpPost("itemTransaction")]
    public async Task<IActionResult> ItemTransaction([FromBody] Item item)
    {

        try
        {
            var updatedUser = await _transactionService.ProcessTransaction(item);
            HttpContext.Session.SetString("LoggedInUser", JsonSerializer.Serialize(updatedUser));
            return Ok(new {message = "Item bought successfully."});
        } catch (Exception ex)
        {
            return BadRequest(new {message = ex.Message });
        }
    } 
}