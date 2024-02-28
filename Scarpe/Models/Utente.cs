namespace Scarpe.Models
{
    public class Utente
    {
        public int idUtente { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }

        public Utente()
        {

        }
        public Utente(int idUtente, string Email, string Password, bool isAdmin)
        {
            this.idUtente = idUtente;
            this.Email = Email;
            this.Password = Password;
            this.isAdmin = isAdmin;
        }
    }
}