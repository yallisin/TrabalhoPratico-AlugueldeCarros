using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ClienteController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _context.Clientes
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/Cliente/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _context.Clientes
                .FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            return Ok(cliente);
        }

        // POST: api/Cliente
        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = cliente.Id },
                cliente
            );
        }

        // PUT: api/Cliente/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new
                {
                    mensagem = "O ID da URL não corresponde ao ID do cliente."
                });
            }

            var clienteExistente = await _context.Clientes
                .FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.CPF = cliente.CPF;
            clienteExistente.Email = cliente.Email;

            await _context.SaveChangesAsync();

            return Ok(clienteExistente);
        }

        // DELETE: api/Cliente/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes
                .FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}