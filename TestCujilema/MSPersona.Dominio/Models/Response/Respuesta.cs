namespace MSPersona.Dominio.Models.Response
{
    public class Respuesta
    {
        public int Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public object Data { get; set; }        
    }
}
