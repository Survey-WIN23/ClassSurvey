namespace ClassSurvey.Configurations;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();
    }
}
