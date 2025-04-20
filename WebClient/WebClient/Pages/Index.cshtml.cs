using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.Services;

public class IndexModel : PageModel
{
    private readonly ICatalogServiceClient _catalogServiceClient;

    public IndexModel(ICatalogServiceClient catalogServiceClient)
    {
        _catalogServiceClient = catalogServiceClient;
    }

    public void OnGet()
    {
        // Здесь можно загрузить данные для главной страницы, если необходимо.
    }
}