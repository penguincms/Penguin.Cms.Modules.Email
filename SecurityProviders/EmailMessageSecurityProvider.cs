using Penguin.Cms.Email;
using Penguin.Cms.Entities;
using Penguin.Cms.Modules.Email.Constants.Strings;
using Penguin.DependencyInjection.Abstractions.Attributes;
using Penguin.Security.Abstractions;
using Penguin.Security.Abstractions.Extensions;
using Penguin.Security.Abstractions.Interfaces;
using System;

namespace Penguin.Cms.Modules.Email.SecurityProviders
{
    [Register(DependencyInjection.Abstractions.Enums.ServiceLifetime.Scoped, typeof(ISecurityProvider<EmailMessage>))]
    public class EmailMessageSecurityProvider : ISecurityProvider<EmailMessage>
    {
        public EmailMessageSecurityProvider(IUserSession userSession, ISecurityProvider<Entity> securityProvider)
        {
            this.UserSession = userSession;
            this.EntitySecurityProvider = securityProvider;
        }

        private IUserSession UserSession { get; set; }
        private ISecurityProvider<Entity> EntitySecurityProvider { get; set; }

        public void AddPermissions(EmailMessage entity, PermissionTypes permissionTypes, ISecurityGroup source = null) => this.EntitySecurityProvider.AddPermissions(entity, permissionTypes, source);

        public void AddPermissions(EmailMessage entity, PermissionTypes permissionTypes, Guid source) => this.EntitySecurityProvider.AddPermissions(entity, permissionTypes, source);

        public bool CheckAccess(EmailMessage entity, PermissionTypes permissionTypes = PermissionTypes.Read)
        {
            if (this.UserSession.IsLoggedIn && this.UserSession.LoggedInUser.HasRole(RoleNames.CONTENT_MANAGER))
            {
                return true;
            }

            return this.EntitySecurityProvider.CheckAccess(entity, permissionTypes);
        }

        public void ClonePermissions(EmailMessage source, EmailMessage destination) => this.EntitySecurityProvider.ClonePermissions(source, destination);

        public void SetDefaultPermissions(params EmailMessage[] o) => this.EntitySecurityProvider.SetDefaultPermissions(o);

        public void SetLoggedIn(EmailMessage entity) => this.EntitySecurityProvider.SetLoggedIn(entity);

        public void SetPublic(EmailMessage entity) => this.EntitySecurityProvider.SetPublic(entity);
    }
}
