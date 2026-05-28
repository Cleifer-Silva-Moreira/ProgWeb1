using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;

        public AccountController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim().ToLowerInvariant();

            var usuario = await _usuarioRepository
                .ObterPorEmailAsync(email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Senha, usuario.SenhaHash))
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha invalidos.");
                return View(model);
            }

            HttpContext.Session.SetString("Usuario", usuario.Email);

            return RedirectToAction("Index", "Aluno");
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim().ToLowerInvariant();

            var usuarioJaExiste = await _usuarioRepository
                .EmailExisteAsync(email);

            if (usuarioJaExiste)
            {
                ModelState.AddModelError(nameof(model.Email), "Este e-mail ja esta cadastrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Email = email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(model.Senha)
            };

            await _usuarioRepository.AdicionarAsync(usuario);

            HttpContext.Session.SetString("Usuario", usuario.Email);

            return RedirectToAction("Index", "Aluno");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
