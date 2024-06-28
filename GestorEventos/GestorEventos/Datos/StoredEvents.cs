using GestorEventos.Models.Dto;
using Newtonsoft.Json;

namespace GestorEventos.Datos
{
    public static class StoredEvents
    {
        public static List<EventoDto> eventoList = new List<EventoDto>();

        public static void cargarEventoList()
        {
            string jsonStr = File.ReadAllText("event_list.json"); 
            List<EventoDto> fromJson = JsonConvert.DeserializeObject<List<EventoDto>>(jsonStr);
            eventoList.Clear();
            eventoList.AddRange(fromJson);
        }
    }
}
