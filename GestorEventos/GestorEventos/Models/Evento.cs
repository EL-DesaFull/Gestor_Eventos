namespace GestorEventos.Models
{
    public class Evento
    {
        public int eventId { get; set; }
        public string eventName { get; set; }
        public string eventDescription { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string eventStatus { get; set; }
        public DateTime eventCreated { get; set; }
        public DateTime eventUpdated { get; set; }

    }
}
