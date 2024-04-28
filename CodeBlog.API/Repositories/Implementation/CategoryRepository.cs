using CodeBlog.API.Data;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;
     

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory is not null)
            {
                context.Categories.Remove(existingCategory);
                await context.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (existingCategory is not null)
            {
                context.Entry(existingCategory).CurrentValues.SetValues(category);
                await context.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
