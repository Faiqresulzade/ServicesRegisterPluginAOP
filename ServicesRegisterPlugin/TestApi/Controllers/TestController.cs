using Microsoft.AspNetCore.Mvc;
using TestApi.Services;
namespace TestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    readonly IProductService _productService;
    readonly CategoryService _categoryService;
    public TestController(IProductService productService, CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    [HttpGet]
    public string Get() => "Test";
}
