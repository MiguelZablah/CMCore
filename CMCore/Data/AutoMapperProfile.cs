using AutoMapper;
using CMCore.DTO;
using CMCore.Models;
using System.Linq;
using Type = CMCore.Models.Type;

namespace CMCore.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Domain to Dto
            CreateMap<File, FileDto>()
                .ForMember(dto => dto.Tags, opt => opt.MapFrom(ft => ft.FileTags.Select(t => t.Tag)))
                .ForMember(dto => dto.Clubs, opt => opt.MapFrom(fc => fc.FileClubs.Select(c => c.Club)))
                .ForMember(dto => dto.Companies, opt => opt.MapFrom(fco => fco.FileCompanies.Select(c => c.Companie)));

            CreateMap<Tag, TagDto>();

            CreateMap<Club, ClubDto>()
                .ForMember(dto => dto.Regions, opt => opt.MapFrom(cr => cr.ClubRegions.Select(r => r.Region)))
                .ForMember(dto => dto.Types, opt => opt.MapFrom(fty => fty.ClubTypes.Select(ty => ty.Type)));

            CreateMap<Region, RegionDto>();

            CreateMap<Countrie, CountrieDto>();

            CreateMap<Type, TypeDto>();

            CreateMap<Extension, ExtensionDto>();

            CreateMap<Companie, CompanieDto>();

            // Dto to Domain
            CreateMap<FileDto, File>()
                .ForMember(t => t.Id, opt => opt.Ignore());

            CreateMap<TagDto, Tag>()
                .ForMember(t => t.Id, opt => opt.Ignore());

            CreateMap<ClubDto, Club>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<RegionDto, Region>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<CountrieDto, Countrie>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<TypeDto, Type>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<ExtensionDto, Extension>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<CompanieDto, Companie>()
                .ForMember(c => c.Id, opt => opt.Ignore());
        }

    }
}
