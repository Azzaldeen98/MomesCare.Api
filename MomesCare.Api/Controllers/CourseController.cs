using Microsoft.AspNetCore.Mvc;
using MomesCare.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using MomesCare.Api.ApiClient.Entitis;
using MomesCare.Api.Services;
using MomesCare.Api.Entities.ViewModel.Courses;

namespace MomesCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly CourseServices service;

        public CourseController(CourseServices _service)
        {
            this.service = _service;
        }


        //==================================================================
        // Course 
        //==================================================================

        [Authorize(Roles = ("Admin"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse>> create(CreateCourse model)
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

        [Authorize(Roles = ("Admin"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<BaseResponse>> update(UpdateCourse model)
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

        [Authorize(Roles =("Admin"))]
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

        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<BaseResponse>> getOne(int CourseId)
        {
            try
            {
                var response = await this.service.getOne(CourseId);
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }

        //==================================================================
        // Course Items
        //==================================================================

        [Authorize(Roles = ("Admin"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Route("createCourseItem")]
        public async Task<ActionResult<BaseResponse>> createCourseItem(CreateCourseItem model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this.service.createCourseItemAsync(model);
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

        [Authorize(Roles = ("Admin"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("updateCourseItem")]
        public async Task<ActionResult<BaseResponse>> updateCourseItem(UpdateCourseItem model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this.service.updateCourseItemAsync(model);
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

        [Authorize(Roles = ("Admin"))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        [Route("deleteCourseItem")]
        public async Task<ActionResult<BaseResponse>> deleteCourseItem(int id)
        {
            try
            {
                await this.service.deleteCourseItemAsync(id);
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
        [Route("getCourseItems")]
        public async Task<ActionResult<BaseResponse>> getCourseItems(int courseId)
        {
            try
            {
                var response = await this.service.getCourseItemsAsync(courseId);
                return Ok(new BaseResponse { Result = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { ErrorsMessage = new List<string> { ex.Message } });
            }

        }




    }
}