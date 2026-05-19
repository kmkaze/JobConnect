using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobConnectAPI.Data;
using JobConnectAPI.Models;
using JobConnectAPI.DTOs;

namespace JobConnectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfertasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OfertasController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/ofertas - listar todas las ofertas
        [HttpGet]
        public async Task<IActionResult> GetOfertas()
        {
            var ofertas = await _context.Ofertas
                .OrderByDescending(o => o.FechaPublicacion)
                .ToListAsync();
            return Ok(ofertas);
        }

        // POST api/ofertas/aplicar - postularse a una oferta
        [HttpPost("aplicar")]
        public async Task<IActionResult> Aplicar([FromBody] Postulacion postulacion)
        {
            bool yaAplico = await _context.Postulaciones.AnyAsync(p =>
                p.UsuarioId == postulacion.UsuarioId &&
                p.OfertaId == postulacion.OfertaId);

            if (yaAplico)
                return Conflict(new { mensaje = "Ya aplicaste a esta oferta." });

            _context.Postulaciones.Add(postulacion);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "¡Postulación exitosa!" });
        }

        // GET api/ofertas/mispostulaciones/{usuarioId}
        [HttpGet("mispostulaciones/{usuarioId}")]
        public async Task<IActionResult> MisPostulaciones(int usuarioId)
        {
            var ids = await _context.Postulaciones
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => p.OfertaId)
                .ToListAsync();
            return Ok(ids);
        }

        // POST api/ofertas/publicar
        [HttpPost("publicar")]
        public async Task<IActionResult> Publicar([FromBody] CrearOfertaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo) ||
                string.IsNullOrWhiteSpace(dto.Empresa) ||
                string.IsNullOrWhiteSpace(dto.Ubicacion) ||
                string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest(new { mensaje = "Todos los campos son obligatorios." });

            var oferta = new Oferta
            {
                Titulo = dto.Titulo,
                Empresa = dto.Empresa,
                Ubicacion = dto.Ubicacion,
                Salario = dto.Salario,
                Descripcion = dto.Descripcion
            };

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Oferta publicada exitosamente." });
        }

        // GET api/ofertas/estadisticas
        [HttpGet("estadisticas")]
        public async Task<IActionResult> Estadisticas()
        {
            var estadisticas = await _context.Ofertas
                .Select(o => new
                {
                    ofertaId = o.Id,
                    titulo = o.Titulo,
                    empresa = o.Empresa,
                    postulados = _context.Postulaciones.Count(p => p.OfertaId == o.Id)
                })
                .OrderByDescending(o => o.postulados)
                .ToListAsync();

            var totalPostulaciones = await _context.Postulaciones.CountAsync();
            var totalUsuarios = await _context.Usuarios.CountAsync(u => u.Rol == "usuario");

            return Ok(new
            {
                totalPostulaciones,
                totalUsuarios,
                ofertas = estadisticas
            });
        }
    }
}