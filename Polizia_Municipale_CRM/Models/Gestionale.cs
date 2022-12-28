using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polizia_Municipale_CRM.Models
{
    public class Gestionale
    {
        [Display(Name ="ID")]
        public int IDTrasgressore { get; set; }

        [Display(Name = "Verbale Nr.")]
        public int IDVerbale { get; set; }

        [Display(Name = "Cod. Violazione")]
        public int IDViolazione { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }

        [Display(Name = "Ind. Violazione")]
        public string Ind_Violazione { get; set; }
        public string Citta { get; set; }
        public string CAP { get; set; }

        [Display(Name = "Codice Fiscale")]
        public string CF { get; set; }
        public string Agente { get; set; }
        public string Descrizione { get; set; }

        [Display(Name = "Inserire importo come da codice violazione riportata sopra. Se diverso inserire nuovo importo")]
        public decimal Importo { get; set; }

        [Display(Name = "Punti Decurtati")]
        public int Punti { get; set; }

        [Display(Name = "Data Violazione")]
        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime DataViolazione { get; set; }

        [Display(Name = "Data Verbale")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DataVerbale { get; set; }

        public int Nr_Verbali { get; set; }

        [Display(Name = "Importo")]
        public decimal ImportoCollapse { get; set; }
        
        public int Tot_Verbali { get; set; }


        public static List<Gestionale> GetAllData()
        {
            List<Gestionale> lista = new List<Gestionale>();

            SqlConnection sql = Shared.GetConnection();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT * FROM VERBALE AS V " +
                                                    "INNER JOIN TRASGRESSORE AS T ON " +
                                                    "V.IDTrasgressore = T.IDTrasgressore " +
                                                    "INNER JOIN VIOLAZIONE AS VL ON " +
                                                    "V.IDViolazione = VL.IDViolazione " +
                                                    "ORDER BY T.IDTrasgressore", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Gestionale g = new Gestionale()
                    {
                        IDTrasgressore = Convert.ToInt32(reader["IDTrasgressore"]),
                        IDVerbale = Convert.ToInt32(reader["IDVerbale"]),
                        IDViolazione = Convert.ToInt32(reader["IDViolazione"]),
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Indirizzo = reader["Indirizzo"].ToString(),
                        Ind_Violazione = reader["IndirizzoViolazione"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        CAP = reader["CAP"].ToString(),
                        CF = reader["CodiceFiscale"].ToString(),
                        Descrizione = reader["Descrizione"].ToString(),
                        Importo = Convert.ToDecimal(reader["Importo"]),
                        Punti = Convert.ToInt32(reader["DecurtamentoPunti"]),
                        DataVerbale = Convert.ToDateTime(reader["DataVerbale"]),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                        Agente = reader["Agente"].ToString()
                    };
                    lista.Add(g);
                }
            }
            catch (Exception ex)
            {
               
            }
                sql.Close();

            return lista;
        }

        public static List<Gestionale> GetAllCustomers()
        {
            List<Gestionale> lista = new List<Gestionale>();

            SqlConnection sql = Shared.GetConnection();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetCommand("SELECT * FROM TRASGRESSORE", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Gestionale g = new Gestionale()
                    {
                        IDTrasgressore = Convert.ToInt32(reader["IDTrasgressore"]),
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Indirizzo = reader["Indirizzo"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        CAP = reader["CAP"].ToString(),
                        CF = reader["CodiceFiscale"].ToString()
                    };
                    lista.Add(g);
                }
                
            }
            catch (Exception ex)
            {

            }
                sql.Close();

            return lista;
        }

        public static Gestionale GetElementById(int id)
        {
            SqlConnection sql = Shared.GetConnection();
            Gestionale g = new Gestionale();

            try
            {
                sql.Open();

                SqlCommand com = Shared.GetStoreProcedure("GetCustomByID", sql);

                com.Parameters.AddWithValue("@IdTrasgressore", id);

                SqlDataReader reader = com.ExecuteReader();


                while (reader.Read())
                {
                    g.IDTrasgressore = Convert.ToInt32(reader["IDTrasgressore"]);
                    g.Cognome = reader["Cognome"].ToString();
                    g.Nome = reader["Nome"].ToString();
                    g.Indirizzo = reader["Indirizzo"].ToString();
                    g.Citta = reader["Citta"].ToString();
                    g.CAP = reader["CAP"].ToString();
                    g.CF = reader["CodiceFiscale"].ToString();
                    g.IDVerbale = Convert.ToInt32(reader["IDVerbale"]);
                    g.IDViolazione = Convert.ToInt32(reader["IDViolazione"]);
                    g.Ind_Violazione = reader["IndirizzoViolazione"].ToString();
                    g.Descrizione = reader["Descrizione"].ToString();
                    g.Importo = Convert.ToDecimal(reader["Importo"]);
                    g.Punti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                    g.DataVerbale = Convert.ToDateTime(reader["DataVerbale"]);
                    g.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                    g.Agente = reader["Agente"].ToString();

                }

            }
            catch (Exception ex)
            {
                
            }
            sql.Close();
            return g;
        }

        public static List<SelectListItem> ListaViolazioni
        {
            get
            {
                List<SelectListItem> selectViolation = new List<SelectListItem>();
                SqlConnection sql = Shared.GetConnection();
                sql.Open();
                SqlCommand com = Shared.GetCommand("SELECT * FROM VIOLAZIONE", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    SelectListItem l = new SelectListItem
                    {
                        Text = reader["Descrizione"].ToString() + " " + Convert.ToDecimal(reader["Importo"]) + "€",
                        Value = reader["IDViolazione"].ToString(),
                    };
                    selectViolation.Add(l);
                }
                sql.Close();
                return selectViolation;
            }
        }
        public static List<SelectListItem> ListaUtenti
        {
            get
            {
                List<SelectListItem> selectUser = new List<SelectListItem>();
                SqlConnection sql = Shared.GetConnection();
                sql.Open();
                SqlCommand com = Shared.GetCommand("SELECT IDTrasgressore, Cognome, Nome FROM TRASGRESSORE", sql);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    SelectListItem l = new SelectListItem
                    {
                        Text = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(),
                        Value = reader["IDTrasgressore"].ToString(),
                    };
                    selectUser.Add(l);
                }
                sql.Close();
                return selectUser;
            }
        }
    }
}