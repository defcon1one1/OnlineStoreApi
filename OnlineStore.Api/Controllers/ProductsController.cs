using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetProductById()
    {
        return Ok();
    }

    [HttpPost]
    [Authorize] // for employer
    public async Task<IActionResult> AddProduct()
    {
        return Ok();
    }
    [HttpPut]
    [Authorize] // for employer
    public async Task<IActionResult> EditProduct()
    {
        return Ok();
    }
    [HttpDelete]
    [Authorize] // for employer
    public async Task<IActionResult> DeleteProduct()
    {
        return Ok();
    }
}
