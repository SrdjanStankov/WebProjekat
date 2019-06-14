﻿using ProjektniZadatak.Controllers;
using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml;
using System.Xml.Serialization;

namespace ProjektniZadatak
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoadAdministrators();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void LoadAdministrators()
        {
            var serializer = new XmlSerializer(typeof(List<Administrator>));
            var stream = new FileStream(Server.MapPath("Admins.xml"), FileMode.OpenOrCreate);

            using (var reader = XmlReader.Create(stream))
            {
                var user = serializer.Deserialize(reader) as List<Administrator>;
                AddAdminsToDatabaseIfNecesary(user);
            }
            CasheUsersFromDatabase();
        }

        private void CasheUsersFromDatabase()
        {
            using (var model = new Model())
            {
                UserController.RegisteredUsers.AddRange(model.GetAllUsers());
            }
        }

        private static void AddAdminsToDatabaseIfNecesary(List<Administrator> user)
        {
            using (var model = new Model())
            {
                var allUsers = model.GetAllUsers();

                foreach (var item in user)
                {
                    if (!allUsers.Exists(u => u.Username != item.Username))
                    {
                        model.AddUser(item);
                    }
                }
            }
        }
    }
}