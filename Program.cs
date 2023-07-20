var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddMvc();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}


app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "ContainerRoute",
        pattern: "Home/{action=GetItemData}/{id}",
        defaults: new { controller = "Home" }
    );

    endpoints.MapDefaultControllerRoute();
});


app.Run();
