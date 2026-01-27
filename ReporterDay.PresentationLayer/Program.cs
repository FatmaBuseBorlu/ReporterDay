using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.BusinessLayer.Concrete;
using ReporterDay.BusinessLayer.Models;
using ReporterDay.DataAccessLayer.Abstract;
using ReporterDay.DataAccessLayer.Context;
using ReporterDay.DataAccessLayer.EntityFramework;
using ReporterDay.EntityLayer.Entities;
using ReporterDay.PresentationLayer.Extensions;
using ReporterDay.PresentationLayer.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

builder.Services.AddScoped<ISliderService, SliderManager>();
builder.Services.AddScoped<ISliderDal, EfSliderDal>();

builder.Services.AddScoped<IArticleService, ArticleManager>();
builder.Services.AddScoped<IArticleDal, EfArticleDal>();

builder.Services.AddScoped<ITagService, TagManager>();
builder.Services.AddScoped<ITagDal, EfTagDal>();

builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<ICommentDal, EfCommentDal>();

builder.Services.AddDataProtection();
builder.Services.AddScoped<IArticleIdProtector, ArticleIdProtector>();

builder.Services.AddDbContext<ArticleContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ArticleContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddPresentationServices();

builder.Services.AddMemoryCache();

builder.Services.Configure<HuggingFaceOptions>(
    builder.Configuration.GetSection("HuggingFace"));
builder.Services.Configure<CommentModerationOptions>(
    builder.Configuration.GetSection("CommentModeration"));

builder.Services.AddScoped<ICommentModerationService, CommentModerationManager>();


builder.Services.AddHttpClient<IToxicityService, ToxicityManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/test-config", (IConfiguration cfg, IWebHostEnvironment env) =>
    {
        var token = cfg["HuggingFace:ApiToken"];
        var baseUrl = cfg["HuggingFace:BaseUrl"];
        var modelId = cfg["HuggingFace:ModelId"];

        return Results.Json(new
        {
            env = env.EnvironmentName,
            hasToken = !string.IsNullOrWhiteSpace(token),
            tokenPrefix = string.IsNullOrWhiteSpace(token) ? "" : token.Substring(0, Math.Min(6, token.Length)),
            baseUrl,
            modelId
        });
    });

    app.MapGet("/test-tox", async (IToxicityService tox) =>
    {
        var toxic = await tox.CheckAsync("you are an idiot");
        var clean = await tox.CheckAsync("have a nice day");
        return Results.Json(new { toxic, clean });
    });
}

app.MapGet("/_endpoints", (IEnumerable<EndpointDataSource> sources) =>
{
    var list = sources.SelectMany(s => s.Endpoints)
                      .Select(e => e.DisplayName)
                      .Where(x => !string.IsNullOrWhiteSpace(x));

    return string.Join("\n", list);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
