
public class TransactionService
{
    private readonly UserRepository _userRepository;
    private readonly ItemRepository _itemRepository;

    public TransactionService (UserRepository userRepository, ItemRepository itemRepository)
    {
        _userRepository = userRepository;
        _itemRepository = itemRepository;
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
            throw new InvalidOperationException("This item is too expensive");
        }

        await _userRepository.UpdateUserAsync(user);
        await _itemRepository.UpdateItemAsync(item);
    }


}