namespace ClassSurvey.Configurations
{
    public static class ExceptionHandlingConfiguration
    {
        public static void ConfigureExceptionHandling(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
        }
    }
}
