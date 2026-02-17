using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackendApi.Interfaces;
using BackendApi.Models.DTOs;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnersController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetOwner(int id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            
            if (owner == null)
                return NotFound(new { message = $"Owner with ID {id} not found" });

            return Ok(owner);
        }

        [HttpPost]
        public async Task<ActionResult<OwnerDto>> CreateOwner([FromBody] CreateOwnerDto createOwnerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var owner = await _ownerService.CreateOwnerAsync(createOwnerDto);
                return CreatedAtAction(nameof(GetOwner), new { id = owner.Id }, owner);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<OwnerDto>> UpdateOwner(int id, [FromBody] UpdateOwnerDto updateOwnerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var owner = await _ownerService.UpdateOwnerAsync(id, updateOwnerDto);
                
                if (owner == null)
                    return NotFound(new { message = $"Owner with ID {id} not found" });

                return Ok(owner);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            try
            {
                var result = await _ownerService.DeleteOwnerAsync(id);
                
                if (!result)
                    return Conflict(new { message = "Cannot delete owner with associated companies or properties" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
