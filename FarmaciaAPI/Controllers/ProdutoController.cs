using AutoMapper;
using FarmaciaAPI.DTOS;
using FarmaciaAPI.Middlewares.Exceptions;
using FarmaciaAPI.Models;
using FarmaciaAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProdutoController : Controller
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutoController(IUnitOfWork context, IMapper mapper)
    {
        _uof = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadProdutoDTO>>> Get([FromQuery] int skip = 0, [FromQuery] int take = 4)
    {
        var consulta = await _uof.ProdutoRepository.Get(skip, take).ToListAsync();

        if (consulta == null) throw new NotFoundException("Erro ao Retornar os Produtos");

        var produtos = _mapper.Map<List<ReadProdutoDTO>>(consulta);
        return produtos;

    }

    [HttpGet("{id}", Name = "Obtem Produto")]
    public async Task<ActionResult<ReadProdutoDTO>> GetProduto(int id)
    {
        var produto = await _uof.ProdutoRepository.BuscaPorID(p => p.ProdutoId == id);

        if (produto == null) throw new NotFoundException("Produto não encontrado");

        ReadProdutoDTO readProduto = _mapper.Map<ReadProdutoDTO>(produto);

        return Ok(readProduto);
    }


    [HttpPost]
    public async Task<ActionResult> Add([FromBody] ProdutoDTO produtoDto)
    {

        Produtos produto = _mapper.Map<Produtos>(produtoDto);

        _uof.ProdutoRepository.Add(produto);

        await _uof.Commit();

        return Ok(produto);

    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
    {
        var produto = await _uof.ProdutoRepository.BuscaPorID(p => p.ProdutoId == id);

        if (produto == null) throw new NotFoundException("Produto não encontrado");

        _mapper.Map(produtoDto, produto);

        await _uof.Commit();

        return Ok();
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {

        var produto = await _uof.ProdutoRepository.BuscaPorID(pro => pro.ProdutoId == id);

        if (produto == null) throw new NotFoundException("Produto não encontrado");

        foreach (Imagem imgNome in produto.Imagens)
        {
            await _uof.ProdutoRepository.ApagaImagens(imgNome);
        }

        _uof.ProdutoRepository.Delete(produto);
        await _uof.Commit();

        return NoContent();
    }
}
