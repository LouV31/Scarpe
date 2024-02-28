using System.Collections.Generic;

namespace Scarpe.Models
{
    public class Scarpa
    {
        //[ScaffoldColumn(false)]
        public int idScarpa { get; set; }
        public string NomeScarpa { get; set; }
        public string Descrizione { get; set; }
        public string ImmagineCopertina { get; set; }
        public decimal Prezzo { get; set; }
        public bool Attivo { get; set; }

        public List<Dettagli> dettagli { get; set; }

        public Scarpa(int idScarpa, string nomeScarpa, string descrizione, string immagineCopertina, decimal prezzo, bool attivo)
        {
            this.idScarpa = idScarpa;
            NomeScarpa = nomeScarpa;
            Descrizione = descrizione;
            ImmagineCopertina = immagineCopertina;
            Prezzo = prezzo;
            Attivo = attivo;
        }

        public Scarpa()
        {
        }
    }
}