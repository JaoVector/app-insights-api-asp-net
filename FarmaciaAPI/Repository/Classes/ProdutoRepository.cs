using Azure.Storage.Blobs;
using FarmaciaAPI.Context;
using FarmaciaAPI.DTOS;
using FarmaciaAPI.Models;
using FarmaciaAPI.Repository.Interfaces;

namespace FarmaciaAPI.Repository.Classes;


public class ProdutoRepository : Repository<Produtos>, IProdutoRepository
{
    private readonly BlobServiceClient _blobService;
    private readonly BlobContainerClient _containerInstance;
    public ProdutoRepository(FContext context, BlobServiceClient blobService) : base(context, blobService)
    {
        _blobService = blobService;
        _containerInstance = _blobService.GetBlobContainerClient("apifarmablob");
    }

    public async Task ApagaImagens(Imagem imagem)
    {
        var blobInstance = _containerInstance.GetBlobClient(imagem.ImagemName);

        await blobInstance.DeleteAsync();
    }
}
