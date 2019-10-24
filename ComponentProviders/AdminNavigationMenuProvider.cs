using Penguin.Cms.Modules.Core.ComponentProviders;
using Penguin.Cms.Modules.Core.Navigation;
using Penguin.Cms.Modules.Email.Constants.Strings;
using Penguin.Navigation.Abstractions;
using Penguin.Security.Abstractions;
using Penguin.Security.Abstractions.Interfaces;
using System.Collections.Generic;
using SecurityRoleNames = Penguin.Security.Abstractions.Constants.RoleNames;

namespace Penguin.Cms.Modules.Email.ComponentProviders
{
    public class AdminNavigationMenuProvider : NavigationMenuProvider
    {
        public override INavigationMenu GenerateMenuTree()
        {
            return new NavigationMenu()
            {
                Name = "Admin",
                Text = "Admin",
                Children = new List<INavigationMenu>() {
                    new NavigationMenu()
                    {
                        Text = "Email",
                        Name = "EmailAdmin",
                        Href = "/Admin/EmailMessage/Index",
                        Permissions = new List<ISecurityGroupPermission>()
                        {
                            this.CreatePermission(RoleNames.ContentManager, PermissionTypes.Read),
                            this.CreatePermission(SecurityRoleNames.SysAdmin, PermissionTypes.Read | PermissionTypes.Write)
                        },
                        Children = new List<INavigationMenu>()
                        {
                            new NavigationMenu()
                            {
                                Text = "View Email Queue",
                                Name = "ListEmails",
                                Icon = "list",
                                Href = "/Admin/EmailMessage/List",
                                Permissions = new List<ISecurityGroupPermission>()
                                {
                                    this.CreatePermission(RoleNames.ContentManager, PermissionTypes.Read),
                                    this.CreatePermission(SecurityRoleNames.SysAdmin, PermissionTypes.Read | PermissionTypes.Write)
                                }
                            },
                            new NavigationMenu()
                            {
                                Text = "View Templates",
                                Name = "ListTemplates",
                                Icon = "list",
                                Href = "/Admin/EmailTemplate/List",
                                Permissions = new List<ISecurityGroupPermission>()
                                {
                                    this.CreatePermission(RoleNames.ContentManager, PermissionTypes.Read),
                                    this.CreatePermission(SecurityRoleNames.SysAdmin, PermissionTypes.Read | PermissionTypes.Write)
                                }
                            },
                            new NavigationMenu()
                            {
                                Text = "Author Template",
                                Name = "AuthorTemplate",
                                Icon = "add_box",
                                Href = "/Admin/EmailTemplate/Edit",
                                Permissions = new List<ISecurityGroupPermission>()
                                {
                                    this.CreatePermission(RoleNames.ContentManager, PermissionTypes.Read),
                                    this.CreatePermission(SecurityRoleNames.SysAdmin, PermissionTypes.Read | PermissionTypes.Write)
                                }
                            }
                        }
                    },
                    }
            };
        }
    }
}