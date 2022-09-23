using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Usuario/Login";
            options.LogoutPath = "/Usuario/Logout";
            options.AccessDeniedPath = "/Home/Restringido/";
        });

builder.Services.AddAuthorization(options => 
{
        options.AddPolicy("Administrador",
                    policy => policy.RequireRole("Administrador","Empleado"));
});  

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapControllerRoute( name: "Login" ,"entrar/{**accion}", new {controller = "Usuario",action = "Login"});
});

/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/

app.Run();
