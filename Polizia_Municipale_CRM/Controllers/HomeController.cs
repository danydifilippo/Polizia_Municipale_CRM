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
        public ActionResult Trasgressori()
        {
            List<Gestionale> ListaTrasgressori = Gestionale.GetAllCustomers();

            return View(ListaTrasgressori);
        }

        public ActionResult Create()
        {
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


            }
            catch (Exception ex)
            {

            }
            finally
            {
                sql.Close();
            }
            return RedirectToAction("CreateNotice(t.IDTrasgressore)");
        }

        public ActionResult CreateNotice(int id)
        {
            return View(id);
        }

        public ActionResult Delete(int id)
        {
            Gestionale g = Gestionale.GetElementById(id);
            return View(g);
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
    }
}