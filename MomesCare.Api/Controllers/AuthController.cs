using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MomesCare.Api.ApiClient;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Helpers.Enums;
using MomesCare.Api.Helpers;
using System.Reflection;
using MomesCare.Api.Entities.Models;
using User = MomesCare.Api.Entities.Models.ApplicationUser;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using MomesCare.Api.Exceptions;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using MomesCare.Api.ApiClient.Entitis;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Net.Http.Headers;
using UserInfo = MomesCare.Api.Entities.ViewModel.UserInfo;
using MomesCare.Api.Entities.Models;
using FirebaseAuthException = Firebase.Auth.FirebaseAuthException;
using Newtonsoft.Json.Linq;
using System.Linq;
using MomesCare.Api.Repository;
using MomesCare.Api.Services.Static;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;

namespace MomesCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {


        //private readonly ILogger<AuthController> _logger;
        private readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly FirebaseAuth firebaseAuth;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly UserServices userServices;

        public AuthController(
                                FirebaseAuthClient firebaseAuthClient, 
                              
                                IConfiguration config,
                                UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IUserClaimsHelper userClaimsHelper,
                                UserServices userServices)
        {
           
            _firebaseAuthClient = firebaseAuthClient;
            this.firebaseAuth = FirebaseAuth.DefaultInstance;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _userClaimsHelper = userClaimsHelper;
            this.userServices = userServices;
        }

       
   

   

        class UserInfoFire {
            public string Id { get; set; }

            public string Email { get; set; }
        }

    

        private string GenerateJwtToken(UserInfoFire userInfo)
        {
            return null;
        }





