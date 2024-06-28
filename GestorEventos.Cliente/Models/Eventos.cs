using System.ComponentModel.DataAnnotations;

namespace GestorEventos.Cliente.Models
{
    public class Eventos
    {
        public int eventId { get; set; }
        
        [Required(ErrorMessage ="Este campo es requerido")]
        [MaxLength(30, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string eventName { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string eventDescription { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        public string startDate { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        public string endDate { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        public string eventStatus { get; set; }
    }
}
