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

                yield return await JsonSerializer.DeserializeAsync<Story>(
                    await storyItem.Content.ReadAsStreamAsync(), options);
            }

        }
    }
}