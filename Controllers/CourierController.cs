using Courier.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult DetallesPago()
        {
            return View();
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
