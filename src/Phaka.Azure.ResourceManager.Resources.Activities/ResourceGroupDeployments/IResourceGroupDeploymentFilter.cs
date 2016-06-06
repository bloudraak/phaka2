namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public interface IResourceGroupDeploymentFilter
    {
        bool Match(ResourceGroupDeployment deployment);
    }
}