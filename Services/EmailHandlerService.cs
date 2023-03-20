using Loxifi;
using Penguin.DependencyInjection.Abstractions.Interfaces;
using Penguin.Reflection;
using Penguin.Templating.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace Penguin.Cms.Modules.Email.Services
{
    public class EmailHandlerService : ISelfRegistering
    {
        private static TypeFactory TypeFactory { get; set; } = new TypeFactory(new TypeFactorySettings());

        private readonly IServiceProvider ServiceProvider;

        private List<IProvideTemplates>? MacroHandlers;

        public EmailHandlerService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public List<ITemplateDefinition> GetHandlers()
        {
            if (MacroHandlers is null)
            {
                MacroHandlers = new List<IProvideTemplates>();

                IEnumerable<Type> MacroHandlerTypes = TypeFactory.GetAllImplementations(typeof(IProvideTemplates));

                foreach (Type thisHandlerType in MacroHandlerTypes)
                {
                    MacroHandlers.Add((IProvideTemplates)ServiceProvider.GetService(thisHandlerType));
                }
            }

            List<ITemplateDefinition> toReturn = new();

            foreach (IProvideTemplates thisHandler in MacroHandlers)
            {
                toReturn.AddRange(thisHandler.GetTemplateDefinitions());
            }

            return toReturn;
        }
    }
}