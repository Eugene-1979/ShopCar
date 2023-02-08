using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopCar.Data;
using ShopCar.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// добавление сервисов Idenity
builder.Services.AddDefaultIdentity<IdentityUser>
(
/*options => options.SignIn.RequireConfirmedAccount = true*/
options => {
options.Password.RequireDigit = true; /*Цифра обязательно*/
options.Password.RequiredLength = 5;/* мин длина */
options.Password.RequireUppercase = true; /*обяз верх регистр*/
options.Lockout.MaxFailedAccessAttempts = 5; /*блокировкеа аккаута после неправильн введения*/
options.User.RequireUniqueEmail = true; /*уникальній емаил*/
options.SignIn.RequireConfirmedEmail = false;/* подтверждение почті*/
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();











builder.Services.AddControllersWithViews();

/*MyDatabase*/
IConfigurationRoot conString = builder.Configuration.AddJsonFile("appsettingsShop.json").Build();
builder.Services.AddDbContext<AppDbContent>(option => option.
UseSqlServer(conString.GetConnectionString("DefaultConnection")));

/*Loging*/
builder.Logging.ClearProviders();
builder.Logging.AddConsole();





/*MyExtention method for IoS*/
builder.Services.CreateServiceCollection();













var app = builder.Build();


using IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
SeedData.EnsureSeedData(serviceScope.ServiceProvider);






// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
app.UseMigrationsEndPoint();
}
else
{
app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

/*аутентификации и авторизации*/
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

/*MyMiddleware*/
/*app.UseMiddleware<MyMiddleware>();*/






using(var scope = app.Services.CreateScope())
{
AppDbContent context = scope.ServiceProvider.GetRequiredService<AppDbContent>();


    if(false) { DbObjects.Initial(context); }

    /*   context.Products.RemoveRange(context.Products);
       context.Categorys.RemoveRange(context.Categorys);
       context.Customers.RemoveRange(context.Customers);
       context.Employees.RemoveRange(context.Employees);
       context.Orders.RemoveRange(context.Orders);
       context.Enrollment.RemoveRange(context.Enrollment);

       context.SaveChanges();*/
    }

app.Run();
public partial class Program { }
