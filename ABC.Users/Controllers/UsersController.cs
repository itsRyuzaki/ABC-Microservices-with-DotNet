using ABC.Users.DTO.Request;
using ABC.Users.DTO.Response;
using ABC.Users.Enums;
using ABC.Users.Facade;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Users.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class UsersController(IUserFacade _userFacade) : ControllerBase
{

    [HttpGet("Health", Name = "GetHealth")]
    public IActionResult GetHealthStatus()
    {
        return Ok("Users microservice up and running!!");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpUser(UserSignUpDto userData)
    {
        var response = await _userFacade.SignUpUserAsync(userData);

        if (response.ErrorDetails?.Code == (int)ResponseCode.DUPLICATE)
        {

            return Conflict(response);
        }
        else if (!response.Success)
        {
            return StatusCode((int)ResponseCode.ERROR, response);
        }
        else
        {
            bool sessionAdded = await AddSessionCookie(userData.UserName);
            if (sessionAdded)
                return Ok(response);
            else
            {
                return StatusCode(
                    (int)ResponseCode.ERROR,
                    ApiResponseDto.HandleErrorResponse(
                        (int)ResponseCode.ERROR,
                        ["Error while setting session"]
                    )
                );
            }

        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLoginDto loginRequest)
    {
        var response = await _userFacade.LoginUserAsync(loginRequest);

        if (response.Success)
        {
            bool sessionAdded = await AddSessionCookie(loginRequest.UserName);
            if (sessionAdded)
                return Ok(response);
            else
            {
                return StatusCode(
                    (int)ResponseCode.ERROR,
                    ApiResponseDto.HandleErrorResponse(
                        (int)ResponseCode.ERROR,
                        ["Error while setting session"]
                    )
                );
            }
        }


        return StatusCode(response.ErrorDetails?.Code ?? (int)ResponseCode.UNAUTHORIZED, response);
    }

    [HttpGet("validate")]
    public async Task<IActionResult> ValidateCredentials()
    {
        string? sessionToken = Request.Cookies["session_abc"];
        if (!string.IsNullOrEmpty(sessionToken))
        {
            var result = await _userFacade.GetUserDetailsFromSessionAsync(sessionToken);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        return Unauthorized(
                    ApiResponseDto.HandleErrorResponse(
                            (int)ResponseCode.UNAUTHORIZED,
                            ["Session Token is empty"]
                        )
                    );
    }
    private async Task<bool> AddSessionCookie(string userName)
    {
        string? token = await _userFacade.CreateSessionHistoryAsync(userName);
        if (token == null)
        {
            return false;
        }

        Response.Cookies.Append("session_abc", token, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(30), // Expire in 30 mins
            IsEssential = true, // Makes the cookie essential for the app
            HttpOnly = true, // Makes the cookie accessible only via HTTP requests (not JavaScript)
            Secure = true, // Ensures the cookie is only sent over HTTPS
            SameSite = SameSiteMode.Strict // Restrict cookie sending to same-site requests
        });
        return true;
    }

}
