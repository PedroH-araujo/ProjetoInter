using ProjetoInter.Data;
using ProjetoInter.Services.User;
using ProjetoInter.Services.Produto;
using Microsoft.EntityFrameworkCore;
using ProjetoInter.Helper;
using ProjetoInter.Services.MarketCar;

//     "DefaultConnection": "server=localhost;database=Production;User=aluno;Password=dba;TrustServerCertificate=True"
//     "DefaultConnection": "server=DESKTOP-K2MKISJ; database=Production; trusted_connection = true; trustservercertificate=true"

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Inje��o de depend�ncia
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserInterface, UserService>(); 
builder.Services.AddScoped<IProductInterface, ProductService>();
builder.Services.AddScoped<ISessionInterface, Session>();
builder.Services.AddScoped<IMarketCarInterface, MarketCarService>();

builder.Services.AddDistributedMemoryCache(); //armazena as informa��es da sess�o no cahce

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; //cookie s� pode ser acessado via http
    options.Cookie.IsEssential = true; // Isso significa que mesmo que o usu�rio tenha optado por n�o aceitar cookies, este cookie ser� enviado com cada solicita��o HTTP, garantindo que a sess�o funcione corretamente.
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
