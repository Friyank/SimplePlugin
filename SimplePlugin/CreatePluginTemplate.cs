using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlugin
{
    public class CreatePluginTemplate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService traceObj = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            try
            {
                traceObj.Trace("Plugin Starts");


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