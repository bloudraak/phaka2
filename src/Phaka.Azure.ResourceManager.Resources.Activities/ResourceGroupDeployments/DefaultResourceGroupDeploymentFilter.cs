namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public class DefaultResourceGroupDeploymentFilter : IResourceGroupDeploymentFilter
    {
        public bool Match(ResourceGroupDeployment deployment)
        {
            return true;
        }
    }
}