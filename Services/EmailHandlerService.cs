using Penguin.DependencyInjection.Abstractions.Interfaces;
using Penguin.Reflection;
using Penguin.Templating.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace Penguin.Cms.Modules.Email.Services
{
    public class EmailHandlerService : ISelfRegistering
    {
        private readonly IServiceProvider ServiceProvider;

        private List<IProvideTemplates>? MacroHandlers;

        public EmailHandlerService(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public List<ITemplateDefinition> GetHandlers()
        {
            if (this.MacroHandlers is null)
            {
                this.MacroHandlers = new List<IProvideTemplates>();

                IEnumerable<Type> MacroHandlerTypes = TypeFactory.GetAllImplementations(typeof(IProvideTemplates));

                foreach (Type thisHandlerType in MacroHandlerTypes)
                {
                    this.MacroHandlers.Add((IProvideTemplates)this.ServiceProvider.GetService(thisHandlerType));
                }
            }

            List<ITemplateDefinition> toReturn = new List<ITemplateDefinition>();

            foreach (IProvideTemplates thisHandler in this.MacroHandlers)
            {
                toReturn.AddRange(thisHandler.GetTemplateDefinitions());
            }

            return toReturn;
        }
    }
}