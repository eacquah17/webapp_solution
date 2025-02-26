
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //Creates New User 
    public async Task RegisterUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User data cannot be null");

        var existingEmail = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingEmail != null)
        {
            throw new Exception("This email address is already in use.");
        }

        var existingUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
        if (existingUsername != null)
        {
            throw new Exception("This username is already taken.");
        }



        await _userRepository.CreateUserAsync(user);
    }

    public async Task AuthenticateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User data cannot be null");

        var existingUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
        if (existingUsername == null)
        {
            throw new UnauthorizedAccessException("This username does not exist.");
        }
        else
        {
            if (existingUsername.Password != user.Password)
            {
                throw new UnauthorizedAccessException("This Password is Incorrect.");
            }
        }
    }

    //Updates User Information NOT FINISHED 
    public async Task ChangeUserInfoAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentException("a username is required");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("an email is required");
        }
        await _userRepository.UpdateUserAsync(user);
    }

    /*Logging in User
    public async Task LoginUser(User user, string username, string enteredPassword)
    {
        if (username == user.Username)
        if (password == user.Password)
    }
    */

}