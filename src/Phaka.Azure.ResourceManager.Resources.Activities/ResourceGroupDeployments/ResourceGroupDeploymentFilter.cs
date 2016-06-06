using System.Collections.Generic;
using System.Text.RegularExpressions;
using Phaka.Azure.ResourceManager.Activities;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public class ResourceGroupDeploymentFilter : IResourceGroupDeploymentFilter
    {
        private readonly Regex _regex;
        private readonly HashSet<ProvisioningState> _provisioningStates;

        public ResourceGroupDeploymentFilter(string namePattern, IEnumerable<ProvisioningState> provisioningStates)
        {
            if (!string.IsNullOrWhiteSpace(namePattern))
            {
                _regex = new Regex(namePattern);
            }

            if(provisioningStates == null)
                provisioningStates = new ProvisioningState[0];

            this._provisioningStates = new HashSet<ProvisioningState>(provisioningStates);
        }

        public bool Match(ResourceGroupDeployment deployment)
        {
            return MatchName(deployment.Name) || MatchProvisionState(deployment.ProvisioningState);
        }

        private bool MatchProvisionState(ProvisioningState provisioningState)
        {
            // when there are no states, it means we don't want to match it
            var states = _provisioningStates;
            return states.Count == 0 && states.Contains(provisioningState);
        }

        private bool MatchName(string name)
        {
            return _regex == null || _regex.IsMatch(name);
        }
    }
}