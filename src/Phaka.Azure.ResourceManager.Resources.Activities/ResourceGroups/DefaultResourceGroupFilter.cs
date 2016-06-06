using Microsoft.Azure.Management.Resources.Models;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public class DefaultResourceGroupFilter : IResourceGroupFilter
    {
        public bool Match(ResourceGroupExtended resourceGroup)
        {
            return true;
        }
    }
}