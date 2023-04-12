using Courier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Courier.Controllers
{
    public class CourierController : Controller
    {
        private readonly CourierContext _context;
        public CourierController(CourierContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Paquetes = paquetes();
            return View();
        }
        public IActionResult Configuracion()
        {
            var user = usuarios();
            return View(user);
        }

        public IActionResult Sucursales()
        {
            return View();
        }

        public IActionResult DetallesPago(string idPaquete)
        {
            Factura f = _context.Facturas.FromSqlRaw("select * from factura where id_paquete = '" + idPaquete + "' ").FirstOrDefault();
            if (f == null)
            {
                TempData["Titulo"] = "Ha Ocurrido un error";
                TempData["Mensaje"] = "Todavia no se ha generado la factura de su paquete";
                TempData["Tipo"] = "error";

                return RedirectToAction("Index", "Courier");
            }
            else
            {
                ViewBag.Factura = misfacturas(idPaquete);
            } 
            
            return View();
        }

        public List<DetalleFactura> misfacturas(string idPaquete)
        {
                SqlConnection conexion = Conexion.GetConexion();
                conexion.Open();
                string sql = "select f.id, u.nombre, u.apellido, u.cedula, f.fechaGeneracion, f.id_paquete, p.contenido, p.peso, f.total from factura f, usuarios u, paquetes p where f.id_paquete = p.id_paquete and f.id_usuario = u.cedula and p.id_paquete = '" + idPaquete + "'";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataReader reader = comando.ExecuteReader();

                List<DetalleFactura> factura = new List<DetalleFactura>();

                while (reader.Read())
                {
                    DetalleFactura t = new DetalleFactura();
                    t.Id = reader.GetInt32(0);
                    t.Nombre = reader.GetString(1);
                    t.Apellido = reader.GetString(2);
                    t.Cedula = reader.GetString(3);
                    t.FechaGeneracion = reader.GetDateTime(4);
                    t.IdPaquete = reader.GetInt32(5);
                    t.Contenido = reader.GetString(6);
                    t.Peso = reader.GetDouble(7);
                    t.Total = reader.GetInt32(8);
                    factura.Add(t);
                }

                conexion.Close();
                return factura;
        }
   

        public List<Paquete> paquetes()
        {
            List<Paquete> paquetes = _context.Paquetes.FromSqlRaw("select * from paquetes where id_usuario = "+ User.Identity.Name + "").ToList();
            return paquetes;
        }
        public Usuario usuarios()
        {
            Usuario usuario = _context.Usuarios.FromSqlRaw("select * from usuarios where cedula = " + User.Identity.Name + "").FirstOrDefault();
            return usuario;
        }

        public async Task<IActionResult> Editar(Usuario cliente, string pass)
        {
            if (pass == null)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                var encryptedPass = GetSHA256(pass);
                cliente.Password = encryptedPass;
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }

            TempData["Titulo"] = "Confirmacion";
            TempData["Mensaje"] = "operacion realizada correctamente";
            TempData["Tipo"] = "success";

            return RedirectToAction("Index", "Courier");
        }
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
