using System.Collections.Generic;
using AutoMapper;
using Muuvis.Catalog.ReadModel;

namespace Muuvis.Catalog.EntityFramework
{
    internal class CatalogMapper
    {
        public CatalogMapper()
        {
            Mapper = new MapperConfiguration(c =>
            {
                c.CreateMap<DataModel.TitleTranslation, MovieRead>()
                    .ForMember(rm => rm.Id, dm => dm.MapFrom(r => r.Movie.Id))
                    .ForMember(rm => rm.Year, dm => dm.MapFrom(r => r.Movie.Year));
            }).CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
