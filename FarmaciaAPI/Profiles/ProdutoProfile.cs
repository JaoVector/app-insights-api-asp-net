using AutoMapper;
using FarmaciaAPI.DTOS;
using FarmaciaAPI.Models;

namespace FarmaciaAPI.Profiles;

public class ProdutoProfile : Profile
{
	public ProdutoProfile()
	{
		CreateMap<ProdutoDTO, Produtos>();
		CreateMap<Produtos, ReadProdutoDTO>().ForMember(e => e.Imagens, opt => opt.MapFrom(p => p.Imagens));
	}
}
