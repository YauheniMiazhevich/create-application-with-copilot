using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackendApi.Interfaces;
using BackendApi.Models.DTOs;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetProperty(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            
            if (property == null)
                return NotFound(new { message = $"Property with ID {id} not found" });

            return Ok(property);
        }

        [HttpPost]
        public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto createPropertyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var property = await _propertyService.CreatePropertyAsync(createPropertyDto);
                return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the property", error = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, [FromBody] UpdatePropertyDto updatePropertyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var property = await _propertyService.UpdatePropertyAsync(id, updatePropertyDto);
                
                if (property == null)
                    return NotFound(new { message = $"Property with ID {id} not found" });

                return Ok(property);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the property", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                var result = await _propertyService.DeletePropertyAsync(id);
                
                if (!result)
                    return NotFound(new { message = $"Property with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
