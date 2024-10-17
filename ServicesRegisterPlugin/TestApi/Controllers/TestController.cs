using Microsoft.AspNetCore.Mvc;
using TestApi.Services;
namespace TestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    readonly IProductService _productService;
    public TestController(IProductService productService) => _productService = productService;

    [HttpGet]
    public string Get() => "Test";
}
