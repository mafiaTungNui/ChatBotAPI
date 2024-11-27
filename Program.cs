using Microsoft.EntityFrameworkCore;
using ChatBotAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ChatBotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatBotDb")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatBotAPI V1");
    });
}
app.UseAuthorization();
app.MapControllers();
app.Run();
