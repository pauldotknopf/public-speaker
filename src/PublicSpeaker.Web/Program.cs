using PublicSpeaker.Web.Middleware;
using PublicSpeaker.Web.Services;
using SpotifyAPI.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<SpotifyConfig>(builder.Configuration.GetSection("Spotify"));
builder.Services.AddSingleton<SpotifySession>();
builder.Services.AddScoped(context => { return new SpotifyClient(context.GetRequiredService<SpotifySession>().AccessToken); });

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

app.UseMiddleware<SpotifyAuthMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
