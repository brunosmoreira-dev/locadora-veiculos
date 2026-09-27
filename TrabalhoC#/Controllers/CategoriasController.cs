using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Data;
using TrabalhoC_.Models;

namespace TrabalhoC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CategoriasController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Listar()
        {
            return await _context.Categorias.OrderBy(c => c.Nome).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Buscar(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            return categoria;
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Criar(Categoria categoria)
        {
            categoria.Nome = categoria.Nome.Trim();
            var nomeJaExiste = await _context.Categorias.AnyAsync(c => c.Nome == categoria.Nome);

            if (nomeJaExiste)
            {
                return BadRequest("Já existe uma categoria com esse nome.");
            }

            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Buscar), new { id = categoria.Id }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao cadastrar a categoria.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest("O ID informado é diferente do ID da categoria.");
            }

            categoria.Nome = categoria.Nome.Trim();
            var categoriaBanco = await _context.Categorias.FindAsync(id);

            if (categoriaBanco == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            var nomeJaExiste = await _context.Categorias.AnyAsync(c =>
                c.Nome == categoria.Nome && c.Id != id);

            if (nomeJaExiste)
            {
                return BadRequest("Já existe uma categoria com esse nome.");
            }

            categoriaBanco.Nome = categoria.Nome;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao atualizar a categoria.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            var possuiVeiculo = await _context.Veiculos.AnyAsync(v => v.CategoriaId == id);

            if (possuiVeiculo)
            {
                return BadRequest("A categoria possui veículos cadastrados.");
            }

            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir a categoria.");
            }
        }
    }
}
