using ProjetoInter.Data;
using ProjetoInter.Services.User;
using ProjetoInter.Services.Produto;
using Microsoft.EntityFrameworkCore;
using ProjetoInter.Helper;
using ProjetoInter.Services.MarketCar;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Injeção de dependência
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserInterface, UserService>(); 
builder.Services.AddScoped<IProductInterface, ProductService>();
builder.Services.AddScoped<ISessionInterface, Session>();
builder.Services.AddScoped<IMarketCarInterface, MarketCarService>();

builder.Services.AddDistributedMemoryCache(); //armazena as informações da sessão no cahce

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; //cookie só pode ser acessado via http
    options.Cookie.IsEssential = true; // Isso significa que mesmo que o usuário tenha optado por não aceitar cookies, este cookie será enviado com cada solicitação HTTP, garantindo que a sessão funcione corretamente.
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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=login}/{action=Index}/{id?}");

app.Run();
