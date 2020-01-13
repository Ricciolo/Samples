using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OData.Edm;
using ODataSample;
using ODataSampleApi.Controllers;

namespace ODataSampleApi.Component
{
    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        private readonly Dictionary<string, string> _types;

        public GenericControllerRouteConvention(IEdmModel edmModel)
        {
            _types = edmModel.EntityContainer.Elements.OfType<EdmEntitySet>()
                .ToDictionary(e => ((EdmCollectionType)e.Type).ElementType.FullName(), e => e.Name);
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType && controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericController<>)
                && _types.TryGetValue(controller.ControllerType.GetGenericArguments()[0].FullName, out string name))
            {
                controller.ControllerName = name;
            }
        }
    }
}
