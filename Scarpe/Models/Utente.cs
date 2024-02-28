namespace Scarpe.Models
{
    public class Utente
    {
        public int idUtente { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
    }
}