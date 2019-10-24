using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Email;
using Penguin.Cms.Email.Templating.Repositories;
using Penguin.Cms.Modules.Dynamic.Areas.Admin.Controllers;
using Penguin.Cms.Modules.Email.Services;
using System;

namespace Penguin.Cms.Modules.Email.Areas.Admin.Controllers
{
    public class EmailMessageController : ObjectManagementController<EmailMessage>
    {
        protected EmailHandlerService EmailHandlerService { get; set; }

        protected EmailTemplateRepository EmailTemplateRepository { get; set; }

        public EmailMessageController(EmailTemplateRepository emailTemplateRepository, EmailHandlerService emailHandlerService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            EmailHandlerService = emailHandlerService;
            EmailTemplateRepository = emailTemplateRepository;
        }

        public ActionResult LeftPane() => this.View();
    }
}