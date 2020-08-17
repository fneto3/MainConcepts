using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TestConcepts.Web.Models;

namespace TestConcepts.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;

        public IndexModel(ILogger<IndexModel> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAddUserAsync()
        {
            return await _client.PostAsync<UserModel>("api/User", );
        }
    }
}
