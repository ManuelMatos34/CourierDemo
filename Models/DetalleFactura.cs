namespace Courier.Models
{
    public class DetalleFactura
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public int IdPaquete { get; set; }
        public string Contenido { get; set; }
        public double Peso { get; set; }
        public int Total { get; set; }
    }
}
