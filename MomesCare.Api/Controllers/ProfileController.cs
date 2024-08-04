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
using MomesCare.Api.Services;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Entities.ViewModel.Profile;
using MomesCare.Api.Entities.ViewModel.Doctor;

namespace MomesCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileServices service;

        public ProfileController(ProfileServices _service)
        {
            this.service = _service;
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse>> create(ProfileCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this.service.createAsync(model);
                    return Ok(new BaseResponse { Result = "Successfully" });
                }

                var baseResponse = new BaseResponse();
                if (ModelState.ErrorCount > 0)
                {
                    baseResponse.ErrorsMessage = Helper.GetModelErrors(ModelState).ToList();
                    ModelState.Clear();
                }
                return BadRequest(baseResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<BaseResponse>> update(ProfileUpdate model)
        {
            try
            {
                await this.service.updateAsync(model);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<BaseResponse>> delete()
        {
            try
            {
                await this.service.deleteAsync();
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }    
        
        
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getInfo")]
        public async Task<ActionResult<BaseResponse>> getInfo()
        {
            try
            {
               var response = await this.service.getInfoAsync();
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage= new List<string> { ex.Message } });
            }

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getAuthorInfo")]
        public async Task<ActionResult<BaseResponse>> getAuthorInfo(string userId)
        {
            try
            {
                var response = await this.service.getInfoAsync(userId);
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getMyPosts")]
        public async Task<ActionResult<BaseResponse>> getMyPosts()
        {
            try
            {
                var response = await this.service.getMyPostAsync();
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getMyFavoritePosts")]
        public async Task<ActionResult<BaseResponse>> getMyFavoritePosts()
        {
            try
            {
                var response = await this.service.getMyFavoritePostsAsync();
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("changePassword")]
        public async Task<ActionResult<bool>> changePassword(ChangePassword model)
        {

         

                try
                {

                        if (ModelState.IsValid)
                        {

                            var response = await this.service.changePasswordAsync(model);
                            if (response != null && response.IsSuccess)
                                return Ok(new BaseResponse { Result = "Successfully" });
                            else
                                return BadRequest(response);
                        }
                        else
                        {
                            return BadRequest(new BaseResponse { ErrorsMessage = Helper.GetModelErrors(ModelState).ToList()});
                        }  

                }
                catch (Exception ex)
                {
                    return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
                }

        }

  
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("updateEmail")]
        public async Task<ActionResult<BaseResponse>> updateEmail(UpdateEmail email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this.service.updateEmailAsync(email);
                     return Ok(new BaseResponse { Result ="Successfuly"});
                }
                else
                {
                    return BadRequest(new BaseResponse { ErrorsMessage = Helper.GetModelErrors(ModelState).ToList() });
                }

        }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }


        }


        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("updateName")]
        public async Task<ActionResult<BaseResponse>> updateName(UserNameUpdate model)
        {
            try
            {
                await this.service.updateNameAsync(model);
                return Ok(new BaseResponse { Result = "Successfuly" });

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }


        }

        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("updateImage")]
        public async Task<ActionResult<BaseResponse>> updateImage(UpdateImage model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this.service.updateImageAsync(model);
                    return Ok(new BaseResponse { Result = "Successfully" });
                }
                else
                {
                    return BadRequest(new BaseResponse { ErrorsMessage = Helper.GetModelErrors(ModelState).ToList() });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }


        }


        ///========================== ====================== ================
        ///  Doctor
        ///========================== ====================== ================

        [Authorize(Roles ="Doctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("updateDoctor")]
        public async Task<ActionResult<BaseResponse>> updateDoctor(DoctorUpdate model)
        {
            try
            {
                await this.service.updateDoctorAsync(model);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }



    }
}