        private async Task<string?> GenerateToken(ApplicationUser user)
        {

            if (user == null)
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Email, user?.Email??""),
                new Claim(JwtClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault()??""),

            };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        private async void setSessionInfo(User user)
        {
            // »⁄œ  ”ÃÌ· «·œŒÊ· »‰Ã«Õ
            var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Id, user.Id ?? ""),
            new Claim(JwtClaimTypes.Email,user.Email ?? ""),
            new Claim(JwtClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? ""),

        };


            var userIdentity = new ClaimsIdentity(claims, "Bearer");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            // ﬁ„ » ”ÃÌ· «·„” Œœ„ ›Ì «·Ã·”…
            await HttpContext.SignInAsync(principal);
        }

        [AllowAnonymous]
        [HttpGet("getCurrentTime")]
        public  IActionResult getCurrentTime()
        {
            return Ok(Helper.GetCurrentTime().TimeOfDay.ToString());
        }


        [AllowAnonymous]
        [HttpPost("setTime")]
        public IActionResult setTime(TimeSpan time)
        {
            return Ok(time);
        }

        [AllowAnonymous]
        [HttpGet("CompareWithCurrentTime")]
        public IActionResult CompareWithCurrentTime(string time)
        {
          

            var currentTime = Helper.GetCurrentTime().TimeOfDay;

            TimeSpan inputTime = Helper.ConvertToTimeSpan(time);
                
            var result = TimeSpan.Compare(currentTime, inputTime);
            var res = "";
            if (result == 1)
                res = "currentTime is greater than inputTime";

            else if (result == 0)
                res = "currentTime is equal to inputTime";

            else
                res = "inputTime is greater than currentTime";
            return Ok(new { result, res, inputTime = inputTime });
            
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUp model)
        {

            var baseResponse = new BaseResponse();

            try
            {

                if (model.Role == null || Helper.ConvertFirstLetterToCapital(model.Role) == UserRoles.Admin.ToString())
                    ModelState.AddModelError(string.Empty, "invalid  user Role   !!");

                else if (model != null && ModelState.IsValid && await _userManager.FindByEmailAsync(model.Email) == null)
                {
                    var user = new ApplicationUser
                    {
                        Id = model.uId,
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };
                    var identityResult = await _userManager.CreateAsync(user, model.Password);

                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _userManager.AddToRoleAsync(user, Helper.ConvertFirstLetterToCapital(model.Role));

                    }
                    
                }
                else
                    ModelState.AddModelError(string.Empty, "invalid  user !!");


            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }


            if (ModelState.ErrorCount > 0)
            {
                baseResponse.ErrorsMessage = Helper.GetModelErrors(ModelState).ToList();
                ModelState.Clear();
                return BadRequest(baseResponse);
            }
            else
                return Ok(baseResponse);

        }




        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ActionResult<BaseResponse>> SignIn(SignIn model)
        {
            var baseResponse = new BaseResponse();

            try
            {
                if (model != null && ModelState.IsValid)
                {

                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {

                        if (await _userManager.CheckPasswordAsync(user, model.Password.Trim()))
                        {
                            // Password is correct, sign in the user
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            // »⁄œ  ”ÃÌ· «·œŒÊ· »‰Ã«Õ
                            var claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Id , user.Id ?? ""),
                                new Claim(JwtClaimTypes.Email,user.Email??""),
                                new Claim(JwtClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault()??""),

                            };


                            var userIdentity = new ClaimsIdentity(claims, "Bearer");
                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            // ﬁ„ » ”ÃÌ· «·„” Œœ„ ›Ì «·Ã·”…
                            await HttpContext.SignInAsync(principal);

                            var token = await GenerateToken(user);

                            var _role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                            if (_role == null)
                                ModelState.AddModelError(string.Empty, "login is Invalid  !!");
                            else
                            {
                                baseResponse.Result = new UserInfo { Name = user.FullName, Email = user.Email, Role = _role, Token = "bearer " + token };

                                if (!model.FCMToken.IsNullOrEmpty())
                                {

                                    await userServices.initializeFCMToken(new FCMToken { Token = model.FCMToken,userId=user.Id });
                                }
                                  
                            }

                        }
                        else
                            ModelState.AddModelError(string.Empty, "Invalid password  !!");
                     

                    }
                    else
                        ModelState.AddModelError(string.Empty, "Invalid userName and  password  !!");

                }
                else
                    ModelState.AddModelError(string.Empty, "Invalid Email or password !!");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            if (ModelState.ErrorCount > 0)
            {
                baseResponse.ErrorsMessage = Helper.GetModelErrors(ModelState).ToList();
                return BadRequest(baseResponse);
            }
            else
                return Ok(baseResponse);
        }

        [Authorize]
        [HttpPost("initializeFCMToken")]
        public async Task<IActionResult> initializeFCMToken(FCMToken model)
        {

            var baseResponse = new BaseResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    await userServices.initializeFCMToken(model);
                    baseResponse.Result = "Successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "not valid !!");
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }


            if (ModelState.ErrorCount > 0)
            {
                baseResponse.ErrorsMessage = Helper.GetModelErrors(ModelState).ToList();
                ModelState.Clear();
                return BadRequest(baseResponse);
            }
            else
                return Ok(baseResponse);

        }


        [Authorize]
        [HttpPut("ResetEmailPassword")]
        public async Task<IActionResult> ResetEmailPassword(string email)
        {
            await  _firebaseAuthClient.ResetEmailPasswordAsync(email);

            return Ok(new BaseResponse { Result= "A verification link has been sent via email. Please click on the link to complete the verification !!" });
        }


        [Authorize]
        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            
            try
            {
                //_firebaseAuthClient.SignOut();
                await HttpContext.SignOutAsync();
                await _signInManager.SignOutAsync();
                return Ok(new BaseResponse { Result = " logout is successful " });

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }
         
        }



        //private async Task<string?> GenerateTokenAsync(ApplicationUser user,string firebaseToken)
        //{

        //    if (user == null)
        //        return null;


        //    var handler = new JwtSecurityTokenHandler();

        //    // «· Õﬁﬁ „‰ √‰ «·—„“ » ‰”Ìﬁ JWT «·’ÕÌÕ
        //    if (!handler.CanReadToken(firebaseToken))
        //    {
        //        throw  new Exception("Invalid token format.");
        //    }

        //    var jsonToken = handler.ReadToken(firebaseToken) as JwtSecurityToken;
        //    var email = jsonToken.Claims.First(claim => claim.Type == "email").Value;
        //    var iat = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Iat).Value;
        //    var sub = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
        //    var exp = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value;
        //    var iss = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Iss).Value;
        //    var aud = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Aud).Value;
        //    //var jti = jsonToken.Payload.FirstOrDefault(claim => claim. == JwtRegisteredClaimNames.Aud).Value;

        //    //var authTime = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Au);


        //    //var Jti = jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub,sub),//_config["Jwt:Subject"]),
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat,iat),//DateTime.UtcNow.ToString()),
        //        new Claim(IdentityModel.JwtClaimTypes.Id, user.Id),
        //        new Claim(IdentityModel.JwtClaimTypes.Email, user?.Email??""),
        //        new Claim(IdentityModel.JwtClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault()??""),

        //    };

        //    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
        //    //var token = new JwtSecurityToken(
        //    //    _config["Jwt:Issuer"],
        //    //    _config["Jwt:Audience"], 
        //    //    claims,
        //    //    expires: DateTime.UtcNow.AddDays(1),
        //    //    signingCredentials: credentials
        //    //  );     

        //    var token = new JwtSecurityToken(
        //        iss,
        //        aud, 
        //        claims,
        //        expires: DateTime.UtcNow.AddDays(1),
        //        signingCredentials: credentials
        //      );


        //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //    return tokenString;

        //}

        //private async Task<ModelStateDictionary> RegisterAsync(SignUp model,string uId)
        //{

        //    if (model.Role == null || Helper.ConvertFirstLetterToCapital(model.Role) == UserRoles.Admin.ToString())
        //        ModelState.AddModelError(string.Empty, "invalid user role !!");

        //    else if (ModelState.IsValid && await _userManager.FindByEmailAsync(model.Email) == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            //Id = Guid.NewGuid().ToString()
        //            Id = uId,
        //            UserName = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            Email = model.Email
        //        };
        //        var identityResult = await _userManager.CreateAsync(user, model.Password);

        //        if (identityResult.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            await _userManager.AddToRoleAsync(user, Helper.ConvertFirstLetterToCapital(model.Role));

        //        }

        //        foreach (var error in identityResult.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }

        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid Email !!");
        //    }

        //    return  ModelState ;

        //}

        //private async Task<(string,User)> LoginAsync(SignIn model)
        //{


        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user != null){

        //            if (await _userManager.CheckPasswordAsync(user, model.Password))
        //            {
        //                // Password is correct, sign in the user
        //                await _signInManager.SignInAsync(user, isPersistent: false);


        //                // »⁄œ  ”ÃÌ· «·œŒÊ· »‰Ã«Õ
        //                var claims = new List<Claim>
        //                {
        //                    new Claim(JwtClaimTypes.Id , user.Id ?? ""),
        //                    new Claim(JwtClaimTypes.Email,user.Email??""),
        //                    new Claim(JwtClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault()??""),

        //                };

        //                var userIdentity = new ClaimsIdentity(claims, "Bearer");
        //                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
        //                // ﬁ„ » ”ÃÌ· «·„” Œœ„ ›Ì «·Ã·”…
        //                await HttpContext.SignInAsync(principal);


        //            var _role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        //            if (_role == null)
        //                ModelState.AddModelError("Role", "Invalid Authorize !!  ");

        //            return (_role,user);

        //        }
        //            else
        //                 ModelState.AddModelError("Password","Invalid password !!  ");

        //        }
        //        else
        //            ModelState.AddModelError("Email", "Invalid Email !!  ");

        //    return (null,user);

        //}
        //private async Task<IActionResult> myLogin(SignIn model)
        //{
        //    // «” ·«„ —„“ «· Õﬁﬁ „‰  ÿ»Ìﬁ Flutter
        //    string idToken = model.FCMToken ;

        //    // «· Õﬁﬁ „‰ ’Õ… —„“ «· Õﬁﬁ

        //    try
        //    {
        //        var decodedToken = await firebaseAuth.VerifyIdTokenAsync(idToken);
        //        var uid = decodedToken.Uid;


        //        UserRecord userRecord = await firebaseAuth.GetUserAsync(uid);
        //        string email = userRecord.Email;

        //        // «” Œ—«Ã „⁄·Ê„«  «·„” Œœ„ „‰ decodedToken
        //        var userInfo = new UserInfoFire
        //        {
        //            Id = uid,
        //            Email = email,
        //        };


        //        // ≈‰‘«¡ —„“ JWT
        //        var token = GenerateJwtToken(userInfo);

        //        return Ok(new { token = token });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        //If ID token argument is null or empty
        //        return BadRequest(ex.Message);

        //    } catch (FirebaseAuthException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[AllowAnonymous]
        //[HttpPost("SignUp")]
        //public async Task<IActionResult> SignUp(SignUp model)
        //{


        //    var baseResponse = new BaseResponse();

        //    try
        //    {
        //        if (model != null && ModelState.IsValid)
        //        {


        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            if (user != null)
        //                ModelState.AddModelError(string.Empty, "Invalid Email !!");
        //            else
        //            {

        //                var response = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(model.Email, model.Password, model.UserName);

        //                //("Admin@gmail.com", "Test@123", "Adminstor");

        //                if (response != null && response.User != null && response.User.Credential != null && response.User.Uid != null)
        //                {

        //                    await RegisterAsync(model, response.User.Info.Uid);

        //                    if (ModelState.ErrorCount == 0)
        //                    {

        //                        //await LoginAsync(signIn);

        //                        baseResponse.Result = "SignUp is successfully !!";

        //                        //new UserView
        //                        //    {
        //                        //        UId = response.User.Info.Uid,
        //                        //        DisplayName = response.User.Info.DisplayName,
        //                        //        Email = response.User.Info.Email,
        //                        //        Token = "bearer " + response.User.Credential.IdToken, 
        //                        //        Role = role,
        //                        //    };



        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, "User creation failed.");
        //                }
        //            }
        //        }
        //        else
        //            ModelState.AddModelError(string.Empty, "User creation failed.");


        //    }
        //    catch (Firebase.Auth.FirebaseAuthException ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //    }


        //    if (ModelState.ErrorCount > 0)
        //    {
        //        baseResponse.ErrorsMessage = GetModelErrors().ToList();
        //        ModelState.Clear();
        //        return BadRequest(baseResponse);
        //    }
        //    else
        //        return Ok(baseResponse);


        //}

        //[AllowAnonymous]
        //[HttpPost("Login")]
        //public async Task<ActionResult<BaseResponse>> Login(SignIn model)
        //{

        //    var baseResponse = new BaseResponse();


        //    if (ModelState.IsValid)
        //    {



        //        //var firebaseResponse = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
        //        //if (firebaseResponse != null && firebaseResponse.User.Credential.Created != null)
        //        //{
        //        //    var (userRole, user) = await LoginAsync(model);

        //        //    if (ModelState.ErrorCount == 0)
        //        //    {
        //        //        string idToken = firebaseResponse.User.Credential.IdToken;
        //        //        var tokenString = await GenerateTokenAsync(user, idToken);
        //        //        //  Œ“Ì‰ «·—„“ ›Ì «·Ã·”…
        //        //        HttpContext.Session.SetString("JWToken", tokenString);

        //        //        baseResponse.Result = new UserView
        //        //        {
        //        //            UId = firebaseResponse.User.Info.Uid,
        //        //            DisplayName = firebaseResponse.User.Info.DisplayName,
        //        //            Email = firebaseResponse.User.Info.Email,
        //        //            Role = userRole,
        //        //            Token = "bearer " + idToken
        //        //        };


        //        //        //DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

        //        //    }
        //        //}
        //    }

        //    // Invalid login attempt
        //    return BadRequest("Invalid username or password !!");



        //}

    }
}