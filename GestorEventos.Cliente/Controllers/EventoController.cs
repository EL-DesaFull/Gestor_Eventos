using GestorEventos.Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GestorEventos.Cliente.Controllers
{
    public class EventoController : Controller
    {
        private readonly HttpClient _httpClient;

        public EventoController(IHttpClientFactory httpClientFactory) 
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44391/api");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Evento");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var eventos = JsonConvert.DeserializeObject<IEnumerable<EventoViewModel>>(content);
                return View("Index",eventos);
            }

            return View(new List<EventoViewModel>());
        }
    }
}
