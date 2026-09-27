using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Data;
using TrabalhoC_.Models;

namespace TrabalhoC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricantesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public FabricantesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricante>>> Listar()
        {
            return await _context.Fabricantes.OrderBy(f => f.Nome).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fabricante>> Buscar(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);

            if (fabricante == null)
            {
                return NotFound("Fabricante não encontrado.");
            }

            return fabricante;
        }

        [HttpPost]
        public async Task<ActionResult<Fabricante>> Criar(Fabricante fabricante)
        {
            fabricante.Nome = fabricante.Nome.Trim();
            var nomeJaExiste = await _context.Fabricantes.AnyAsync(f => f.Nome == fabricante.Nome);

            if (nomeJaExiste)
            {
                return BadRequest("Já existe um fabricante com esse nome.");
            }

            try
            {
                _context.Fabricantes.Add(fabricante);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Buscar), new { id = fabricante.Id }, fabricante);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao cadastrar o fabricante.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Fabricante fabricante)
        {
            if (id != fabricante.Id)
            {
                return BadRequest("O ID informado é diferente do ID do fabricante.");
            }

            fabricante.Nome = fabricante.Nome.Trim();
            var fabricanteBanco = await _context.Fabricantes.FindAsync(id);

            if (fabricanteBanco == null)
            {
                return NotFound("Fabricante não encontrado.");
            }

            var nomeJaExiste = await _context.Fabricantes.AnyAsync(f =>
                f.Nome == fabricante.Nome && f.Id != id);

            if (nomeJaExiste)
            {
                return BadRequest("Já existe um fabricante com esse nome.");
            }

            fabricanteBanco.Nome = fabricante.Nome;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao atualizar o fabricante.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);

            if (fabricante == null)
            {
                return NotFound("Fabricante não encontrado.");
            }

            var possuiVeiculo = await _context.Veiculos.AnyAsync(v => v.FabricanteId == id);

            if (possuiVeiculo)
            {
                return BadRequest("O fabricante possui veículos cadastrados.");
            }

            try
            {
                _context.Fabricantes.Remove(fabricante);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir o fabricante.");
            }
        }
    }
}
