using Courier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Courier.Controllers
{
    public class AdminCourierController : Controller
    {
        private readonly CourierContext _context;

        public AdminCourierController(CourierContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            TempData["Fecha"] = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            return View();
        }

        public ActionResult Paquetes(string filtro, string buscar)
        {
            ViewBag.Paquetes = paquete(filtro, buscar);
            TempData["Fecha"] = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            return View();
        }

        public List<Paquete> paquete(string filtro, string buscar)
        {
            if (filtro != null)
            {
                List<Paquete> paquete = _context.Paquetes.FromSqlRaw("select * from paquetes where estatus = '" + filtro + "' ").ToList();
                return paquete;
            }
            else if (buscar != null)
            {
                List<Paquete> paquete = _context.Paquetes.FromSqlRaw("select * from paquetes where id_usuario = " + buscar + "").ToList();
                return paquete;
            }
            else
            {
                List<Paquete> paquete = _context.Paquetes.FromSqlRaw("select * from paquetes").ToList();
                return paquete;
            }
        }

        public IActionResult UpdateStatusPaquetes(string id, string newStatus)
        {
            SqlConnection conexion = Conexion.GetConexion();
            conexion.Open();
            string sql = "UPDATE paquetes SET estatus = @estatus WHERE id_paquete = @id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@estatus", newStatus);
            comando.Parameters.AddWithValue("@id", id);
            comando.ExecuteNonQuery();
            conexion.Close();

            TempData["Titulo"] = "Confirmacion";
            TempData["Mensaje"] = "Estatus modificado correctamente";
            TempData["Tipo"] = "success";

            return RedirectToAction("Paquetes", "AdminCourier");
        }

        public IActionResult RegistrarPaquete(string idUsuario, string remitente, string contenido, double peso)
        {
            Paquete paquete = new Paquete();
            paquete.IdUsuario = idUsuario;
            paquete.Remitente = remitente;  
            paquete.Contenido = contenido;
            paquete.Peso = peso;
            paquete.Estatus = "En almacen";
            _context.Paquetes.Add(paquete);
            _context.SaveChanges();

            TempData["Titulo"] = "Confirmacion";
            TempData["Mensaje"] = "Registro realizado correctamente";
            TempData["Tipo"] = "success";

            return RedirectToAction("Paquetes", "AdminCourier");
        }

        public ActionResult Clientes(string estatus, string buscador, string rol)
        {
            ViewBag.Usuarios = usuarios(estatus, buscador, rol);
            TempData["Fecha"] = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            return View();
        }

        public List<Usuario> usuarios(string estatus, string buscador, string rol)
        {
            if (estatus != null)
            {
                List<Usuario> usuario = _context.Usuarios.FromSqlRaw("select * from usuarios where estatus = '" + estatus + "'").ToList();
                return usuario;
            }
            else if (buscador != null)
            {
                List<Usuario> usuario = _context.Usuarios.FromSqlRaw("select * from usuarios where nombre like '%" + buscador + "%'").ToList();
                return usuario;
            }
            else if (rol != null)
            {
                List<Usuario> usuario = _context.Usuarios.FromSqlRaw("select * from usuarios where rol = '" + rol + "'").ToList();
                return usuario;
            }
            else
            {
                List<Usuario> usuario = _context.Usuarios.FromSqlRaw("select * from usuarios").ToList();
                return usuario;
            }
        }

        public IActionResult UpdateRol(string id, string newRol)
        {
            SqlConnection conexion = Conexion.GetConexion();
            conexion.Open();
            string sql = "UPDATE usuarios SET rol = @rol WHERE cedula = @id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@rol", newRol);
            comando.Parameters.AddWithValue("@id", id);
            comando.ExecuteNonQuery();
            conexion.Close();

            TempData["Titulo"] = "Confirmacion";
            TempData["Mensaje"] = "Rol modificado correctamente";
            TempData["Tipo"] = "success";

            return RedirectToAction("Clientes", "AdminCourier");
        }

        public IActionResult UpdateStatus(string id, string newStatus)
        {
            SqlConnection conexion = Conexion.GetConexion();
            conexion.Open();
            string sql = "UPDATE usuarios SET estatus = @estatus WHERE cedula = @id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@estatus", newStatus);
            comando.Parameters.AddWithValue("@id", id);
            comando.ExecuteNonQuery();
            conexion.Close();

            TempData["Titulo"] = "Confirmacion";
            TempData["Mensaje"] = "Estatus modificado correctamente";
            TempData["Tipo"] = "success";

            return RedirectToAction("Clientes", "AdminCourier");
        }

        public IActionResult RegistrarUsuario(int celular, int telefono, string nombre,string apellido,string sexo,string correo, string cedula, string password, string confirmPassword)
        {
            Usuario usuario = new Usuario();

            if (password == confirmPassword)
            {
                usuario.Correo = correo;
                usuario.Sexo = sexo;
                usuario.Apellido = apellido;
                usuario.Nombre = nombre;
                usuario.Telefono = telefono;
                usuario.Cedula = cedula;
                usuario.Celular = celular;
                usuario.Password = Encryptar.GetSHA256(password);
                usuario.Estatus = "A";
                usuario.Rol = "cliente";
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                TempData["Titulo"] = "Confirmacion";
                TempData["Mensaje"] = "Registro realizado correctamente";
                TempData["Tipo"] = "success";
            }
            else
            {
                TempData["Titulo"] = "Ha ocurrido un error";
                TempData["Mensaje"] = "Verifique las password";
                TempData["Tipo"] = "error";
            }

            return RedirectToAction("Clientes", "AdminCourier");
        }

        public class Encryptar
        {
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
}
