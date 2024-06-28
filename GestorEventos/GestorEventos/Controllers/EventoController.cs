using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestorEventos.Models;
using GestorEventos.Models.Dto;
using GestorEventos.Datos;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace GestorEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {

        private readonly ILogger<EventoController> _logger;
        private readonly IConfiguration _configuration;


        public EventoController(
            ILogger<EventoController> logger,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _configuration = configuration;
            StoredEvents.cargarEventoList();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EventoDto>> GetEventos() 
        {
            _logger.LogInformation("Obtener todos los eventos");
            return Ok(StoredEvents.eventoList);
        }

        [HttpGet("id:int", Name = "GetEvento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EventoDto> GetEvento(int id) 
        {
            if (id == 0) {
                _logger.LogError("Error al cargar el evento con id: " + id);
                return BadRequest();
            }

            EventoDto evento = StoredEvents.eventoList.FirstOrDefault(x => x.eventId == id);
            
            if (evento == null) {
                _logger.LogError("Error: Evento con id: " + id + " no encontrado");
                return NotFound();
            }

            _logger.LogInformation("Evento con id " + id + " encontrado!");
            return Ok(evento);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<EventoDto> CrearEvento([FromBody] EventoDto eventoDto) 
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear el evento");
                return BadRequest();
            }

            //Validacion Personalizada
            if (StoredEvents.eventoList.FirstOrDefault(x => x.eventName.ToLower() == eventoDto.eventName.ToLower()) != null)
            {
                string msg = $"El nombre del evento {eventoDto.eventName} ya existe!";
                ModelState.AddModelError("Nombre de Evento Existente", msg);
                return BadRequest(ModelState);
            }

            if (eventoDto == null) { 
                return BadRequest(eventoDto);
            }

            if (eventoDto.eventId > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            eventoDto.eventId=StoredEvents.eventoList.OrderByDescending(x => x.eventId).FirstOrDefault().eventId + 1;
            StoredEvents.eventoList.Add(eventoDto);
            string json = JsonConvert.SerializeObject(StoredEvents.eventoList);
            string path = ObtenerUrl("JsonUrl");
            System.IO.File.WriteAllText(path, json);
            return CreatedAtRoute("GetEvento", new {id = eventoDto.eventId}, eventoDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEvento(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            EventoDto evento = StoredEvents.eventoList.FirstOrDefault(x => x.eventId == id);

            if (evento == null) 
            {
                return NotFound();
            }

            string filePath = ObtenerUrl("JsonUrl");
            string json = System.IO.File.ReadAllText(filePath);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
            foreach (var item in data)
            {
                if (item["eventId"] == id)
                {
                    data.Remove(item);
                    break;
                }
            }

            string updatedJson = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(filePath, updatedJson);

            StoredEvents.eventoList.Remove(evento);

            return NoContent();

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEvento(int id, [FromBody] EventoDto eventoDto) 
        { 
            if (eventoDto == null || id != eventoDto.eventId)
            {
                return BadRequest();
            }

            EventoDto evento = StoredEvents.eventoList.FirstOrDefault(x => x.eventId == id);

            evento.eventName = eventoDto.eventName;
            evento.eventDescription = eventoDto.eventDescription;
            evento.startDate = eventoDto.startDate;
            evento.endDate = eventoDto.endDate;
            evento.eventStatus = eventoDto.eventStatus;

            string filePath = ObtenerUrl("JsonUrl");
            string json = System.IO.File.ReadAllText(filePath);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (var item in data)
            {
                if (item["eventId"] == id)
                {
                    item["eventName"] = evento.eventName;
                    item["eventDescription"] = evento.eventDescription;
                    item["startDate"] = evento.startDate;
                    item["endDate"] = evento.endDate;
                    item["eventStatus"] = evento.eventStatus;
                    System.IO.File.WriteAllText(filePath, data.ToString());
                }
            }


            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialEvento(int id, JsonPatchDocument<EventoDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            EventoDto evento = StoredEvents.eventoList.FirstOrDefault(x => x.eventId == id);

            patchDto.ApplyTo(evento, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }

        private string ObtenerUrl(string valorAppSettings)
        {
            return $"{_configuration.GetValue<string>(valorAppSettings)}";
        }
    }
}
