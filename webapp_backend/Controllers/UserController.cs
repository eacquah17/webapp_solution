
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
    public IActionResult ChangeView(int viewId)
    {
        var user = _userService.GetUserFromSession();

        switch (viewId)
        {
            case 1:
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Login");
                }
            case 2:
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("ProfileView", user);
                }
            case 3:
                // needs to call a function instead that logs the user out 
                Logout();
                return RedirectToAction("Index", "Home");
            case 4:
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Registration");
                }

            default:
                return RedirectToAction("Index", "Home");
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
            var authenticatedUser = await _userService.AuthenticateUserAsync(user);
            HttpContext.Session.SetString("LoggedInUser", JsonSerializer.Serialize(authenticatedUser));
            return Ok(new { message = "User logged in successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
  
    

    
    /*
    private void DeleteUserAccount()
    {
        var user = _userService.GetUserFromSession();
        
    } */
    private void Logout()
    {
        HttpContext.Session.Remove("LoggedInUser");
        HttpContext.Session.Clear();
    }
}
