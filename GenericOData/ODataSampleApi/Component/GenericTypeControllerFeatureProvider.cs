using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using ODataSample;
using ODataSampleApi.Controllers;

namespace ODataSampleApi.Component
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IEnumerable<Type> _types;

        public GenericTypeControllerFeatureProvider(Assembly assembly) : this(assembly.GetExportedTypes())
        {

        }

        public GenericTypeControllerFeatureProvider(IEnumerable<Type> types)
        {
            _types = types;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var candidate in _types)
            {
                feature.Controllers.Add(typeof(GenericController<>).MakeGenericType(candidate).GetTypeInfo());
            }
        }
    }
}
