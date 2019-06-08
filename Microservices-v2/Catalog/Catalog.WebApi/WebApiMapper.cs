using AutoMapper;
using Muuvis.Catalog.ReadModel;

namespace Muuvis.Catalog.WebApi
{
    public class WebApiMapper
    {
        public WebApiMapper()
        {
            Mapper = new MapperConfiguration(m =>
            {
				m.CreateMap<MovieRead, Models.Movie.GetApiModel>();
            }).CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
