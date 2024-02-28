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

            string conection = ConfigurationManager.ConnectionStrings["ScarpeDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(conection);
            List<ImmagineArticolo> listaImmagini = new List<ImmagineArticolo>();
            Scarpa scarpa = null;
            try
            {
                conn.Open();
                string query = "SELECT PercorsoImmagine FROM ImmaginiArticoli WHERE FK_IdScarpa = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listaImmagini.Add(new ImmagineArticolo { PercorsoImmagine = reader["PercorsoImmagine"].ToString() });
                }
                reader.Close();
                string query2 = "SELECT * FROM Articoli WHERE idScarpa = @id";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@id", id);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {
                    scarpa = new Scarpa(
                       reader2["NomeScarpa"].ToString(),
                       reader2["Descrizione"].ToString(),
                       reader2["ImmagineCopertina"].ToString()
                       );
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex);
            }
            finally
            {
                conn.Close();
            }

            if (scarpa == null)
            {
                return RedirectToAction("Index");
            }

            DettagliModelView model = new DettagliModelView
            {
                Scarpa = scarpa,
                ListaImmagini = listaImmagini
            };

            return View(model);

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