﻿using Penguin.Cms.Email.Abstractions.Attributes;
using Penguin.Email.Abstractions.Interfaces;
using Penguin.Reflection;
using Penguin.Reflection.Extensions;
using Penguin.Testing.Interfaces;
using Penguin.Testing.RuntimeValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Penguin.Cms.Modules.Email.Validators
{
    public class EmailHandlingSystem : IRuntimeValidator
    {
        public ValidationResults Validate()
        {
            ValidationResults results = new ValidationResults();

            IEnumerable<Type> AllTypes = TypeFactory.GetAllTypes().Where(t => !t.IsAbstract && !t.IsInterface);

            List<Type> NotIEmailHandlerClasses = AllTypes.Where(t => !t.ImplementsInterface<IEmailHandler>()).ToList();

            List<Type> IEmailHandlerClasses = AllTypes.Where(t => t.ImplementsInterface<IEmailHandler>()).ToList();

            //Check for EmailHandler methods that aren't on classes that would be found
            foreach (Type thisType in NotIEmailHandlerClasses)
            {
                foreach (MethodInfo thisMethod in thisType.GetMethods())
                {
                    if (thisMethod.GetCustomAttribute<EmailHandlerAttribute>() != null)
                    {
                        results.AddResult(new ValidationResult()
                        {
                            Checked = thisType,
                            Success = false,
                            Message = $"{thisType.Name} contains method {thisMethod.Name} that is registered as an email handler, but the type does not implement {nameof(IEmailHandler)}"
                        });
                    }
                }
            }

            //Check IEmailHandler classes to make sure they all actually have at least one method that sends emails
            foreach (Type thisType in IEmailHandlerClasses)
            {
                if (!thisType.GetMethods().Any(m => m.GetCustomAttribute<EmailHandlerAttribute>() != null))
                {
                    results.AddResult(new ValidationResult()
                    {
                        Checked = thisType,
                        Success = false,
                        Message = $"{thisType.Name} implements {nameof(IEmailHandler)} but no methods with attribute {nameof(EmailHandlerAttribute)} were found"
                    });
                }
            }

            return results;
        }
    }
}