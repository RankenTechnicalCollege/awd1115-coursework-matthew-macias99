using Auctions.Models;

namespace Auctions.Data.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext _context;

        public CommentsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Comment commnet)
        {
            _context.Comments.Add(commnet);
            await _context.SaveChangesAsync();
        }
    }
}
