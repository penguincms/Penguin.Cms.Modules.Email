using Penguin.Cms.Email.Abstractions.Attributes;
using Penguin.Cms.Web.Macros;
using Penguin.Email.Abstractions.Interfaces;
using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Reflection;
using Penguin.Templating.Abstractions;
using Penguin.Templating.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Penguin.Cms.Modules.Email.Macros
{
    public class Action : IMessageHandler<Penguin.Messaging.Application.Messages.Startup>, IProvideTemplates
    {
        private static List<ITemplateDefinition>? Cache;

        public void AcceptMessage(Penguin.Messaging.Application.Messages.Startup startup)
        {
            this.Refresh();
        }

        public List<ITemplateDefinition> GetTemplateDefinitions()
        {
            return Cache ?? throw new NullReferenceException("Cache was not initialized before access");
        }

        IEnumerable<ITemplateDefinition> IProvideTemplates.GetTemplateDefinitions()
        {
            return this.GetTemplateDefinitions();
        }

        private void Refresh()
        {
            List<ITemplateDefinition> toReturn = new List<ITemplateDefinition>();

            List<Type> IMessageHandlers = TypeFactory.GetAllImplementations(typeof(IEmailHandler)).ToList();

            List<MethodInfo> MessageHandlers = IMessageHandlers.SelectMany(mh =>
                mh.GetMethods().Where(m =>
                    m.GetCustomAttribute<EmailHandlerAttribute>() != null &&
                    m.DeclaringType == mh
                )).ToList();

            foreach (MethodInfo thisMethod in MessageHandlers)
            {
                TemplateDefinition toAdd = new TemplateDefinition(thisMethod.GetCustomAttribute<EmailHandlerAttribute>()?.HandlerName ?? $"{thisMethod.DeclaringType}.{thisMethod.Name}", this.GetType());

                foreach (ParameterInfo thisParameter in thisMethod.GetParameters())
                {
                    if (thisParameter.Name != null)
                    {
                        toAdd.Children.Add(new ModelBindingMacro(thisParameter.Name, thisParameter.ParameterType));
                    }
                }

                toReturn.Add(toAdd);
            }

            Cache = toReturn;
        }
    }
}