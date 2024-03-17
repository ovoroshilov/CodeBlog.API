using CodeBlog.API.Data;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetById(Guid id)
        {
            return await _context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost post)
        {
            var existingBlogPost = await _context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == post.Id);

            if (existingBlogPost is null) return null;

            _context.Entry(existingBlogPost).CurrentValues.SetValues(post);
            existingBlogPost.Categories = post.Categories;

            await _context.SaveChangesAsync();

            return post;
        }
    }
}
