using PublicSpeaker.Web.Middleware;
using PublicSpeaker.Web.Services;
using SpotifyAPI.Web;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllersWithViews();
if(builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.Configure<SpotifyConfig>(builder.Configuration.GetSection("Spotify"));
builder.Services.AddSingleton<SpotifySession>();
builder.Services.AddScoped(context => {
    var sessionInfo = context.GetRequiredService<SpotifySession>().GetSessionInfo();
    if(sessionInfo == null)
    {
        throw new NotSupportedException("can't resolve SpotifyClient without an active session.");
    }
    return new SpotifyClient(sessionInfo.AccessToken);
});
builder.Services.AddHostedService<TokenRefreshService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpLogging();

app.UseStaticFiles();

app.UseMiddleware<SpotifyAuthMiddleware>();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
