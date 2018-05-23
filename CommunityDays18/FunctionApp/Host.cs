using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionApp
{
    public static class Host
    {
        public static IServiceProvider Create(Action<IServiceCollection> configure)
        {
            // Registrazione delle dipendenze globali
            var services = new ServiceCollection();
            services.AddOptions();

            // Registrazione delle dipendenze esterne
            configure(services);

            // Creazione del container
            return services.BuildServiceProvider();
        }
    }
}
