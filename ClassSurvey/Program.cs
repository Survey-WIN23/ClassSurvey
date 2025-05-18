using ClassSurvey.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// COmment out this line to prevent the application from crash locally.
//await app.SeedSuperAdminAsync();

app.ConfigureExceptionHandling();
app.ConfigureMiddleware();
app.ConfigureEndpoints();
app.Run();
