using AutoMapper;
using FarmaciaAPI.DTOS;
using FarmaciaAPI.Middlewares.Exceptions;
using FarmaciaAPI.Models;
using FarmaciaAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace FarmaciaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagemController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    public ImagemController(IMapper mapper, IUnitOfWork uof)
    {
        _uof = uof;
        _mapper = mapper;
    }


    [HttpGet]
    public ActionResult<IEnumerable<ReadImagemDTO>> GetImagens([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {
        return Ok(_mapper.Map<List<ReadImagemDTO>>(_uof.ImagemRepository.Get(skip, take).ToList()));
    }

    [HttpPost(nameof(UploadImage))]
    public async Task<ActionResult> UploadImage([FromForm] ImagemDTO imagemDTO)
    {
       
        var produto = _uof.ProdutoRepository.BuscaPorID(p => p.ProdutoId == imagemDTO.ProdutoId);

        if (produto == null) throw new NotFoundException($"Não existe produto com o ID: {imagemDTO.ProdutoId}");

        var img = await _uof.ImagemRepository.UploadImage(imagemDTO);

        Imagem imagem = _mapper.Map<Imagem>(imagemDTO);

        imagem.ImagemName = imagemDTO.File.FileName; imagem.ImagemURL = img;
        
        _uof.ImagemRepository.Add(imagem);

        await _uof.Commit();

        return Ok(imagem);
        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadImagemDTO>> BuscaPorID(int id) 
    {
        var imagem = await _uof.ImagemRepository.BuscaPorID(img => img.ImageId == id);

        if (imagem == null) throw new NotFoundException("Imagem Não Encontrada");

        ReadImagemDTO readImagemId = _mapper.Map<ReadImagemDTO>(imagem);

        return Ok(readImagemId);
    }


    [HttpDelete(nameof(DeletaImagemPelaUrl))]
    public async Task<ActionResult<ReadImagemDTO>> DeletaImagemPelaUrl([FromQuery] string url) 
    {
        Imagem imagem = await _uof.ImagemRepository.BuscaPorID(img => img.ImagemURL == url);

        if (imagem == null) throw new NotFoundException("Imagem Não Encontrada");
        
        var imgDeletada = await _uof.ImagemRepository.ApagaImagem(imagem);

        if (imgDeletada == null) throw new NotFoundException("Url não Encontrada no Blob");

        try
        {
            _uof.ImagemRepository.Delete(imagem);
            await _uof.Commit();
        }
        catch (DBConcurrencyException)
        {
            throw;
        }
       
        ReadImagemDTO readImagem = _mapper.Map<ReadImagemDTO>(imagem);

        return Ok(readImagem);
    }

}

