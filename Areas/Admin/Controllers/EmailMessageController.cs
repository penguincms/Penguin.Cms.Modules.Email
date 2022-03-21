using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Email;
using Penguin.Cms.Modules.Dynamic.Areas.Admin.Controllers;
using Penguin.Cms.Modules.Email.Services;
using Penguin.Security.Abstractions.Interfaces;
using System;

namespace Penguin.Cms.Modules.Email.Areas.Admin.Controllers
{
    /// <summary>
    /// A controller used to display Email Messages for the CMS
    /// </summary>
    public class EmailMessageController : ObjectManagementController<EmailMessage>
    {
        /// <summary>
        /// The service used to interact with email handlers and templates
        /// </summary>
        protected EmailHandlerService EmailHandlerService { get; set; }

        /// <summary>
        /// Constructs a new instance of this controller
        /// </summary>
        /// <param name="emailHandlerService">The service used to interact with email handlers and templates</param>
        /// <param name="serviceProvider">An instance of a service provider</param>
        public EmailMessageController(EmailHandlerService emailHandlerService, IServiceProvider serviceProvider, IUserSession userSession) : base(serviceProvider, userSession)
        {
            this.EmailHandlerService = emailHandlerService;
        }

        /// <summary>
        /// The left pane for views managed by this controller
        /// </summary>
        /// <returns>The left pane for views managed by this controller</returns>
        public ActionResult LeftPane() => this.View();
    }
}