using Application.Common.Interfaces;
using Domain.Enums;
using System.Linq;
using System.Security;

namespace WebApi.Services
{
    public class PermissionHelper : IPermissions
    {
        private readonly ICurrentActorService _actor;
        private readonly IConfiguration _configuration;
        public PermissionHelper(ICurrentActorService actor, IConfiguration configuration)
        {
            _actor = actor;
            _configuration = configuration;
        }
        /// <summary>
        /// It gets obejct.method in entrance and returns true if actor has access to this 
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool HasAccessInMethod(string methodName, string controllerName = "", string actionName = "")
        {
            if (CheckForGenericGlobalPermission(controllerName, actionName)) return true;

            foreach (var item in _actor.Permissions)
            {
                //split object + accesses

                //objectName.0 or 1 for CanGet.0 or 1 for CanCreate.0 or 1 for CanUpdate. or 1 for CanDelete.Methodnames
                string[] splited_permission = item.Split('.');


                //objectName.AccessType  (one of the following CanGet or CanCreate or CanUpdate or CanDelete)
                string[] splited_access = methodName.Split('.');

                if (splited_permission[0] == splited_access[0]) //same object
                {
                    if (splited_permission[5].Contains(splited_access[1])) return true;

                }
            }
            return false;

        }

        public bool HasAccessInMethod(Type t, string methodName, string controllerName = "", string actionName = "")
        {

            if (CheckForGenericGlobalPermission(controllerName, actionName)) return true;

            foreach (var item in _actor.Permissions)
            {
                //split object + accesses

                //objectName.0 or 1 for CanGet.0 or 1 for CanCreate.0 or 1 for CanUpdate. or 1 for CanDelete.Methodnames
                string[] splited_permission = item.Split('.');

                if (splited_permission[0] == t.Name) //same object
                {
                    if (splited_permission[5].Contains(methodName)) return true;

                }
            }
            return false;
        }


        public bool HasAccessInObject(string accessType, string controllerName = "", string actionName = "")
        {
            //vjen ne hyrje objekt.llojaksesi.
            //do kthehet true nqs gjendet te permission

            if (CheckForGenericGlobalPermission(controllerName, actionName)) return true;


            foreach (var item in _actor.Permissions)
            {
                //split object + accesses

                //objectName.0 or 1 for CanGet.0 or 1 for CanCreate.0 or 1 for CanUpdate. or 1 for CanDelete.Methodnames
                string[] splited_permission = item.Split('.');


                //objectName.AccessType  (one of the following CanGet or CanCreate or CanUpdate or CanDelete)
                string[] splited_access = accessType.Split('.');

                if (splited_permission[0] == splited_access[0]) //same object
                {
                    if (splited_access[1] == AccessType.CanCreate.ToString())
                    {
                        if (splited_permission[1] == "1") return true;
                    }
                    else if (splited_access[1] == AccessType.CanGet.ToString())
                    {
                        if (splited_permission[2] == "1") return true;
                    }
                    else if (splited_access[1] == AccessType.CanUpdate.ToString())
                    {
                        if (splited_permission[3] == "1") return true;
                    }
                    else if (splited_access[1] == AccessType.CanDelete.ToString())
                    {
                        if (splited_permission[4] == "1") return true;
                    }
                }

            }
            return false;
        }


        /// <summary>
        /// This method will return true 
        /// 1-if this actor has a permission that has a hasAccessToMethod named : microservice.controller.action  
        /// Example "Generic.1.1.1.0.pg_Sample:Values:Get"
        /// 2-if this actor for this microservice and this controller  has the ALL permission in any of the methods
        /// of any of the objects. 
        /// Example "Generic.1.1.1.0.pg_Sample:Values:ALL"
        /// 3-if this actor for this microservice has the ALL permission in any of the methods
        /// of any of the objects. 
        /// Example "Generic.1.1.1.0.pg_Sample:ALL"
        /// </summary>
        /// <returns></returns>
        private bool CheckForGenericGlobalPermission(string controllerName, string actionName)
        {
            foreach (var item in _actor.Permissions)
            {
                //objectName.0 or 1 for CanGet.0 or 1 for CanCreate.0 or 1 for CanUpdate. or 1 for CanDelete.Methodnames
                string[] splited_permission = item.Split('.');

                if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
                    if (splited_permission[5].Contains(_configuration["MicroserviceName"] + ":" + controllerName + ":" + actionName)) return true;

                if (!string.IsNullOrEmpty(controllerName))
                    if (splited_permission[5].Contains(_configuration["MicroserviceName"] + ":" + controllerName + ":ALL")) return true;

                if (splited_permission[5].Contains(_configuration["MicroserviceName"] + ":ALL")) return true;
            }

            return false;
        }
    }
}
