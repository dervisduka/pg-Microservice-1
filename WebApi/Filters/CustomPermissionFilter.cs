using Application.Common.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class CustomPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IPermissions _permissionHelper;
        private readonly string[] _akseset;

        /// <summary>
        /// kontrollon aksesin ne objekt dhe ne metoda per objekt
        /// Parametri ne hyrje _akseset eshte nje liste stringjes qe ka dy trajta
        /// 1-EmerObjekti.llojpermissioni.permisison . Psh Product.Access.CanGet
        /// 2-EmerObjekti.llojpermissioni.Metoda.  Psh Product.Method.Metoda1
        /// 
        /// per keto dy trajta ne varesi te llojit therritet permissionHelper qe kthen true ose false
        /// metoda do autorizoje vetem nqs nuk ka asnje false
        /// </summary>
        /// <param name="akseset"></param>
        /// <param name="permissionHelper"></param>
        public CustomPermissionFilter(string[] akseset, IPermissions permissionHelper)
        {
            _permissionHelper = permissionHelper;
            _akseset = akseset;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ObjectResult("Not authenticated!") { StatusCode = 401 };
            }

            ControllerActionDescriptor? contentDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string objekti, menyra, vleraPermissionit;
            bool kaAkses = true;
            string ReturnMessage = "";
            foreach (var aksesi in _akseset)
            {
                objekti = aksesi.Split(".")[0];
                menyra = aksesi.Split(".")[1];
                vleraPermissionit = aksesi.Split(".")[2];

                if (menyra == PermissionType.Access.ToString())
                {
                    if (!_permissionHelper.HasAccessInObject(objekti + "." + vleraPermissionit, contentDescriptor != null ? contentDescriptor.ControllerName : "", contentDescriptor != null ? contentDescriptor.ActionName : ""))
                    {
                        kaAkses = false;
                        ReturnMessage += "Authorization fail: No access in '" + objekti + "' for permission '" + vleraPermissionit + "'!" + Environment.NewLine;
                    }

                }
                else if (menyra == PermissionType.Method.ToString())
                {
                    if (!_permissionHelper.HasAccessInMethod(objekti + "." + vleraPermissionit, contentDescriptor != null ? contentDescriptor.ControllerName : "", contentDescriptor != null ? contentDescriptor.ActionName : ""))
                    {
                        kaAkses = false;
                        ReturnMessage += "Authorization fail: No access in '" + objekti + "' for method '" + vleraPermissionit + "'!" + Environment.NewLine;
                    }
                }
            }

            if (!kaAkses)
            {
                context.Result = new ObjectResult(ReturnMessage) { StatusCode = 403 };
            }
        }

    }
}
