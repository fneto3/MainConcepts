using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestConcepts.Domain;

namespace TestConcepts.Web.Services
{
    public class UserService
    {
        public HttpClient Client { get; }

        public UserService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:2020/api/User");

            Client = client;
        }

        public async Task<User> CreateAsync(User user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/", content);

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
                return await JsonSerializer.DeserializeAsync<User>(responseStream);
        }

        public async Task DeleteAsync(int userId)
        {
            var response = await Client.DeleteAsync($"/{userId}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            var response = await Client.GetAsync("/");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
                return await JsonSerializer.DeserializeAsync<IEnumerable<User>>(responseStream);
        }

        public async Task<User> GetAsync(int userId)
        {
            var response = await Client.GetAsync($"/{userId}");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
                return await JsonSerializer.DeserializeAsync<User>(responseStream);
        }
    }
}
