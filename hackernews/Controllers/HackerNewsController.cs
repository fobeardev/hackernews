using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hackernews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private const string NewPostsUri = "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty";
        private const string ItemUri = "https://hacker-news.firebaseio.com/v0/item/";

        /// <summary>
        /// Naive implementation to get ALL approx 500 recent stories
        /// </summary>
        /// <returns>Recent Stories from Hacker News</returns>
        [HttpGet]
        public async IAsyncEnumerable<Story> Get()
        {
            var client = new HttpClient();

            var newsResponse = await client.GetAsync(NewPostsUri);

            var newsItemIds = await JsonSerializer.DeserializeAsync<List<int>>(
                await newsResponse.Content.ReadAsStreamAsync());

            var stories = new List<Story>();

            foreach (var id in newsItemIds)
            {
                var storyItem = await client.GetAsync($"{ItemUri}{id}.json?print=pretty");
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var story = await JsonSerializer.DeserializeAsync<Story>(
                    await storyItem.Content.ReadAsStreamAsync(), options);

                // Items can be null, even though they were in the recent list
                if (story != null)
                {
                    yield return story;
                }
            }
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
            var client = new HttpClient();

            var newsResponse = await client.GetAsync(NewPostsUri);

            var newsItemIds = await JsonSerializer.DeserializeAsync<List<int>>(
                await newsResponse.Content.ReadAsStreamAsync());

            var stories = new List<Story>();

            for (var i = (pageNumber * pageSize); i < newsItemIds.Count && i < (pageNumber * pageSize) + pageSize; i++)
            {
                var url = $"{ItemUri}{newsItemIds[i]}.json?print=pretty";
                var storyItem = await client.GetAsync(url);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var story = await JsonSerializer.DeserializeAsync<Story>(
                    await storyItem.Content.ReadAsStreamAsync(), options);

                // Items can be null, even though they were in the recent list
                if (story != null)
                {
                    stories.Add(story);
                }
            }

            return stories;
        }
    }
}