var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();


var app = builder.Build();
app.MapControllers();
app.Run();
