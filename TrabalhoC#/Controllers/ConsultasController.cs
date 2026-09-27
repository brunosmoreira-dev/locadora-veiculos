using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Data;

namespace TrabalhoC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ConsultasController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("veiculos-por-fabricante/{fabricanteId}")]
        public async Task<IActionResult> VeiculosPorFabricante(int fabricanteId)
        {
            // INNER JOIN de veículos, fabricantes e categorias
            var resultado = await (
                from veiculo in _context.Veiculos
                join fabricante in _context.Fabricantes
                    on veiculo.FabricanteId equals fabricante.Id
                join categoria in _context.Categorias
                    on veiculo.CategoriaId equals categoria.Id
                where fabricante.Id == fabricanteId
                select new
                {
                    veiculo.Id,
                    veiculo.Modelo,
                    veiculo.AnoFabricacao,
                    veiculo.Quilometragem,
                    Fabricante = fabricante.Nome,
                    Categoria = categoria.Nome
                }).ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("veiculos-por-categoria/{categoriaId}")]
        public async Task<IActionResult> VeiculosPorCategoria(int categoriaId)
        {
            // INNER JOIN de veículos, fabricantes e categorias
            var resultado = await (
                from veiculo in _context.Veiculos
                join fabricante in _context.Fabricantes
                    on veiculo.FabricanteId equals fabricante.Id
                join categoria in _context.Categorias
                    on veiculo.CategoriaId equals categoria.Id
                where categoria.Id == categoriaId
                select new
                {
                    veiculo.Id,
                    veiculo.Modelo,
                    veiculo.AnoFabricacao,
                    veiculo.Quilometragem,
                    Fabricante = fabricante.Nome,
                    Categoria = categoria.Nome
                }).ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("alugueis-por-cliente/{clienteId}")]
        public async Task<IActionResult> AlugueisPorCliente(int clienteId)
        {
            // INNER JOIN de aluguéis, clientes e veículos
            var resultado = await (
                from aluguel in _context.Alugueis
                join cliente in _context.Clientes
                    on aluguel.ClienteId equals cliente.Id
                join veiculo in _context.Veiculos
                    on aluguel.VeiculoId equals veiculo.Id
                where cliente.Id == clienteId
                select new
                {
                    aluguel.Id,
                    aluguel.DataInicio,
                    aluguel.DataFim,
                    aluguel.ValorTotal,
                    Cliente = cliente.Nome,
                    Veiculo = veiculo.Modelo
                }).ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("alugueis-por-periodo")]
        public async Task<IActionResult> AlugueisPorPeriodo(DateTime inicio, DateTime fim)
        {
            if (inicio == default || fim == default)
            {
                return BadRequest("Informe a data inicial e a data final.");
            }

            if (fim < inicio)
            {
                return BadRequest("A data final não pode ser menor que a data inicial.");
            }

            // INNER JOIN de aluguéis, clientes e veículos
            var resultado = await (
                from aluguel in _context.Alugueis
                join cliente in _context.Clientes
                    on aluguel.ClienteId equals cliente.Id
                join veiculo in _context.Veiculos
                    on aluguel.VeiculoId equals veiculo.Id
                where aluguel.DataInicio <= fim && aluguel.DataFim >= inicio
                select new
                {
                    aluguel.Id,
                    aluguel.DataInicio,
                    aluguel.DataFim,
                    aluguel.ValorTotal,
                    Cliente = cliente.Nome,
                    Veiculo = veiculo.Modelo
                }).ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("veiculos-disponiveis")]
        public async Task<IActionResult> VeiculosDisponiveis(DateTime inicio, DateTime fim)
        {
            if (inicio == default || fim == default)
            {
                return BadRequest("Informe a data inicial e a data final.");
            }

            if (fim < inicio)
            {
                return BadRequest("A data final não pode ser menor que a data inicial.");
            }

            var alugueisDoPeriodo = _context.Alugueis.Where(a =>
                a.DataInicio <= fim && a.DataFim >= inicio);

            // LEFT JOIN para encontrar veículos sem aluguel no período
            var resultado = await (
                from veiculo in _context.Veiculos
                join fabricante in _context.Fabricantes
                    on veiculo.FabricanteId equals fabricante.Id
                join categoria in _context.Categorias
                    on veiculo.CategoriaId equals categoria.Id
                join aluguel in alugueisDoPeriodo
                    on veiculo.Id equals aluguel.VeiculoId into grupoAlugueis
                from aluguel in grupoAlugueis.DefaultIfEmpty()
                where aluguel == null
                select new
                {
                    veiculo.Id,
                    veiculo.Modelo,
                    veiculo.AnoFabricacao,
                    Fabricante = fabricante.Nome,
                    Categoria = categoria.Nome
                }).ToListAsync();

            return Ok(resultado);
        }
    }
}
