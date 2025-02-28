
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using System.Text.RegularExpressions;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public User GetUserFromSession()
    {
        var userJson = _httpContextAccessor.HttpContext.Session.GetString("LoggedInUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return null; // No user in session
        }
         
        // Deserialize the JSON to a User object
        return JsonSerializer.Deserialize<User>(userJson);
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

    public async Task<User> AuthenticateUserAsync(User user)
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

        return existingUsername; 
    }
/*
    public async Task DeleteUserAsync(User user)
    {

    }*/

    //Updates User Information NOT FINISHED 
    public async Task ChangeUserInfoAsync(User user)
    {
        /*
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentException("a username is required");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("an email is required");
        } */

        await _userRepository.UpdateUserAsync(user);
    }



}