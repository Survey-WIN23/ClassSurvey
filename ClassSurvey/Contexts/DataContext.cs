using Microsoft.EntityFrameworkCore;

namespace ClassSurvey.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    //public DbSet<QuestionEntity> Questions { get; set; }
    //public DbSet<OptionEntity> Options { get; set; }
    //public DbSet<AnswerEntity> Answers { get; set; }
}
