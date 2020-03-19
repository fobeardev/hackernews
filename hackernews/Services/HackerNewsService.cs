using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerNews.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private const string NewPostsUri = "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty";
        private const string ItemUri = "https://hacker-news.firebaseio.com/v0/item/";

        public async IAsyncEnumerable<Story> GetAllRecentStoriesAsync()
        {
            using (var client = new HttpClient())
            {
                var newsResponse = await client.GetAsync(new Uri(NewPostsUri)).ConfigureAwait(false);

                var newsItemIds = await JsonSerializer.DeserializeAsync<List<int>>(
                    await newsResponse.Content.ReadAsStreamAsync().ConfigureAwait(false));

                var stories = new List<Story>();

                foreach (var id in newsItemIds)
                {
                    var storyItem = await client.GetAsync(new Uri($"{ItemUri}{id}.json?print=pretty")).ConfigureAwait(false);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };

                    var story = await JsonSerializer.DeserializeAsync<Story>(
                        await storyItem.Content.ReadAsStreamAsync().ConfigureAwait(false), options);

                    // Items can be null, even though they were in the recent list
                    if (story != null)
                    {
                        yield return story;
                    }
                }
            }
        }

        public async Task<IEnumerable<Story>> GetRecentStoriesAsync(int pageNumber = 0, int pageSize = 30)
        {
            using (var client = new HttpClient())
            {
                var newsResponse = await client.GetAsync(new Uri(NewPostsUri)).ConfigureAwait(false);

                var newsItemIds = await JsonSerializer.DeserializeAsync<List<int>>(await newsResponse.Content.ReadAsStreamAsync().ConfigureAwait(false));

                var stories = new List<Story>();

                for (var i = (pageNumber * pageSize); i < newsItemIds.Count && i < (pageNumber * pageSize) + pageSize; i++)
                {
                    var url = $"{ItemUri}{newsItemIds[i]}.json?print=pretty";
                    var storyItem = await client.GetAsync(new Uri(url)).ConfigureAwait(false);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };

                    var story = await JsonSerializer.DeserializeAsync<Story>(
                        await storyItem.Content.ReadAsStreamAsync().ConfigureAwait(false), options);

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
}
