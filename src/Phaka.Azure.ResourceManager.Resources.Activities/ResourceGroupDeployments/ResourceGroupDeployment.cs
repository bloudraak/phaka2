using System;
using System.Collections.Generic;
using Phaka.Azure.ResourceManager.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public class ResourceGroupDeployment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public ProvisioningState ProvisioningState { get; set; }
        public DateTime Timestamp { get; set; }
        public string CorrelationId { get; set; }
        public Dictionary<string, DeploymentVariable> Outputs { get; set; }

        public object GetOutputValue(string name)
        {
            if (null == Outputs)
            {
                return null;
            }
            if (!Outputs.ContainsKey(name))
            {
                return null;
            }
            var deploymentVariable = Outputs[name];
            return deploymentVariable?.Value;
        }

        public bool HasOutputValue(string name)
        {
            if (null == Outputs)
            {
                return false;
            }
            if (!Outputs.ContainsKey(name))
            {
                return false;
            }

            var deploymentVariable = Outputs[name];
            return deploymentVariable != null;
        }

        public T GetOutputValue<T>(string name)
        {
            if (null == Outputs)
            {
                return default(T);
            }
            if (!Outputs.ContainsKey(name))
            {
                return default(T);
            }
            var deploymentVariable = Outputs[name];
            if (null == deploymentVariable)
            {
                return default(T);
            }
            return (T)deploymentVariable.Value;
        }
    }
}