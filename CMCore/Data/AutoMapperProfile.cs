using AutoMapper;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Domain to Dto
            CreateMap<File, FileDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<Club, ClubDto>();
            CreateMap<Region, RegionDto>();
            CreateMap<Countrie, CountrieDto>();
            CreateMap<Type, TypeDto>();
            CreateMap<Extension, ExtensionDto>();
            CreateMap<Companie, CompanieDto>();

            // Dto to Domain
            CreateMap<FileDto, File>()
                .ForMember(f => f.Id, opt => opt.Ignore());

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
