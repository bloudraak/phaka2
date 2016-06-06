using System;

namespace Phaka.Azure.ResourceManager.Storage.Activities
{
    public class CustomDomain
    {
        /// <summary>Initializes a new instance of the CustomDomain class.</summary>
        public CustomDomain()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the CustomDomain class with required
        ///     arguments.
        /// </summary>
        public CustomDomain(string name)
            : this()
        {
            if (name == null)
                throw new ArgumentNullException("name");
            Name = name;
        }

        /// <summary>
        ///     Required. Gets or sets the custom domain name. Name is the CNAME
        ///     source.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Optional. Indicates whether indirect CName validation is enabled.
        ///     Default value is false. This should only be set on updates
        /// </summary>
        public bool? UseSubDomain { get; set; }
    }
}