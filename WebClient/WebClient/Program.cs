using WebClient.Services;

var builder = WebApplication.CreateBuilder(args);

var catalogServiceUrl = builder.Configuration["CatalogService:Url"];
var orderServiceUrl = builder.Configuration["OrderService:Url"];


builder.Services.AddGrpcClient<CatalogService.GrpcContracts.Catalog.CatalogClient>(o =>
{
    o.Address = new Uri(catalogServiceUrl);
});

// Register OrderService gRPC client
builder.Services.AddGrpcClient<OrderService.GrpcContracts.OrderService.OrderServiceClient>(o =>
{
    o.Address = new Uri(orderServiceUrl);
});

builder.Services.AddScoped<ICatalogServiceClient, CatalogServiceClient>();
builder.Services.AddScoped<IOrderServiceClient, OrderServiceClient>();
builder.Services.AddScoped<OrderStateService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
