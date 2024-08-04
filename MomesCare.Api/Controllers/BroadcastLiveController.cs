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
using MomesCare.Api.Entities.ViewModel.BroadcastLive;

namespace MomesCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BroadcastLiveController : ControllerBase
    {
        private readonly BroadcastLiveServices service;

        public BroadcastLiveController(BroadcastLiveServices services)
        {
            this.service = services;
        }


        [Authorize(Roles = ("Doctor"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse>> create(CreateBroadcastLive model)
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
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } }); ;
            }

        }

        [Authorize(Roles = ("Doctor"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<BaseResponse>> update(UpdateBroadcastLive model)
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

        [Authorize(Roles = ("Doctor"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("NotifyUsersOfBroadcast")]
        public async Task<ActionResult<BaseResponse>> NotifyUsersOfBroadcast(int id)
        {
            try
            {
                await this.service.NotifyUsersOfBroadcastAsync(id);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


        [Authorize(Roles = ("Doctor"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("ActiveBroadcastLive")]
        public async Task<ActionResult<BaseResponse>> ActiveBroadcastLive(int id)
        {
            try
            {
                await this.service.ActiveBroadcastLiveAsync(id);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }

        [Authorize(Roles = ("Doctor"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("InActiveBroadcastLive")]
        public async Task<ActionResult<BaseResponse>> InActiveBroadcastLive(int id)
        {
            try
            {
                await this.service.InActiveBroadcastLiveAsync(id);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


        [Authorize(Roles = ("Doctor"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getMyBroadcastLives")]
        public async Task<ActionResult<BaseResponse>> getMyBroadcastLives()
        {
            try
            {
                var response = await this.service.getMyBroadcastLivesAsync();
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }



        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getActivesBroadcastLives")]
        public async Task<ActionResult<BaseResponse>> getActivesBroadcastLives()
        {
            try
            {
                var response = await this.service.getActivesBroadcastLivesAsync();
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<BaseResponse>> getOne(int id)
        {
            try
            {
                var response = await this.service.getOne(id);
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


        [Authorize(Roles = ("Doctor,Admin"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<BaseResponse>> delete(int id)
        {
            try
            {
                await this.service.deleteAsync(id);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }






    }
}