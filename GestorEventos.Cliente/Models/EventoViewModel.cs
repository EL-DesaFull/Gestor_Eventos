using System.ComponentModel.DataAnnotations;

namespace GestorEventos.Cliente.Models
{
    public class EventoViewModel
    {
        public int eventId { get; set; }
        [Required]
        [MaxLength(30)]
        public string eventName { get; set; }
        [Required]
        [MaxLength(100)]
        public string eventDescription { get; set; }
        [Required]
        public string startDate { get; set; }
        [Required]
        public string endDate { get; set; }
        [Required]
        public string eventStatus { get; set; }
    }
}
