using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricanteController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public FabricanteController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Fabricante
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fabricantes = await _context.Fabricantes
                .ToListAsync();

            return Ok(fabricantes);
        }

        // GET: api/Fabricante/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fabricante = await _context.Fabricantes
                .FindAsync(id);

            if (fabricante == null)
            {
                return NotFound(new
                {
                    mensagem = "Fabricante não encontrado."
                });
            }

            return Ok(fabricante);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(Fabricante fabricante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fabricantes.Add(fabricante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = fabricante.Id },
                fabricante
            );
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Fabricante fabricante)
        {
            if (id != fabricante.Id)
            {
                return BadRequest();
            }

            var existente = await _context.Fabricantes
                .FindAsync(id);

            if (existente == null)
            {
                return NotFound();
            }

            existente.Nome = fabricante.Nome;

            await _context.SaveChangesAsync();

            return Ok(existente);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fabricante = await _context.Fabricantes
                .FindAsync(id);

            if (fabricante == null)
            {
                return NotFound();
            }

            _context.Fabricantes.Remove(fabricante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
