using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Data;
using TrabalhoC_.Models;

namespace TrabalhoC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public VeiculosController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> Listar()
        {
            return await _context.Veiculos.OrderBy(v => v.Modelo).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> Buscar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound("Veículo não encontrado.");
            }

            return veiculo;
        }

        [HttpPost]
        public async Task<ActionResult<Veiculo>> Criar(Veiculo veiculo)
        {
            veiculo.Modelo = veiculo.Modelo.Trim();
            var fabricanteExiste = await _context.Fabricantes.AnyAsync(f => f.Id == veiculo.FabricanteId);
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == veiculo.CategoriaId);

            if (!fabricanteExiste)
            {
                return BadRequest("Fabricante não encontrado.");
            }

            if (!categoriaExiste)
            {
                return BadRequest("Categoria não encontrada.");
            }

            try
            {
                _context.Veiculos.Add(veiculo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Buscar), new { id = veiculo.Id }, veiculo);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao cadastrar o veículo.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest("O ID informado é diferente do ID do veículo.");
            }

            veiculo.Modelo = veiculo.Modelo.Trim();
            var veiculoBanco = await _context.Veiculos.FindAsync(id);

            if (veiculoBanco == null)
            {
                return NotFound("Veículo não encontrado.");
            }

            var fabricanteExiste = await _context.Fabricantes.AnyAsync(f => f.Id == veiculo.FabricanteId);
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == veiculo.CategoriaId);

            if (!fabricanteExiste || !categoriaExiste)
            {
                return BadRequest("Fabricante ou categoria não encontrado.");
            }

            veiculoBanco.Modelo = veiculo.Modelo;
            veiculoBanco.AnoFabricacao = veiculo.AnoFabricacao;
            veiculoBanco.Quilometragem = veiculo.Quilometragem;
            veiculoBanco.FabricanteId = veiculo.FabricanteId;
            veiculoBanco.CategoriaId = veiculo.CategoriaId;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao atualizar o veículo.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound("Veículo não encontrado.");
            }

            var possuiAluguel = await _context.Alugueis.AnyAsync(a => a.VeiculoId == id);

            if (possuiAluguel)
            {
                return BadRequest("O veículo possui aluguéis cadastrados.");
            }

            try
            {
                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir o veículo.");
            }
        }
    }
}
