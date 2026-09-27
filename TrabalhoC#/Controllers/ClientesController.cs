using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Data;
using TrabalhoC_.Models;

namespace TrabalhoC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ClientesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Listar()
        {
            return await _context.Clientes.OrderBy(c => c.Nome).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Buscar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Criar(Cliente cliente)
        {
            cliente.Nome = cliente.Nome.Trim();
            cliente.CPF = cliente.CPF.Trim();
            cliente.Email = cliente.Email.Trim();

            var cpfJaExiste = await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF);

            if (cpfJaExiste)
            {
                return BadRequest("Já existe um cliente com esse CPF.");
            }

            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Buscar), new { id = cliente.Id }, cliente);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao cadastrar o cliente.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("O ID informado é diferente do ID do cliente.");
            }

            cliente.Nome = cliente.Nome.Trim();
            cliente.CPF = cliente.CPF.Trim();
            cliente.Email = cliente.Email.Trim();

            var clienteBanco = await _context.Clientes.FindAsync(id);

            if (clienteBanco == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var cpfJaExiste = await _context.Clientes.AnyAsync(c =>
                c.CPF == cliente.CPF && c.Id != id);

            if (cpfJaExiste)
            {
                return BadRequest("Já existe um cliente com esse CPF.");
            }

            clienteBanco.Nome = cliente.Nome;
            clienteBanco.CPF = cliente.CPF;
            clienteBanco.Email = cliente.Email;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao atualizar o cliente.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var possuiAluguel = await _context.Alugueis.AnyAsync(a => a.ClienteId == id);

            if (possuiAluguel)
            {
                return BadRequest("O cliente possui aluguéis cadastrados.");
            }

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir o cliente.");
            }
        }
    }
}
