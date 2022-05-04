using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Email.Templating;
using Penguin.Cms.Email.Templating.Repositories;
using Penguin.Cms.Modules.Dynamic.Areas.Admin.Controllers;
using Penguin.Cms.Modules.Dynamic.Attributes;
using Penguin.Cms.Modules.Email.Areas.Admin.Models;
using Penguin.Cms.Modules.Email.Services;
using Penguin.Persistence.Abstractions.Attributes.Control;
using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Security.Abstractions.Interfaces;
using System;

namespace Penguin.Cms.Modules.Email.Areas.Admin.Controllers
{
    public class EmailTemplateController : ObjectManagementController<EmailTemplate>
    {
        protected EmailHandlerService EmailHandlerService { get; set; }

        protected EmailTemplateRepository EmailTemplateRepository { get; set; }

        public EmailTemplateController(EmailTemplateRepository emailTemplateRepository, EmailHandlerService emailHandlerService, IServiceProvider serviceProvider, IUserSession userSession) : base(serviceProvider, userSession)
        {
            this.EmailHandlerService = emailHandlerService;
            this.EmailTemplateRepository = emailTemplateRepository;
        }

        [DynamicPropertyHandler(DisplayContexts.Edit, typeof(EmailTemplate), nameof(EmailTemplate.HandlerName))]
        public ActionResult EmailHandlerSelector(IMetaObject o)
        {
            if (o is null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            return this.View(new EmailHandlerSelectorPageModel(this.EmailHandlerService.GetHandlers()) { Selected = o.Value });
        }

        public ActionResult LeftPane()
        {
            return this.View();
        }
    }
}