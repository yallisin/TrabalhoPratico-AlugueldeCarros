using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public VeiculoController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Veiculo
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Fabricante)
                .ToListAsync();

            return Ok(veiculos);
        }

        // GET: api/Veiculo/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.Fabricante)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
            {
                return NotFound(new
                {
                    mensagem = "Veículo não encontrado."
                });
            }

            return Ok(veiculo);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(Veiculo veiculo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fabricante = await _context.Fabricantes
                .FindAsync(veiculo.FabricanteId);

            if (fabricante == null)
            {
                return BadRequest(new
                {
                    mensagem = "Fabricante informado não existe."
                });
            }

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = veiculo.Id },
                veiculo
            );
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest(new
                {
                    mensagem = "O ID da URL não corresponde ao ID do veículo."
                });
            }

            var existente = await _context.Veiculos
                .FindAsync(id);

            if (existente == null)
            {
                return NotFound(new
                {
                    mensagem = "Veículo não encontrado."
                });
            }

            var fabricante = await _context.Fabricantes
                .FindAsync(veiculo.FabricanteId);

            if (fabricante == null)
            {
                return BadRequest(new
                {
                    mensagem = "Fabricante informado não existe."
                });
            }

            existente.Modelo = veiculo.Modelo;
            existente.AnoFabricacao = veiculo.AnoFabricacao;
            existente.Quilometragem = veiculo.Quilometragem;
            existente.FabricanteId = veiculo.FabricanteId;

            await _context.SaveChangesAsync();

            return Ok(existente);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var veiculo = await _context.Veiculos
                .FindAsync(id);

            if (veiculo == null)
            {
                return NotFound(new
                {
                    mensagem = "Veículo não encontrado."
                });
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
