
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text.Json;

public class TransactionService
{
    private readonly UserRepository _userRepository;
    private readonly ItemRepository _itemRepository;
    private readonly UserService _userService;

    public TransactionService (UserRepository userRepository, ItemRepository itemRepository, UserService userService)
    {
        _userRepository = userRepository;
        _itemRepository = itemRepository;
        _userService = userService;
    }

    
    public async Task<List<Item>> LoadItemsAsync()
    {
        return await _itemRepository.GetAllItemsAsync();
    }  

    public async Task<Item> LoadItemViewAsync(int id)
    {
        
        return await _itemRepository.GetItemByIdAsync(id);

    }

    public async Task<User> ProcessTransaction(Item item)
    {
        var user = _userService.GetUserFromSession();

        if (user == null)
        {
            throw new UnauthorizedAccessException("User is not logged in.");
        }

        if (item.Price <= user.Balance)
        {
            user.Balance -= item.Price;
            item.Stock -= 1;


        }
        else
        {
            throw new InvalidOperationException("Insufficient balance.");
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

        await _userRepository.UpdateUserAsync(user);
        await _itemRepository.UpdateItemAsync(item);
        return user; 
    }


}