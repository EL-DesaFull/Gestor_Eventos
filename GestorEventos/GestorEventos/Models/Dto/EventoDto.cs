using GestorEventos.Models.Dto;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GestorEventos.Models.Dto
{   
    public class EventoDto
    {
        public int eventId { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string eventName { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string eventDescription { get; set; }
        [Required]
        public string startDate { get; set; }
        [Required]
        public string endDate { get; set; }
        [Required]
        public string eventStatus { get; set; }
    }
}