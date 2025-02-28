
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace webapp_backend.Controllers;

[Route("user")]
[ApiController]
public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("changeView")]
    public IActionResult ChangeView (int viewId)
    {
        switch (viewId)
        {
            case 1:
                return View("Login");
            case 2:
                return View("UserProfile");
            case 3:
                // needs to call a function instead that logs the user out 
                return Logout();
            case 4:
                return View("Registration");

            default:
                return RedirectToAction("Index");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        try
        {
            await _userService.RegisterUserAsync(user);
            return Ok(new { message = "User registered succesfully" });
        } catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        try
        {
            await _userService.AuthenticateUserAsync(user);
            HttpContext.Session.SetString("LoggedInUser", JsonSerializer.Serialize(user));
            return Ok(new { message = "User logged in successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

  
    [HttpGet]
    public IActionResult GetUserFromSession()
    {
        var userJson = HttpContext.Session.GetString("LoggedInUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return Unauthorized("No user is logged in.");
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        return Ok(user);
    } 

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("LoggedInUser");
        return Ok(new { message = "Logged out successfully!" });
    }
}
