using ClassSurvey.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();
app.ConfigureExceptionHandling();
app.ConfigureMiddleware();
app.ConfigureEndpoints();
app.Run();
