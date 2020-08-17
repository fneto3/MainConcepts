using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TestConcepts.Domain.Interfaces;

namespace TestConcepts.Console
{
    class Program
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string _uriCalculator = "http://localhost/api/calculator/";

        static async Task Main(string[] args)
        {
            //System.Console.WriteLine("Starting Sum();");
            //System.Console.Write("Result: ");
            //System.Console.WriteLine(await Sum(20.0M,20.0M));

            System.Console.ReadLine();
        }

        static async Task<ICalculator> Sum(decimal a, decimal b) =>
            await JsonSerializer.DeserializeAsync<ICalculator>(await _client.GetStreamAsync($"{_uriCalculator}/sum?a={a}&b={b}"));
    }
}
