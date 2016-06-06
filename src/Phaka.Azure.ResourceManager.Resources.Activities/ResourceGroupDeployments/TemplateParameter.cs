using Newtonsoft.Json;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroupDeployments
{
    public class TemplateParameter
    {
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}