using CriandoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")] // Rota base: /api/produtos
public class ProdutosController : ControllerBase
{
    // Lista em memória (simulando um "banco de dados")
    private static List<Produto> _produtos = new List<Produto>
    {
        new Produto { Id = 1, Nome = "Notebook", Preco = 3500.00m },
        new Produto { Id = 2, Nome = "Mouse", Preco = 80.50m }
    };

    // GET api/produtos?nome=xxx&ordenarPorPreco=true
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get(
        [FromQuery] string nome = null,
        [FromQuery] bool ordenarPorPreco = false)
    {
        var produtosFiltrados = _produtos.AsQueryable();

        // Filtro por nome (se fornecido)
        if (!string.IsNullOrEmpty(nome))
        {
            produtosFiltrados = produtosFiltrados
                .Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        }

        // Ordenação por preço (se solicitado)
        if (ordenarPorPreco)
        {
            produtosFiltrados = produtosFiltrados.OrderBy(p => p.Preco);
        }

        return Ok(produtosFiltrados.ToList());
    }

    // POST: api/produtos
    [HttpPost]
    public ActionResult<Produto> Post([FromBody] Produto produto)
    {
        // Gera um novo ID (simulação)
        produto.Id = _produtos.Max(p => p.Id) + 1;

        _produtos.Add(produto); // Adiciona à lista

        // Retorna 201 Created com o produto criado e a rota para acessá-lo
        return CreatedAtAction(nameof(Get), produto);
    }
}