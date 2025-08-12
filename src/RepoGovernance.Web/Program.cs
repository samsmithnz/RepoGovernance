using RepoGovernance.Web.Services;
using RepoGovernance.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);

//Add DI for the service api client 
builder.Services.AddScoped<ISummaryItemsServiceApiClient, SummaryItemsServiceApiClient>();

//Add DI for the ignored recommendations data access 
builder.Services.AddScoped<IIgnoredRecommendationsDA>(provider =>
{
    IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("DefaultConnection") ?? "UseDevelopmentStorage=true";
    return new IgnoredRecommendationsDA(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
