using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackendApi.Interfaces;
using BackendApi.Models.DTOs;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypesController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyTypeDto>>> GetAllPropertyTypes()
        {
            var propertyTypes = await _propertyTypeService.GetAllPropertyTypesAsync();
            return Ok(propertyTypes);
        }
    }
}
