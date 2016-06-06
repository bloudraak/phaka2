using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Azure.Management.Resources.Models;

namespace Phaka.Azure.ResourceManager.Resources.Activities.ResourceGroups
{
    public class ResourceGroupFilter : IResourceGroupFilter
    {
        private readonly HashSet<string> _locations;
        private readonly Regex _nameRegex;

        public ResourceGroupFilter(string namePattern, IEnumerable<string> locations)
        {
            _locations = new HashSet<string>(locations);
            if (!string.IsNullOrWhiteSpace(namePattern))
            {
                _nameRegex = new Regex(namePattern);
            }
        }

        public bool Match(ResourceGroupExtended resourceGroup)
        {
            return NameMatches(resourceGroup) || LocationMatches(resourceGroup);
        }

        private bool LocationMatches(ResourceGroupExtended resourceGroup)
        {
            return _locations.Count == 0 || _locations.Contains(resourceGroup.Location);
        }

        private bool NameMatches(ResourceGroupExtended resourceGroup)
        {
            return _nameRegex == null || _nameRegex.IsMatch(resourceGroup.Name);
        }
    }
}