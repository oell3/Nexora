using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;

namespace Nexora.services
{
     
    public class UsersServices : Controller
    {




        public IResult Index()
        {
            var connString =
                "Host=127.0.0.1;Username=trix0;Password=Homepage12!;Database=angebotstool";

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                string cmd = "SELECT * FROM crm.projekte";

                using var result = new NpgsqlCommand(cmd, conn);
                using var rdr = result.ExecuteReader();

                var rows = new List<Dictionary<string, object>>();

                while (rdr.Read())
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        row[rdr.GetName(i)] =
                            rdr.IsDBNull(i) ? null : rdr.GetValue(i);
                    }

                    rows.Add(row);
                }

                return Results.Ok(rows);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }


        }

        // GET: UsersServices/Details/5
        public ActionResult Details(int id)
        {
            var connString = "Host=127.0.0.1;Username=trix0;Password=Homepage12!;Database=angebotstool"; // -> noch ändern!

            var conn = new NpgsqlConnection(connString);
            try
            {

                conn.Open();

                string cmd = "Select * From crm.projekte WHERE ID = $1";
                
                var result = new NpgsqlCommand(cmd, conn);
                result.Parameters.AddWithValue(id);
                NpgsqlDataReader rdr = result.ExecuteReader();

                var rows = new List<Dictionary<string, object>>();

                while (rdr.Read())
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        row[rdr.GetName(i)] = rdr.IsDBNull(i) ? null : rdr.GetValue(i);
                    }

                    rows.Add(row);
                }


                conn.Close();
                return Ok(rows);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
            finally
            {
                conn.Close();
            }

            
        }

        // GET: UsersServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersServices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersServices/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersServices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersServices/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersServices/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
