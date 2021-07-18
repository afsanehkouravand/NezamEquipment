using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Framework.Security
{
    public sealed class GetAllAreaControllerAction
    {
        private static volatile GetAllAreaControllerAction _instance;
        private static readonly object Lock = new object();

        private GetAllAreaControllerAction()
        {
            var id = 0;

            Tree = GetAllAreaWithController();
            List = new List<Auth>();

            foreach (var area in Tree)
            {
                foreach (var controller in area.Controllers)
                {
                    foreach (var action in controller.Actions)
                    {
                        if (!string.IsNullOrWhiteSpace(action.IsDuplicateTo))
                        {
                            //
                        }
                        List.Add(new Auth()
                        {
                            Id = ++id,
                            ActionName = action.Name,
                            ActionTitle = action.Title,
                            ControllerName = controller.Name,
                            ControllerTitle = controller.Title,
                            AreaName = area.Name,
                            AreaTitle = area.Title,
                            IsDuplicateTo = action.IsDuplicateTo,
                        });
                    }
                }
            }
        }

        public IList<AreaAuth> Tree { get; set; }
        public IList<Auth> List { get; set; }

        public static GetAllAreaControllerAction GetInstance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GetAllAreaControllerAction();
                    }
                }
            }

            return _instance;
        }

        #region Private Method

        private static List<AreaAuth> GetAllAreaWithController()
        {
            var listOfAreas = GetArea();

            var listofController = GetController();

            foreach (var area in listOfAreas)
            {
                area.Controllers = area.Namespaces.Select(
                        item => listofController.FirstOrDefault(x => x.NameSpace.ToLower() == item.ToLower()))
                    .Where(x => x != null)
                    .ToList();
            }

            listOfAreas = listOfAreas.OrderBy(x => x.Order).ToList();
            foreach (var listOfArea in listOfAreas)
            {
                listOfArea.Controllers = listOfArea.Controllers.OrderBy(x => x.Order).ToList();
            }

            return listOfAreas;
        }

        private static List<AreaAuth> GetArea()
        {
            var currentAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == "NezamEquipment.Web")
                ?.GetTypes();

            if (currentAssembly == null)
            {
                return new List<AreaAuth>();
            }

            var listOfArea = currentAssembly.Where(
                    type => type.GetCustomAttributes(typeof(AuthorizeAreaNameAttribute)).Any() &&
                            type.GetCustomAttributes(typeof(AuthorizeAreaNameAttribute)).Any())
                .Select(x => new AreaAuth()
                {
                    Name = x.Name.Replace("AreaRegistration", ""),
                    Title = x.GetCustomAttribute<AuthorizeAreaNameAttribute>().Name,
                    Order = x.GetCustomAttribute<AuthorizeAreaNameAttribute>().Order,
                    Namespaces = new List<string>(),
                    Controllers = new List<ControllerAuth>()
                })
                .ToList();

            // list of all routes that have been registerd
            var allRoute =
                RouteTable.Routes.OfType<Route>()
                    .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                    .ToList();

            // geting namespaces that the route has registerd for area
            foreach (var item in allRoute)
            {
                var name = item.DataTokens["area"].ToString();
                var l = listOfArea.FirstOrDefault(x => x.Name == name);
                if (l != null)
                {
                    var d = item.DataTokens["Namespaces"] as IList<string>;
                    if (d != null && d.Any()) l.Namespaces.AddRange(d);
                }
            }

            // fix namespace 
            foreach (var item in listOfArea)
            {
                item.Namespaces = item.Namespaces.Distinct().ToList();
            }

            return listOfArea;
        }

        private static IEnumerable<ControllerAuth> GetController()
        {
            var currentAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == "NezamEquipment.Web")
                ?.GetTypes();

            if (currentAssembly == null)
            {
                return new List<ControllerAuth>();
            }

            return currentAssembly.Where(
                type =>
                    type.IsSubclassOf(typeof(Controller)) &&
                    type.GetCustomAttributes(typeof(AuthorizeControllerNameAttribute)).Any() &&
                    !type.Name.StartsWith("T4MVC_")).Select(x => new
                    {
                        model = x,
                        attr = x.GetCustomAttribute<AuthorizeControllerNameAttribute>(),
                    }).Select(x => new ControllerAuth()
                    {
                        Name = x.model.Name.Replace("Controller", ""),
                        NameSpace = x.model.Namespace,
                        Title = x.attr.Name,
                        Order = x.attr.Order,
                        Actions = GetAction(x.model),
                    }).ToList();
        }

        private static List<ActionAuth> GetAction(Type controller)
        {
            var navItems = new List<ActionAuth>();

            // Get a descriptor of this controller
            var controllerDesc = new ReflectedControllerDescriptor(controller);

            // Look at each action in the controller
            foreach (var action in controllerDesc.GetCanonicalActions())
            {
                var persianName = string.Empty;
                var description = string.Empty;
                var isDuplicateTo = string.Empty;
                var orderBy = 0;

                // Get any attributes (filters) on the action
                var attributes = action.GetCustomAttributes(false);

                // Look at each attribute
                foreach (var filter in attributes)
                {
                    var attribute = filter as AuthorizeActionNameAttribute;
                    if (attribute != null)
                    {
                        persianName = attribute.Name;
                        description = attribute.Description;
                        isDuplicateTo = attribute.IsDuplicateTo;
                        orderBy = attribute.Order;
                    }
                }

                if (string.IsNullOrWhiteSpace(persianName) && string.IsNullOrWhiteSpace(isDuplicateTo))
                {
                    continue;
                }

                // Add the action to the list if it's "valid"
                navItems.Add(new ActionAuth()
                {
                    Name = action.ActionName,
                    Title = persianName,
                    Description = description,
                    IsDuplicateTo = isDuplicateTo,
                    OrderBy = orderBy,
                });
            }

            return navItems.OrderBy(x => x.OrderBy).ToList();
        }

        #endregion

        #region Classes

        public class Auth
        {
            public int Id { get; set; }

            public string AreaName { get; set; }
            public string AreaTitle { get; set; }

            public string ControllerName { get; set; }
            public string ControllerTitle { get; set; }

            public string ActionName { get; set; }
            public string ActionTitle { get; set; }

            public string IsDuplicateTo { get; set; }
        }

        public class AreaAuth
        {
            public string Name { get; set; }

            public string Title { get; set; }

            public int Order { get; set; }

            public List<string> Namespaces { get; set; }

            public List<ControllerAuth> Controllers { get; set; }
        }

        public class ControllerAuth
        {
            public string Name { get; set; }

            public string Title { get; set; }

            public string NameSpace { get; set; }

            public string Order { get; set; }

            public List<ActionAuth> Actions { get; set; }
        }

        public class ActionAuth
        {
            public string Name { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public string IsDuplicateTo { get; set; }

            public int OrderBy { get; set; }
        }

        #endregion

    }
}