using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

AppConfiguration.AddServices(builder);
AppConfiguration.AddCache(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

AppConfiguration.AddAuthentication(builder);
AppConfiguration.AddSwagger(builder);

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

AppConfiguration.AddWebSocket(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();