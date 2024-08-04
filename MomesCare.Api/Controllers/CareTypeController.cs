using Microsoft.AspNetCore.Mvc;
using MomesCare.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using MomesCare.Api.ApiClient.Entitis;
using MomesCare.Api.Services;
using MomesCare.Api.Entities.ViewModel.CareType;

namespace MomesCare.Api.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CareTypeController : ControllerBase
    {
        private readonly CareTypeServices service;

        public CareTypeController(CareTypeServices _service)
        {
            this.service = _service;
        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse>> create(CareTypeCreate model)
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
                //if(ex is ExistsException)

                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }


        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<BaseResponse>> update(CareTypeUpdate model)
        {


            try
            {
                if (ModelState.IsValid)
                {
                    await this.service.updateAsync(model);
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

        [Authorize(Roles = "Admin")]
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

    
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<BaseResponse>> getAll()
        {
            try
            {
                var response = await this.service.getAllAsync();
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }

    
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getAllWithDailyCareTimes")]
        public async Task<ActionResult<BaseResponse>> getAllWithDailyCareTimes()
        {
            try
            {
                var response = await this.service.getAllWithItemsAsync();
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
        public async Task<ActionResult<BaseResponse>> getOne(int CareTypeId)
        {
            try
            {
                var response = await this.service.getOne(CareTypeId);
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


    }
}