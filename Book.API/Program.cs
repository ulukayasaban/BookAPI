using Book.API.Data;
using Book.API.Mappings;
using Book.API.Models;
using Book.API.Repositories;
using Book.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins="_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => 
{
    options.AddPolicy(MyAllowSpecificOrigins,policy => 
    {
        policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite("Data Source=products.db"));
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<AppDbContext>();//identity için context belirttim

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/openapi/v1.json","api");
    });
    
}


//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();



app.Run();


