namespace Scarpe.Models
{
    public class ImmagineArticolo
    {
        public int idImmagine { get; set; }


        public int FK_IdScarpa { get; set; }

        public string PercorsoImmagine { get; set; }

        public string MimeType { get; set; }
    }
}