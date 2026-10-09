using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AluguelController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AluguelController(ApplicationContext context)
        {
            _context = context;
        }


        // GET: api/Aluguel
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alugueis = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                    .ThenInclude(v => v.Fabricante)
                .Include(a => a.Pagamentos)
                .ToListAsync();

            return Ok(alugueis);
        }

        // GET: api/Aluguel/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aluguel = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                    .ThenInclude(v => v.Fabricante)
                .Include(a => a.Pagamentos)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluguel == null)
            {
                return NotFound(new
                {
                    mensagem = "Aluguel não encontrado."
                });
            }

            return Ok(aluguel);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(Aluguel aluguel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = await _context.Clientes
                .FindAsync(aluguel.ClienteId);

            if (cliente == null)
            {
                return BadRequest(new
                {
                    mensagem = "Cliente informado não existe."
                });
            }

            var veiculo = await _context.Veiculos
                .FindAsync(aluguel.VeiculoId);

            if (veiculo == null)
            {
                return BadRequest(new
                {
                    mensagem = "Veículo informado não existe."
                });
            }

            if (aluguel.DataFim < aluguel.DataInicio)
            {
                return BadRequest(new
                {
                    mensagem = "A data final não pode ser anterior à data inicial."
                });
            }

            if (aluguel.QuilometragemFinal <
                aluguel.QuilometragemInicial)
            {
                return BadRequest(new
                {
                    mensagem = "A quilometragem final não pode ser menor que a inicial."
                });
            }

            _context.Alugueis.Add(aluguel);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = aluguel.Id },
                aluguel
            );
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Aluguel aluguel)
        {
            if (id != aluguel.Id)
            {
                return BadRequest(new
                {
                    mensagem = "O ID da URL não corresponde ao ID do aluguel."
                });
            }

            var existente = await _context.Alugueis
                .FindAsync(id);

            if (existente == null)
            {
                return NotFound(new
                {
                    mensagem = "Aluguel não encontrado."
                });
            }

            existente.ClienteId = aluguel.ClienteId;
            existente.VeiculoId = aluguel.VeiculoId;
            existente.DataInicio = aluguel.DataInicio;
            existente.DataFim = aluguel.DataFim;
            existente.QuilometragemInicial =
                aluguel.QuilometragemInicial;
            existente.QuilometragemFinal =
                aluguel.QuilometragemFinal;
            existente.ValorDiaria = aluguel.ValorDiaria;
            existente.ValorTotal = aluguel.ValorTotal;

            await _context.SaveChangesAsync();

            return Ok(existente);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aluguel = await _context.Alugueis
                .FindAsync(id);

            if (aluguel == null)
            {
                return NotFound(new
                {
                    mensagem = "Aluguel não encontrado."
                });
            }

            _context.Alugueis.Remove(aluguel);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // ==========================================
        // 5 FILTERS
        // ==========================================

        // FILTER 1
        // GET: api/Aluguel/filtro/cliente?nome=Joao
        //
        // JOIN Aluguel -> Cliente
        //
        [HttpGet("filtro/cliente")]
        public async Task<IActionResult> FiltrarPorCliente(
            string nome)
        {
            var resultado = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .Where(a => a.Cliente.Nome.Contains(nome))
                .Select(a => new
                {
                    AluguelId = a.Id,
                    Cliente = a.Cliente.Nome,
                    Veiculo = a.Veiculo.Modelo,
                    a.DataInicio,
                    a.DataFim,
                    a.ValorTotal
                })
                .ToListAsync();

            return Ok(resultado);
        }


        // FILTER 2
        // GET: api/Aluguel/filtro/fabricante?nome=Toyota
        //
        // JOIN Aluguel -> Veiculo -> Fabricante
        //
        [HttpGet("filtro/fabricante")]
        public async Task<IActionResult> FiltrarPorFabricante(
            string nome)
        {
            var resultado = await _context.Alugueis
                .Include(a => a.Veiculo)
                    .ThenInclude(v => v.Fabricante)
                .Include(a => a.Cliente)
                .Where(a =>
                    a.Veiculo.Fabricante.Nome.Contains(nome))
                .Select(a => new
                {
                    AluguelId = a.Id,
                    Cliente = a.Cliente.Nome,
                    Fabricante = a.Veiculo.Fabricante.Nome,
                    Modelo = a.Veiculo.Modelo,
                    a.DataInicio,
                    a.DataFim
                })
                .ToListAsync();

            return Ok(resultado);
        }


        // FILTER 3
        // GET: api/Aluguel/filtro/modelo?modelo=Civic
        //
        // JOIN Aluguel -> Veiculo
        //
        [HttpGet("filtro/modelo")]
        public async Task<IActionResult> FiltrarPorModelo(
            string modelo)
        {
            var resultado = await _context.Alugueis
                .Include(a => a.Veiculo)
                .Include(a => a.Cliente)
                .Where(a =>
                    a.Veiculo.Modelo.Contains(modelo))
                .Select(a => new
                {
                    AluguelId = a.Id,
                    Cliente = a.Cliente.Nome,
                    Modelo = a.Veiculo.Modelo,
                    a.DataInicio,
                    a.DataFim,
                    a.ValorTotal
                })
                .ToListAsync();

            return Ok(resultado);
        }


        // FILTER 4
        // GET: api/Aluguel/filtro/pagamento?forma=PIX
        //
        // JOIN Aluguel -> Pagamento
        //
        [HttpGet("filtro/pagamento")]
        public async Task<IActionResult> FiltrarPorPagamento(
            string forma)
        {
            var resultado = await _context.Alugueis
                .Include(a => a.Pagamentos)
                .Include(a => a.Cliente)
                .Where(a =>
                    a.Pagamentos.Any(
                        p => p.FormaPagamento.Contains(forma)))
                .Select(a => new
                {
                    AluguelId = a.Id,
                    Cliente = a.Cliente.Nome,
                    FormaPagamento =
                        a.Pagamentos
                            .Select(p => p.FormaPagamento)
                            .FirstOrDefault(),
                    a.ValorTotal
                })
                .ToListAsync();

            return Ok(resultado);
        }


        // FILTER 5
        // GET: api/Aluguel/filtro/periodo?inicio=2026-01-01&fim=2026-12-31
        //
        // Uses Cliente + Veiculo relationships as well
        //
        [HttpGet("filtro/periodo")]
        public async Task<IActionResult> FiltrarPorPeriodo(
            DateTime inicio,
            DateTime fim)
        {
            var resultado = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                    .ThenInclude(v => v.Fabricante)
                .Where(a =>
                    a.DataInicio >= inicio &&
                    a.DataFim <= fim)
                .Select(a => new
                {
                    AluguelId = a.Id,
                    Cliente = a.Cliente.Nome,
                    Fabricante = a.Veiculo.Fabricante.Nome,
                    Veiculo = a.Veiculo.Modelo,
                    a.DataInicio,
                    a.DataFim,
                    a.ValorTotal
                })
                .ToListAsync();

            return Ok(resultado);
        }
    }
}