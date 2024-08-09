using ClassSurvey.Contexts;
using ClassSurvey.Factories;
using ClassSurvey.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClassSurvey.Repositories
{
    public abstract class BaseRepository<TEntity>(DataContext context) where TEntity : class
    {
        private readonly DataContext _context = context;

        public virtual async Task<ResponseResult> CreateOneAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            catch (Exception ex)
            {
                return ResponseFactory.Error(ex.Message);
            }
        }

        public virtual async Task<ResponseResult> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
                if (result is null)
                {
                    return ResponseFactory.NotFound();
                }
                return ResponseFactory.Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Error(ex.Message);
            }
        }

        public virtual async Task<ResponseResult> GetAllAsync()
        {
            try
            {
                var result = await _context.Set<TEntity>().ToListAsync();
                return ResponseFactory.Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Error(ex.Message);
            }
        }
    }
}
