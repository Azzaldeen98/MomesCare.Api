using Microsoft.AspNetCore.Mvc;
using MomesCare.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using MomesCare.Api.ApiClient.Entitis;
using MomesCare.Api.Services;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;

namespace MomesCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class DailyCareTimesController : ControllerBase
    {
        private readonly DailyCareTimesServices service;

        public DailyCareTimesController(DailyCareTimesServices _service)
        {
            this.service = _service;
        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse>> create(DailyCareTimesCreate model)
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
        public async Task<ActionResult<BaseResponse>> update(DailyCareTimesUpdate model)
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


        [Authorize(Roles ="Admin")]
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
                return BadRequest(new BaseResponse { ErrorsMessage= new List<string> { ex.Message } });
            }

        }

 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<BaseResponse>> getOne(int  id)
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


        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getBabyDailyCareTimes")]
        public async Task<ActionResult<BaseResponse>> getBabyDailyCareTimes(int babyId)
        {
            try
            {
                var response = await this.service.getBabyDailyCareTimesAsync(babyId);
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }


    }
}