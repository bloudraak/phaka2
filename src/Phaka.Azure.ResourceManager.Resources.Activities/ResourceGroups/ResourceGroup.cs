using System.Collections.Generic;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public class ResourceGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IDictionary<string, string> Tags { get; set; }
        public string ProvisioningState { get; set; }
    }
}