using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Data;
using TrabalhoC_.Models;

namespace TrabalhoC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlugueisController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AlugueisController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluguel>>> Listar()
        {
            return await _context.Alugueis.OrderByDescending(a => a.DataInicio).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aluguel>> Buscar(int id)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);

            if (aluguel == null)
            {
                return NotFound("Aluguel não encontrado.");
            }

            return aluguel;
        }

        [HttpPost]
        public async Task<ActionResult<Aluguel>> Criar(Aluguel aluguel)
        {
            var erro = await ValidarAluguel(aluguel, 0);

            if (erro != null)
            {
                return BadRequest(erro);
            }

            aluguel.ValorTotal = CalcularValorTotal(aluguel);

            try
            {
                _context.Alugueis.Add(aluguel);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Buscar), new { id = aluguel.Id }, aluguel);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao cadastrar o aluguel.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Aluguel aluguel)
        {
            if (id != aluguel.Id)
            {
                return BadRequest("O ID informado é diferente do ID do aluguel.");
            }

            var aluguelBanco = await _context.Alugueis.FindAsync(id);

            if (aluguelBanco == null)
            {
                return NotFound("Aluguel não encontrado.");
            }

            var erro = await ValidarAluguel(aluguel, id);

            if (erro != null)
            {
                return BadRequest(erro);
            }

            aluguelBanco.DataInicio = aluguel.DataInicio;
            aluguelBanco.DataFim = aluguel.DataFim;
            aluguelBanco.DataDevolucao = aluguel.DataDevolucao;
            aluguelBanco.QuilometragemInicial = aluguel.QuilometragemInicial;
            aluguelBanco.QuilometragemFinal = aluguel.QuilometragemFinal;
            aluguelBanco.ValorDiaria = aluguel.ValorDiaria;
            aluguelBanco.ValorTotal = CalcularValorTotal(aluguel);
            aluguelBanco.ClienteId = aluguel.ClienteId;
            aluguelBanco.VeiculoId = aluguel.VeiculoId;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao atualizar o aluguel.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);

            if (aluguel == null)
            {
                return NotFound("Aluguel não encontrado.");
            }

            try
            {
                _context.Alugueis.Remove(aluguel);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir o aluguel.");
            }
        }

        private async Task<string?> ValidarAluguel(Aluguel aluguel, int idAtual)
        {
            if (aluguel.DataInicio == default || aluguel.DataFim == default || aluguel.DataDevolucao == default)
            {
                return "Preencha todas as datas.";
            }

            if (aluguel.DataFim < aluguel.DataInicio)
            {
                return "A data final não pode ser menor que a data inicial.";
            }

            if (aluguel.DataDevolucao < aluguel.DataInicio)
            {
                return "A data de devolução não pode ser menor que a data inicial.";
            }

            if (aluguel.QuilometragemFinal < aluguel.QuilometragemInicial)
            {
                return "A quilometragem final não pode ser menor que a inicial.";
            }

            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == aluguel.ClienteId);
            var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == aluguel.VeiculoId);

            if (!clienteExiste)
            {
                return "Cliente não encontrado.";
            }

            if (!veiculoExiste)
            {
                return "Veículo não encontrado.";
            }

            var veiculoOcupado = await _context.Alugueis.AnyAsync(a =>
                a.VeiculoId == aluguel.VeiculoId &&
                a.Id != idAtual &&
                a.DataInicio <= aluguel.DataFim &&
                a.DataFim >= aluguel.DataInicio);

            if (veiculoOcupado)
            {
                return "O veículo já está alugado nesse período.";
            }

            return null;
        }

        private decimal CalcularValorTotal(Aluguel aluguel)
        {
            var quantidadeDias = (aluguel.DataFim.Date - aluguel.DataInicio.Date).Days;

            if (quantidadeDias < 1)
            {
                quantidadeDias = 1;
            }

            return quantidadeDias * aluguel.ValorDiaria;
        }
    }
}
