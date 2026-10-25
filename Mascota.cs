namespace ClinicaVeterinaria
{
    public class Mascota
    {
        public int MascotaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raza { get; set; } = string.Empty;
        public int Edad { get; set; }
        public int ClienteID { get; set; }
    }
}