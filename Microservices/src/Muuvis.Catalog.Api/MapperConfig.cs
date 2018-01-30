using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Muuvis.Catalog.Api
{
    public class MapperConfig
    {
        public static IMapper Get(IServiceProvider provider)
        {
            return new MapperConfiguration(e =>
            {
                e.CreateMap<ReadModel.Movie, Models.Movie.GetModel>();
            }).CreateMapper();
        }
    }
}
