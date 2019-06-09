using System.Collections.Generic;
using AutoMapper;

namespace Muuvis.Taste.EntityFramework
{
    internal class TasteMapper
    {
        public TasteMapper()
        {
            Mapper = new MapperConfiguration(c =>
            {
                c.CreateMap<DataModel.Suggestion, ReadModel.Suggestion>();

            }).CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
