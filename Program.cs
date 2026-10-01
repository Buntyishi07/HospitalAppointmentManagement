// Create the web application builder.
var builder = WebApplication.CreateBuilder(args);

// Add MVC services to the application.
builder.Services.AddControllersWithViews();

// Add memory cache support to the application.
builder.Services.AddMemoryCache();

// Add session support to the application.
builder.Services.AddSession(options =>
{
    // Set the session timeout to 20 minutes.
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    // Make the session cookie unavailable to client-side JavaScript.
    options.Cookie.HttpOnly = true;

    // Allow the session cookie to work for normal requests.
    options.Cookie.IsEssential = true;
});

// Add response caching services.
builder.Services.AddResponseCaching();

// Register HttpClient for making API requests.
builder.Services.AddHttpClient();

// Build the web application.
var app = builder.Build();

// Enable response caching middleware.
app.UseResponseCaching();

// Enable HTTPS redirection.
app.UseHttpsRedirection();

// Enable static files such as CSS and JavaScript.
app.UseStaticFiles();

// Enable routing.
app.UseRouting();

// Enable session before controllers are executed.
app.UseSession();

// Enable authorization middleware.
app.UseAuthorization();

// Configure the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Start the application.
app.Run();