using CatalogService.GrpcContracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.Services;

public class CatalogModel : PageModel
{
    private readonly ICatalogServiceClient _catalogServiceClient;

    public List<Product> Products { get; set; } = new List<Product>();

    public CatalogModel(ICatalogServiceClient catalogServiceClient)
    {
        _catalogServiceClient = catalogServiceClient;
    }

    public async Task OnGetAsync()
    {
        var response = await _catalogServiceClient.GetAllProductsAsync();
        Products = response.Products.ToList();
    }
}