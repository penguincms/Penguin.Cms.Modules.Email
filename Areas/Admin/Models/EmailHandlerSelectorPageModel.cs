using Penguin.Templating.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Cms.Modules.Email.Areas.Admin.Models
{
    public class EmailHandlerSelectorPageModel
    {
        public List<ITemplateDefinition> Handlers { get; }

        public string Selected { get; set; } = string.Empty;

        public EmailHandlerSelectorPageModel(IEnumerable<ITemplateDefinition> handlers)
        {
            this.Handlers = handlers.ToList();
        }
    }
}