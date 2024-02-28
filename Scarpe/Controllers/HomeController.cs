using Scarpe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Scarpe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string connection = ConfigurationManager.ConnectionStrings["ScarpeDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            List<Scarpa> listaScarpe = new List<Scarpa>();
            try
            {
                conn.Open();
                string query = "Select * from Articoli WHERE Attivo = 'True' ";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Scarpa scarpa = new Scarpa(
                        Convert.ToInt32(reader["idScarpa"]),
                        reader["NomeScarpa"].ToString(),
                        reader["Descrizione"].ToString(),
                        reader["ImmagineCopertina"].ToString(),
                        Convert.ToDecimal(reader["Prezzo"]),
                        reader["Attivo"].ToString() == "1" ? true : false
                        );
                    listaScarpe.Add(scarpa);
                }

            }
            catch (Exception e)
            {
                Response.Write("Error: ");
                Response.Write(e);
            }
            finally
            {
                conn.Close();
            }
            return View(listaScarpe);
        }


        public ActionResult Dettagli(int id)
        {
            string connection = ConfigurationManager.ConnectionStrings["ScarpeDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connection);
            Dettagli dettagli = new Dettagli();
            List<Dettagli> listaImmagini = new List<Dettagli>();

            try
            {
                conn.Open();
                string query = "SELECT A.NomeScarpa, A.Descrizione, I.PercorsoImmagine FROM Articoli AS A INNER JOIN ImmaginiArticoli AS I ON A.idScarpa = I.FK_IdScarpa WHERE A.idScarpa = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nomeScarpa = reader["NomeScarpa"].ToString();
                    string descrizione = reader["Descrizione"].ToString();
                    //string percorsoImmagine = reader["PercorsoImmagine"].ToString();
                    string immagineCopertina = reader["ImmagineCopertina"].ToString();

                    dettagli = new Dettagli
                    {
                        NomeScarpa = nomeScarpa,
                        Descrizione = descrizione,
                        ImmagineCopertina = immagineCopertina
                    };

                }
            }
            catch (Exception e)
            {
                Response.Write("Error: ");
                Response.Write(e);
            }
            finally
            {
                conn.Close();
            }

            return View(dettagli);
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}