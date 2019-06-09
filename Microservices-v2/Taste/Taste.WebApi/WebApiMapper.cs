using AutoMapper;
using Muuvis.Taste.ReadModel;
using Muuvis.Taste.WebApi.Models.Suggestion;
using Suggestion = Muuvis.Taste.ReadModel.Suggestion;

namespace Muuvis.Taste.WebApi
{
    public class WebApiMapper
    {
        public WebApiMapper()
        {
            Mapper = new MapperConfiguration(m =>
            {
				m.CreateMap<Suggestion, GetApiModel>();
            }).CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
