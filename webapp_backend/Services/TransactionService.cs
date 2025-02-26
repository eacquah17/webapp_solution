
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

public class TransactionService
{
    private readonly UserRepository _userRepository;
    private readonly ItemRepository _itemRepository;

    public TransactionService (UserRepository userRepository, ItemRepository itemRepository)
    {
        _userRepository = userRepository;
        _itemRepository = itemRepository;
    }

    
    public async Task<List<Item>> LoadItemsAsync()
    {
        return await _itemRepository.GetAllItemsAsync();
    }  

    public async Task<Item> LoadItemViewAsync(int id)
    {
        
        return await _itemRepository.GetItemByIdAsync(id);

    }

    public async Task BuyItem(Item item, User user)
    {
        if (item.Price <= user.Balance)
        {
            user.Balance -= item.Price;
            item.Stock -= 1; 
        }
        else
        {
            throw new InvalidOperationException("This item is too expensive!!");
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
    }


}