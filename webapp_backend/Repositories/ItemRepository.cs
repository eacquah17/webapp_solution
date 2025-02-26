using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class ItemRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Adds an Item
    public async Task CreateItemAsync(Item item)
    {
        if (string.IsNullOrWhiteSpace(item.ImageUrl))
        {
            item.ImageUrl = "https://pixabay.com/vectors/red-circle-backslash-no-symbol-24018/"; 
        }

        if (string.IsNullOrWhiteSpace(item.Description))
        {
            item.Description = "No Description";
        }

        if (item.Stock <= 50)
        {
            item.Status = "Low Stock";
        }
        else if (item.Stock == 0)
        {
            item.Status = "Out of Stock";
        }
        else
        {
            item.Status = "Active";
        }

        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        Console.WriteLine("Item Added.");
    }

    //Get all items
    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _context.Items.ToListAsync();
    }

    //Get Item by Id
    public async Task<Item> GetItemByIdAsync(int id)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
    }

    //Update Item
    public async Task UpdateItemAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }

    //Delete Item by Id
    public async Task DeleteItemAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item); 
            await _context.SaveChangesAsync();
        }
    }
}