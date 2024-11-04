using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Filters
{
    public class CustomPermissionAttribute : TypeFilterAttribute
    {
        public CustomPermissionAttribute(Type tip_objekti, PermissionType pt, AccessType[] permissionList) : base(typeof(CustomPermissionFilter))
        {
            List<string> akseset = new List<string>();
            var n = tip_objekti.Name;
            foreach (var permission in permissionList)
            {
                akseset.Add(n + "." + pt.ToString() + "." + permission.ToString());
            }
            Arguments = new object[] { akseset.ToArray() };
        }

        public CustomPermissionAttribute(Type tip_objekti, PermissionType pt, string[] methodNames) : base(typeof(CustomPermissionFilter))
        {

            List<string> akseset = new List<string>();
            var n = tip_objekti.Name;
            foreach (var method in methodNames)
            {
                akseset.Add(n + "." + pt.ToString() + "." + method);
            }
            Arguments = new object[] { akseset.ToArray() };

        }

        public CustomPermissionAttribute(Type tip_objekti, AccessType[] permissionList, string[] methodNames = null) : base(typeof(CustomPermissionFilter))
        {
            List<string> akseset = new List<string>();
            var n = tip_objekti.Name;
            foreach (var permission in permissionList)
            {
                akseset.Add(n + "." + PermissionType.Access.ToString() + "." + permission.ToString());
            }
            if (methodNames != null)
                foreach (var method in methodNames)
                {
                    akseset.Add(n + "." + PermissionType.Method.ToString() + "." + method);
                }

            Arguments = new object[] { akseset.ToArray() };
        }

        public CustomPermissionAttribute(Type tip_objekti, string[] methodNames) : base(typeof(CustomPermissionFilter))
        {
            List<string> akseset = new List<string>();
            var n = tip_objekti.Name;
            foreach (var method in methodNames)
            {
                akseset.Add(n + "." + PermissionType.Method.ToString() + "." + method);
            }
            Arguments = new object[] { akseset.ToArray() };
        }
    }

}
