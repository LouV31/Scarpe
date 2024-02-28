using Scarpe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Scarpe.Controllers
{
    public class UtenteController : Controller
    {
        // GET: Utente
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utente utente)
        {
            string connection = ConfigurationManager
                .ConnectionStrings["ScarpeDB"]
                .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                conn.Open();
                string query = "Select * from Utenti WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", utente.Email);
                cmd.Parameters.AddWithValue("@Password", utente.Password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Utente utenteAutenticato = new Utente(
                        Convert.ToInt32(reader["idUtente"]),
                        reader["Email"].ToString(),
                        reader["Password"].ToString(),
                        Convert.ToBoolean(reader["isAdmin"])
                    );
                    Session["Utente"] = utenteAutenticato;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Email o Password errati";
                    return View("Login");
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

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Admin()
        {
            if (Session["Utente"] != null)
            {
                Utente utente = (Utente)Session["Utente"];
                if (utente.isAdmin)
                {
                    string connection = ConfigurationManager
                        .ConnectionStrings["ScarpeDB"]
                        .ConnectionString.ToString();
                    SqlConnection conn = new SqlConnection(connection);
                    List<Scarpa> listaScarpe = new List<Scarpa>();
                    try
                    {
                        conn.Open();
                        string query = "SELECT * FROM Articoli";
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
                                Convert.ToBoolean(reader["Attivo"])
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
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Utente");
            }
        }



        public ActionResult AggiungiScarpa()
        {
            if (Session["Utente"] != null)
            {
                Utente utente = (Utente)Session["Utente"];
                if (utente.isAdmin)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Utente");
            }
        }

        [HttpPost]
        public ActionResult AggiungiScarpa(Scarpa scarpa)
        {
            if (Session["Utente"] == null)
            {
                return RedirectToAction("Login", "Utente");
            }
            if (scarpa.FileImmagine != null && scarpa.FileImmagine.ContentLength > 0)
            {
                string fileName = System.IO.Path.GetFileName(scarpa.FileImmagine.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                scarpa.FileImmagine.SaveAs(path);
                scarpa.ImmagineCopertina = "/Content/img/" + fileName;

            }
            string connection = ConfigurationManager
                .ConnectionStrings["ScarpeDB"]
                .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                conn.Open();
                string query = "INSERT INTO Articoli (NomeScarpa, Descrizione, ImmagineCopertina, Prezzo, Attivo) VALUES (@NomeScarpa, @Descrizione, @ImmagineCopertina, @Prezzo, @Attivo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NomeScarpa", scarpa.NomeScarpa);
                cmd.Parameters.AddWithValue("@Descrizione", scarpa.Descrizione);
                cmd.Parameters.AddWithValue("@ImmagineCopertina", scarpa.ImmagineCopertina);
                cmd.Parameters.AddWithValue("@Prezzo", Convert.ToDouble(scarpa.Prezzo));
                cmd.Parameters.AddWithValue("@Attivo", scarpa.Attivo);
                cmd.ExecuteNonQuery();

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

            return RedirectToAction("Admin");
        }



        public ActionResult ModificaScarpa(int idScarpa)
        {
            if (Session["Utente"] != null)
            {
                Utente utente = (Utente)Session["Utente"];
                if (utente.isAdmin)
                {
                    string connection = ConfigurationManager
                        .ConnectionStrings["ScarpeDB"]
                        .ConnectionString.ToString();
                    SqlConnection conn = new SqlConnection(connection);
                    Scarpa scarpa = new Scarpa();
                    try
                    {
                        conn.Open();
                        string query = "SELECT * FROM Articoli WHERE idScarpa = @idScarpa";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idScarpa", idScarpa);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            scarpa = new Scarpa(
                                Convert.ToInt32(reader["idScarpa"]),
                                reader["NomeScarpa"].ToString(),
                                reader["Descrizione"].ToString(),
                                reader["ImmagineCopertina"].ToString(),
                                Convert.ToDecimal(reader["Prezzo"]),
                                Convert.ToBoolean(reader["Attivo"])
                            );
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
                    return View(scarpa);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Utente");
            }
        }
    }
}
