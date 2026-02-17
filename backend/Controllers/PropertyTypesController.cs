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
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public PropertyTypesController(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyTypeDto>>> GetAllPropertyTypes()
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();
            var propertyTypeDtos = propertyTypes.Select(pt => new PropertyTypeDto
            {
                Id = pt.Id,
                Type = pt.Type
            });

            return Ok(propertyTypeDtos);
        }
    }
}
