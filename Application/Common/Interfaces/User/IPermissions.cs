using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPermissions
    {
        bool HasAccessInObject(string accessType, string controllerName, string actionName);
        bool HasAccessInMethod(string methodName, string controllerName, string actionName);
        bool HasAccessInMethod(Type t, string methodName, string controllerName, string actionName);
    }
}
