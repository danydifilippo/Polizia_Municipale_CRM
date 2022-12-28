using Microsoft.SqlServer.Server;
using Polizia_Municipale_CRM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace Polizia_Municipale_CRM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Trasgressori()
        {
            List<Gestionale> ListaTrasgressori = Gestionale.GetAllCustomers();

            return View(ListaTrasgressori);
        }

        public ActionResult Verbali()
        {
            List<Gestionale> ListaVerbali = Gestionale.GetAllData();

            return View(ListaVerbali);
        }

        public ActionResult Ins_Verbale()
        {
            ViewBag.listaCodici = Gestionale.ListaViolazioni;
            ViewBag.listaUtenti = Gestionale.ListaUtenti;

            return View();
        }

        [HttpPost]
        public ActionResult Ins_Verbale(Gestionale t)
        {

            SqlConnection sql = Shared.GetConnection();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetStoreProcedure("CreateNotice", sql);

                com.Parameters.AddWithValue("IDTrasgressore", t.IDTrasgressore);
                com.Parameters.AddWithValue("IDViolazione", t.IDViolazione);
                com.Parameters.AddWithValue("DataViolazione", t.DataViolazione.ToShortDateString());
                com.Parameters.AddWithValue("DataVerbale", t.DataVerbale.ToShortDateString());
                com.Parameters.AddWithValue("Agente", t.Agente);
                com.Parameters.AddWithValue("Punti", t.Punti);
                com.Parameters.AddWithValue("IndirizzoViolazione", t.Ind_Violazione);
                int row = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return RedirectToAction("Verbali");
        }

        public ActionResult Create()
        {
            ViewBag.listaCodici = Gestionale.ListaViolazioni;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Gestionale t)
        {
            SqlConnection sql = Shared.GetConnection();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetStoreProcedure("CreateNewCustomer", sql);

                com.Parameters.AddWithValue("Cognome", t.Cognome);
                com.Parameters.AddWithValue("Nome", t.Nome);
                com.Parameters.AddWithValue("Indirizzo", t.Indirizzo);
                com.Parameters.AddWithValue("Citta", t.Citta);
                com.Parameters.AddWithValue("CAP", t.CAP);
                com.Parameters.AddWithValue("CF", t.CF);
                int row = com.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                
            }
            sql.Close();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("Select IDTrasgressore from Trasgressore where CodiceFiscale = @CF", sql);
                com.Parameters.AddWithValue("CF", t.CF);
                
                SqlDataReader reader= com.ExecuteReader();
                while(reader.Read())
                {
                    t.IDTrasgressore = Convert.ToInt32(reader["IDTrasgressore"]);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetStoreProcedure("CreateNotice", sql);

                com.Parameters.AddWithValue("IDTrasgressore", t.IDTrasgressore);
                com.Parameters.AddWithValue("IDViolazione", t.IDViolazione);
                com.Parameters.AddWithValue("DataViolazione", t.DataViolazione.ToShortDateString());
                com.Parameters.AddWithValue("DataVerbale", t.DataVerbale.ToShortDateString());
                com.Parameters.AddWithValue("Agente", t.Agente);
                com.Parameters.AddWithValue("Punti", t.Punti);
                com.Parameters.AddWithValue("IndirizzoViolazione", t.Ind_Violazione);
                int row = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return RedirectToAction("Trasgressori");
        }

            public ActionResult Edit(int id)
        {
            Gestionale g = Gestionale.GetElementById(id);
            ViewBag.listaCodici = Gestionale.ListaViolazioni;
            return View(g);
        }

        [HttpPost]
        public ActionResult Edit(Gestionale g)
        {

            SqlConnection sql = Shared.GetConnection();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetStoreProcedure("UpdateAllData", sql);
                com.Parameters.AddWithValue("IDVerbale", g.IDVerbale);
                com.Parameters.AddWithValue("IDTrasgressore", g.IDTrasgressore);
                com.Parameters.AddWithValue("IDViolazione", g.IDViolazione);
                com.Parameters.AddWithValue("Cognome", g.Cognome);
                com.Parameters.AddWithValue("Nome", g.Nome);
                com.Parameters.AddWithValue("Indirizzo", g.Indirizzo);
                com.Parameters.AddWithValue("Citta", g.Citta);
                com.Parameters.AddWithValue("CAP", g.CAP);
                com.Parameters.AddWithValue("CF", g.CF);
                com.Parameters.AddWithValue("DataViolazione", g.DataViolazione);
                com.Parameters.AddWithValue("DataVerbale",g.DataVerbale);
                com.Parameters.AddWithValue("Agente", g.Agente);
                com.Parameters.AddWithValue("Punti", g.Punti);
                com.Parameters.AddWithValue("IndirizzoViolazione", g.Ind_Violazione);

                int row = com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return RedirectToAction("Trasgressori");
        }

        public ActionResult DeleteCustomer(int id)
        {
            Gestionale g = Gestionale.GetElementById(id);

            SqlConnection sql = Shared.GetConnection();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetStoreProcedure("DeleteNotice", sql);
                com.Parameters.AddWithValue("IdVerbale", g.IDVerbale);
                int row = com.ExecuteNonQuery();

                SqlCommand command = Shared.GetStoreProcedure("DeleteCustomer", sql);
                command.Parameters.AddWithValue("IDTrasgressore", g.IDTrasgressore);
                int row2 = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return RedirectToAction("Trasgressori");
        }

        public ActionResult RegistroVerbali()
        {
            SqlConnection sql = Shared.GetConnection();

            Gestionale v = new Gestionale();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT COUNT(*) AS TotVerbali FROM VERBALE", sql);

                SqlDataReader reader = com.ExecuteReader();

               
                while (reader.Read())
                {
                    v.Tot_Verbali = Convert.ToInt32(reader["TotVerbali"]);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return View(v);
        }

        public ActionResult PartialViewTrasgressore()
        {
            SqlConnection sql = Shared.GetConnection();

            List<Gestionale> VerbPerTrasg = new List<Gestionale>();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT COUNT(*) AS Nr_Verbali, Cognome, Nome FROM VERBALE INNER JOIN TRASGRESSORE" +
                    " ON VERBALE.IDTrasgressore = TRASGRESSORE.IDTrasgressore GROUP BY Cognome, Nome ORDER BY Cognome", sql);
                
                SqlDataReader reader = com.ExecuteReader();

                while(reader.Read())
                {
                    Gestionale g = new Gestionale
                    {
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Nr_Verbali = Convert.ToInt32(reader["Nr_Verbali"]),
                    };
                    VerbPerTrasg.Add(g);
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return PartialView("_PartialViewTrasgressore", VerbPerTrasg);
        }

        public ActionResult PartialViewPunti()
        {
            SqlConnection sql = Shared.GetConnection();

            List<Gestionale> VerbPerPuntiTrasg = new List<Gestionale>();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT SUM(DecurtamentoPunti) AS TotPuntiDecurtati, Cognome, Nome FROM VERBALE INNER JOIN TRASGRESSORE" +
                    " ON VERBALE.IDTrasgressore = TRASGRESSORE.IDTrasgressore GROUP BY Cognome, Nome ORDER BY Cognome", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Gestionale g = new Gestionale
                    {
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Punti = Convert.ToInt32(reader["TotPuntiDecurtati"]),
                    };
                    VerbPerPuntiTrasg.Add(g);
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return PartialView("_PartialViewPunti", VerbPerPuntiTrasg);
        }

        public ActionResult PartialViewPunti10()
        {
            SqlConnection sql = Shared.GetConnection();

            List<Gestionale> VerbPerPunti = new List<Gestionale>();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT Cognome, Nome, DataViolazione, Importo, DecurtamentoPunti FROM VERBALE INNER JOIN TRASGRESSORE " +
                    "ON VERBALE.IDTrasgressore = TRASGRESSORE.IDTrasgressore INNER JOIN VIOLAZIONE ON VERBALE.IDViolazione = VIOLAZIONE.IDViolazione WHERE DecurtamentoPunti > 10", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Gestionale g = new Gestionale
                    {
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                        ImportoCollapse = Convert.ToDecimal(reader["Importo"]),
                        Punti = Convert.ToInt32(reader["DecurtamentoPunti"]),
                    };
                    VerbPerPunti.Add(g);
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return PartialView("_PartialViewPunti10", VerbPerPunti);
        }

        public ActionResult PartialViewImporto()
        {
            SqlConnection sql = Shared.GetConnection();

            List<Gestionale> VerbPerImp = new List<Gestionale>();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT Cognome, Nome, DataViolazione, Importo, DecurtamentoPunti FROM VERBALE INNER JOIN TRASGRESSORE " +
                    "ON VERBALE.IDTrasgressore = TRASGRESSORE.IDTrasgressore INNER JOIN VIOLAZIONE ON VERBALE.IDViolazione = VIOLAZIONE.IDViolazione WHERE Importo > 400", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Gestionale g = new Gestionale
                    {
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                        ImportoCollapse = Convert.ToDecimal(reader["Importo"]),
                        Punti = Convert.ToInt32(reader["DecurtamentoPunti"]),
                    };
                    VerbPerImp.Add(g);
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }

            return PartialView("_PartialViewImporto", VerbPerImp);
        }
    }
}