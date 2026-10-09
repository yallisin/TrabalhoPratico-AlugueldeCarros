using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PagamentoController(ApplicationContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pagamentos = await _context.Pagamentos
                .Include(p => p.Aluguel)
                .ToListAsync();

            return Ok(pagamentos);
        }

        // GET /api/Pagamento/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pagamento = await _context.Pagamentos
                .Include(p => p.Aluguel)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pagamento == null)
            {
                return NotFound(new
                {
                    mensagem = "Pagamento não encontrado."
                });
            }

            return Ok(pagamento);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(Pagamento pagamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aluguel = await _context.Alugueis
                .FindAsync(pagamento.AluguelId);

            if (aluguel == null)
            {
                return BadRequest(new
                {
                    mensagem = "Aluguel informado não existe."
                });
            }

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = pagamento.Id },
                pagamento
            );
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Pagamento pagamento)
        {
            if (id != pagamento.Id)
            {
                return BadRequest();
            }

            var existente = await _context.Pagamentos
                .FindAsync(id);

            if (existente == null)
            {
                return NotFound();
            }

            existente.Valor = pagamento.Valor;
            existente.DataPagamento = pagamento.DataPagamento;
            existente.FormaPagamento = pagamento.FormaPagamento;
            existente.AluguelId = pagamento.AluguelId;

            await _context.SaveChangesAsync();

            return Ok(existente);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pagamento = await _context.Pagamentos
                .FindAsync(id);

            if (pagamento == null)
            {
                return NotFound();
            }

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}