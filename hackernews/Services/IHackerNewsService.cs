using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Services
{
    public interface IHackerNewsService
    {
        IAsyncEnumerable<Story> GetAllRecentStoriesAsync();
        Task<IEnumerable<Story>> GetRecentStoriesAsync(int pageNumber, int pageSize);
    }
}
