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
            Mapper.Initialize(cfg =>
            {
                // Domain to Dto
                cfg.CreateMap<File, FileDto>()
                    .ForMember(dto => dto.Tags, opt => opt.MapFrom(ft => ft.FileTags.Select(t => t.Tag)))
                    .ForMember(dto => dto.Clubs, opt => opt.MapFrom(fc => fc.FileClubs.Select(c => c.Club)))
                    .ForMember(dto => dto.Companies, opt => opt.MapFrom(fco => fco.FileCompanies.Select(c => c.Companie)))
                    .ForMember(dto => dto.Extension, opt => opt.MapFrom(fco => fco.Extension.Name));

                cfg.CreateMap<Tag, TagDto>();

                cfg.CreateMap<Club, ClubDto>()
                    .ForMember(dto => dto.Regions, opt => opt.MapFrom(cr => cr.ClubRegions.Select(r => r.Region)))
                    .ForMember(dto => dto.Types, opt => opt.MapFrom(fty => fty.ClubTypes.Select(ty => ty.Type)));

                cfg.CreateMap<Region, RegionDto>();

                cfg.CreateMap<Countrie, CountrieDto>();

                cfg.CreateMap<Type, TypeDto>();

                cfg.CreateMap<Extension, ExtensionDto>();

                cfg.CreateMap<Companie, CompanieDto>();

                // Dto to Domain
                cfg.CreateMap<FileDto, File>()
                    .ForMember(f => f.Id, opt => opt.Ignore());

                cfg.CreateMap<TagDto, Tag>()
                    .ForMember(t => t.Id, opt => opt.Ignore());

                cfg.CreateMap<ClubDto, Club>()
                    .ForMember(c => c.Id, opt => opt.Ignore());

                cfg.CreateMap<RegionDto, Region>()
                    .ForMember(r => r.Id, opt => opt.Ignore());

                cfg.CreateMap<CountrieDto, Countrie>()
                    .ForMember(c => c.Id, opt => opt.Ignore());

                cfg.CreateMap<TypeDto, Type>()
                    .ForMember(t => t.Id, opt => opt.Ignore());

                cfg.CreateMap<ExtensionDto, Extension>()
                    .ForMember(e => e.Id, opt => opt.Ignore());

                cfg.CreateMap<CompanieDto, Companie>()
                    .ForMember(c => c.Id, opt => opt.Ignore());
            });

            
        }

    }
}
