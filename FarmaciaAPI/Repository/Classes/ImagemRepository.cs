using Azure.Storage.Blobs;
using FarmaciaAPI.Context;
using FarmaciaAPI.DTOS;
using FarmaciaAPI.Models;
using FarmaciaAPI.Repository.Interfaces;

namespace FarmaciaAPI.Repository.Classes;

public class ImagemRepository : Repository<Imagem>, IImagemRepository
{
	private readonly BlobServiceClient _blobService;
    private readonly BlobContainerClient _containerInstance;
	public ImagemRepository(FContext context, BlobServiceClient blobService) : base(context, blobService)
	{
		_blobService = blobService;
        _containerInstance = _blobService.GetBlobContainerClient("apifarmablob");
    }


    public async Task<string> UploadImage(ImagemDTO imagemDTO)
    {
        var blobInstance = _containerInstance.GetBlobClient(imagemDTO.File.FileName);

        await blobInstance.UploadAsync(imagemDTO.File.OpenReadStream());

        return blobInstance.Uri.AbsoluteUri;
    }

    public async Task<string> ApagaImagem(Imagem imagem) 
    {
        var blobInstance = _containerInstance.GetBlobClient(imagem.ImagemName);

        await blobInstance.DeleteAsync();

        return blobInstance.Uri.AbsoluteUri;
    }
}
