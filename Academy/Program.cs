using Academy.Data;
using Academy.Services;
using Academy.Services.Interfaces;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Academy.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IAboutUsService, AboutUsService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IMissionService, MissionService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IImpactItemService, ImpactItemService>();
builder.Services.AddScoped<IImpactSectionService, ImpactSectionService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IContactItemService, ContactItemService>();
builder.Services.AddScoped<IContactSectionService, ContactSectionService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseFeatureService, CourseFeatureService>();
builder.Services.AddScoped<ICourseRequirementService, CourseRequirementService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSignalR();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string[] roleNames = { "SuperAdmin", "Admin", "Muellim", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var users = new[]
    {
        new { Email = "superadmin@iguru.az", Password = "SuperAdmin123!", Role = "SuperAdmin" },
        new { Email = "admin@iguru.az", Password = "Admin123!", Role = "Admin" },
        new { Email = "muellim@iguru.az", Password = "Muellim123!", Role = "Muellim" },
        new { Email = "user@iguru.az", Password = "User123!", Role = "User" }
    };

    foreach (var u in users)
    {
        if (await userManager.FindByEmailAsync(u.Email) == null)
        {
            var user = new AppUser
            {
                UserName = u.Email,
                Email = u.Email,
                EmailConfirmed = true,
                FullName = u.Role
            };
            var result = await userManager.CreateAsync(user, u.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, u.Role);
            }
        }
    }
}

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

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<Academy.Hubs.LiveClassHub>("/liveclasshub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
