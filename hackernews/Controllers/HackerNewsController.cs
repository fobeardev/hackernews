using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HackerNews.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        public HackerNewsController(IHackerNewsService hackerNews)
        {
            HackerNews = hackerNews;
        }

        public IHackerNewsService HackerNews { get; }

        /// <summary>
        /// Naive implementation to get ALL approx 500 recent stories
        /// </summary>
        /// <returns>Recent Stories from Hacker News</returns>
        [HttpGet]
        public IAsyncEnumerable<Story> Get()
        {
            return HackerNews.GetAllRecentStoriesAsync();
        }

        /// <summary>
        /// Paging implementation of recent stories
        /// </summary>
        /// <param name="pageNumber">0 based index of page</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <returns>Recent stories starting at (pageNumber * pageSize) and returning the next pageSize results</returns>
        [HttpGet]
        [Route("recent/{pageNumber}/{pageSize?}")]
        public async Task<IEnumerable<Story>> GetPage(int pageNumber = 0, int pageSize = 30)
        {
            return await HackerNews.GetRecentStoriesAsync(pageNumber, pageSize).ConfigureAwait(false);
        }
    }
}