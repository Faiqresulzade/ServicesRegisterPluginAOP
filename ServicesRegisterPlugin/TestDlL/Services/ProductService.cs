using ServicesRegisterPlugin.Atributes;
namespace TestApi.Services;

[Scoped(nameof(IProductService))]
public class ProductService : IProductService
{
}
