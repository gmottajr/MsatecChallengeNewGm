using MsaTec.IoC;
using MsaTec.IoC.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.SetupDbContext(builder.Configuration);
builder.Services.AddDbInitializer();
builder.Services.ResolveDependencies();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Allow requests from this origin
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.Configuration.AddUserSecrets<Program>(true);
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();

// Seed data when the application starts
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetService<IDbInitializer>();
    initializer.Initialize();
}

app.Run();
