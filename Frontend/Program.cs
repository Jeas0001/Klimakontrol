using Frontend;
using Frontend.Components;
using Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// My backend api
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5201/api/") });

// My api service
builder.Services.AddScoped<APIService>();

// AuthenticationService Registration
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        // Cookie options configuration
    });

// Register AppState
builder.Services.AddScoped<AppState>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
