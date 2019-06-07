using System;
using System.Collections.Generic;

namespace Industria4.Depedencies
{
    public class DependenciesConfiguration
    {

        private List<IDependencyChecker> _checkers = new List<IDependencyChecker>();

        public DependenciesConfiguration(IServiceProvider services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the services
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets the list of configured checker
        /// </summary>
        public IEnumerable<IDependencyChecker> Checkers => _checkers;

        /// <summary>
        /// Adds a new check
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checker"></param>
        /// <returns></returns>
        public DependenciesConfiguration Add<T>(T checker)
            where T : IDependencyChecker
        {
            _checkers.Add(checker);
            return this;
        }
    }
}