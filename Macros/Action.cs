using Loxifi;
using Penguin.Cms.Email.Abstractions.Attributes;
using Penguin.Cms.Web.Macros;
using Penguin.Email.Abstractions.Interfaces;
using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Reflection;
using Penguin.Templating.Abstractions;
using Penguin.Templating.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Penguin.Cms.Modules.Email.Macros
{
    public class Action : IMessageHandler<Penguin.Messaging.Application.Messages.Startup>, IProvideTemplates
    {
        private static List<ITemplateDefinition>? Cache;

        public void AcceptMessage(Penguin.Messaging.Application.Messages.Startup message)
        {
            Refresh();
        }

        public static List<ITemplateDefinition> GetTemplateDefinitions()
        {
            return Cache ?? throw new NullReferenceException("Cache was not initialized before access");
        }

        IEnumerable<ITemplateDefinition> IProvideTemplates.GetTemplateDefinitions()
        {
            return GetTemplateDefinitions();
        }

        private void Refresh()
        {
            List<ITemplateDefinition> toReturn = new();

            List<Type> IMessageHandlers = TypeFactory.Default.GetAllImplementations(typeof(IEmailHandler)).ToList();

            List<MethodInfo> MessageHandlers = IMessageHandlers.SelectMany(mh =>
                mh.GetMethods().Where(m =>
                    m.GetCustomAttribute<EmailHandlerAttribute>() != null &&
                    m.DeclaringType == mh
                )).ToList();

            foreach (MethodInfo thisMethod in MessageHandlers)
            {
                TemplateDefinition toAdd = new(thisMethod.GetCustomAttribute<EmailHandlerAttribute>()?.HandlerName ?? $"{thisMethod.DeclaringType}.{thisMethod.Name}", GetType());

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