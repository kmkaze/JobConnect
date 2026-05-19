using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobConnectAPI.Data;
using JobConnectAPI.DTOs;
using JobConnectAPI.Models;

namespace JobConnectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(dto.Cedula) ||
                string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Apellido) ||
                string.IsNullOrWhiteSpace(dto.Celular) ||
                string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Contrasena))
            {
                return BadRequest(new { mensaje = "Todos los campos son obligatorios." });
            }

            // Validar formato de correo
            if (!dto.Correo.Contains("@") || !dto.Correo.Contains("."))
                return BadRequest(new { mensaje = "El correo no tiene un formato válido." });

            // Verificar duplicados
            bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo);
            if (correoExiste)
                return Conflict(new { mensaje = "Ya existe una cuenta con ese correo." });

            bool cedulaExiste = await _context.Usuarios.AnyAsync(u => u.Cedula == dto.Cedula);
            if (cedulaExiste)
                return Conflict(new { mensaje = "Ya existe una cuenta con esa cédula." });

            // Cifrar contraseña
            string hashContrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

            // Crear usuario
            var usuario = new Usuario
            {
                Cedula = dto.Cedula,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Celular = dto.Celular,
                Correo = dto.Correo,
                Contrasena = hashContrasena
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario registrado exitosamente." });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Correo) || string.IsNullOrWhiteSpace(dto.Contrasena))
                return BadRequest(new { mensaje = "Correo y contraseña son obligatorios." });

            // Buscar usuario por correo
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == dto.Correo);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            // Verificar contraseña
            bool contrasenaValida = BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena);
            if (!contrasenaValida)
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            return Ok(new
            {
                mensaje = "Login exitoso.",
                id = usuario.Id,
                nombre = usuario.Nombre,
                apellido = usuario.Apellido,
                correo = usuario.Correo,
                cedula = usuario.Cedula,
                rol = usuario.Rol
            });
        }

        // Endpoint temporal para generar hash - BORRAR DESPUÉS
        [HttpGet("generarhash/{password}")]
        public IActionResult GenerarHash(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(new { hash });
        }
    }
}