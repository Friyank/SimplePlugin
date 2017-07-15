using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlugin
{
    public class TriggerTaskCreationOnAccount : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService traceObj = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            try
            {
                traceObj.Trace("Plugin Starts");

                if (pluginContext.InputParameters.Contains("Target") && pluginContext.InputParameters["Target"] is Entity)
                {
                    Entity ExecutingEntity = (Entity)pluginContext.InputParameters["Target"];
                    if (ExecutingEntity.LogicalName != "account")
                        return;

                    Entity taskEntity = new Entity("task");
                    taskEntity["subject"] = "Send e-mail to the new customer.";
                    taskEntity["description"] =
                        "Follow up with the customer. Check if there are any new issues that need resolution.";
                    taskEntity["scheduledstart"] = DateTime.Now.AddDays(7);
                    taskEntity["scheduledend"] = DateTime.Now.AddDays(7);
                    taskEntity["category"] = pluginContext.PrimaryEntityName;

                    if (pluginContext.OutputParameters.Contains("id"))
                    {
                        Guid regardingobjectid = new Guid(pluginContext.OutputParameters["id"].ToString());
                        string regardingobjectidType = "account";

                        taskEntity["regardingobjectid"] =
                        new EntityReference(regardingobjectidType, regardingobjectid);
                    }

                    IOrganizationService CallD365 = serviceFactory.CreateOrganizationService(pluginContext.UserId);


                    traceObj.Trace("Task is getting Created for Account id : " + pluginContext.PrimaryEntityId);
                    CallD365.Create(taskEntity);
                }


                traceObj.Trace("Plugin Ends With No Excepption");
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the plug-in.", ex);
            }
            catch (Exception ex)
            {
                traceObj.Trace("Plugin Exception : {0}", ex.ToString());
                traceObj.Trace("Plugin Ends With No Excepption");
            }
        }
    }
}