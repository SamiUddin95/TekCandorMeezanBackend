using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Authorization;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        //[HasPermission("Security.Users")]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAll(pageNumber, pageSize); 

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages,
                    totalUsers = result.TotalUsers,
                    totalActiveUser = result.TotalActiveUser,
                    totalHubUser = result.TotalHubUser,
                    totalBranchUser = result.TotalBranchUser
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound(ApiResponse<string>.Error("User not found"));

            return Ok(ApiResponse<UserDTO>.Success(user));
        }


        [HttpPut("{id}")]
        public IActionResult Update(UserDTO dto)
        {
            var updated = _service.Update(dto);
            if (updated == null)
                return NotFound(ApiResponse<string>.Error("User not found"));

            return Ok(ApiResponse<UserDTO>.Success(updated, 200));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var deleted = _service.SoftDelete(id);
            return Ok(ApiResponse<bool>.Success(deleted, 200));
        }

        public class CreateUserRequest
        {
            public UserDTO User { get; set; } = new UserDTO();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
        {
            try
            {
                var created = await _service.CreateAsync(req.User, req.User.PasswordHash);
                return Ok(ApiResponse<object>.Success(created, 201));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
