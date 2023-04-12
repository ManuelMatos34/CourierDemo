using Courier.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;

namespace Courier.Controllers
{
    public class LoginController : Controller
    {
        private readonly CourierContext _context;

        public LoginController(CourierContext context)
        {
            _context = context;
        }

        // login
        public IActionResult Index()
        {
            return View();
        }

        // registro
        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult RegistrarUsuario(Usuario usuario,string confirmPassword)
        {
            if(usuario.Password == confirmPassword)
            {
                usuario.Password = Encryptar.GetSHA256(usuario.Password);
                usuario.Estatus = "A";
                usuario.Rol = "cliente";
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(usuario.Correo, ""));
                mail.From = new MailAddress("admisionesunphu@hotmail.com");
                mail.Subject = "Courier Registro";
                mail.Body = "Usted se ha registrado correctamente";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("admisionesunphu@hotmail.com", "1234HOLA");
                smtp.Send(mail);

                TempData["Titulo"] = "Confirmacion";
                TempData["Mensaje"] = "Registro realizado correctamente, revise su correo";
                TempData["Tipo"] = "success";
            }
            else
            {
                TempData["Titulo"] = "Ha ocurrido un error";
                TempData["Mensaje"] = "Verifique las password";
                TempData["Tipo"] = "error";
            }

            return RedirectToAction("Index2", "Login");
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

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> datos = _context.Usuarios.FromSqlRaw("Select * from usuarios").ToList();
            return datos;
        }

        public Usuario ValidarUsuario(string matricula, string password)
        {
            var pswordEncrypted = Encryptar.GetSHA256(password);
            return ObtenerUsuarios().Where(x => x.Cedula == matricula && x.Password == pswordEncrypted).FirstOrDefault();
        }

        public async Task<IActionResult> ValidarInicio(Usuario _usuario)
        {
            var usuario = ValidarUsuario(_usuario.Cedula, _usuario.Password);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Cedula)
                };

                string[] usuarioRol = { usuario.Rol };

                foreach (string rol in usuarioRol)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                switch (usuario.Rol)
                {
                    case "administrador":
                        var claimsIdentityAdmin = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentityAdmin));
                        return RedirectToAction("Index", "AdminCourier");


                    case "cliente":
                        var claimsIdentityEstudiante = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentityEstudiante));
                        return RedirectToAction("Index", "Courier");
                }

                TempData["Titulo"] = "Ha ocurrido un error";
                TempData["Mensaje"] = "El Administrador aun no te ha asignado un rol";
                TempData["Tipo"] = "error";

                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["Titulo"] = "Ha ocurrido un error";
                TempData["Mensaje"] = "Asegurate de que los datos introducidos sean correctos";
                TempData["Tipo"] = "error";

                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}

