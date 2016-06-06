using Microsoft.Azure.Management.Resources.Models;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public interface IResourceGroupFilter
    {
        bool Match(ResourceGroupExtended resourceGroup);
    }
}