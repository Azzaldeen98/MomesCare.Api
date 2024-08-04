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
using MomesCare.Api.Entities.ViewModel.Comment;

namespace MomesCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly CommentServices service;

        public CommentController(CommentServices commentServices)
        {
            this.service = commentServices;
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse>> create(CommentCreate model)
        {
            try
            {
                await this.service.createAsync(model);
                return Ok(new BaseResponse { Result = "Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } }); ;
            }

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("likeUnlike")]
        public async Task<ActionResult<BaseResponse>> likeUnlike(int id)
        {
            try
            {
                var response = await this.service.likeUnlikeAsync(id);
                return Ok(new BaseResponse { Result = response });

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } }); ;
            }

        }



        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<BaseResponse>> update(CommentUpdate model)
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
        
        
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<BaseResponse>> getAll(int postId)
        {
            try
            {
               var response = await this.service.getAllCommentsAsync(postId);
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
        [Route("getComment")]
        public async Task<ActionResult<BaseResponse>> getComment(int id)
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


    }
}