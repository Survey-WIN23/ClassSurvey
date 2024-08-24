namespace ClassSurvey.Configurations
{
    public static class EndpointsConfiguration
    {
        public static void ConfigureEndpoints(this WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}

