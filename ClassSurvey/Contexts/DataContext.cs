using Microsoft.EntityFrameworkCore;

namespace ClassSurvey.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

}
