using Microsoft.AspNetCore.Authentication.Cookies;
using Mini.HttpServiceBus.Services;
using Mini.UI.IContract;
using Mini.UI.Services;
using Mini.UI.Utility;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//AddHttpContextAccessor
//TokenService için gerekli
//Baþlýklar, çerezler, sorgu parametreleri ve kullanýcý talepleri gibi HTTP istek
//ve yanýtýnýn çeþitli yönlerine eriþmenizi saðlar
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();



builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
StaticBase.AuthApiBase = builder.Configuration["ServicesUrls:AuthApiUrl"];

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
