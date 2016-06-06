using System;

namespace Phaka.Azure.ResourceManager.Activities
{
    public enum ProvisioningState
    {
        /// <summary>
        /// The provisioning state is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The provisioning state is accepted.
        /// </summary>
        Accepted,

        /// <summary>
        /// The provisioning state is running.
        /// </summary>
        Running,

        /// <summary>
        /// The provisioning state is creating.
        /// </summary>
        Creating,

        /// <summary>
        /// The provisioning state is created.
        /// </summary>
        Created,

        /// <summary>
        /// The provisioning state is deleting.
        /// </summary>
        Deleting,

        /// <summary>
        /// The provisioning state is deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// The provisioning state is canceled.
        /// </summary>
        Canceled,

        /// <summary>
        /// The provisioning state is failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The provisioning state is succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The provisioning state is moving resources.
        /// </summary>
        MovingResources,
    }

    public class EnumParser
    {
        public static T Parse<T>(string value) where T : struct
        {
            T result;
            if (Enum.TryParse(value, out result)) return result;

            var text = LanguageUtility.ToEnglishList<T>();
            var message = string.Format("The value '{0}' could not be parsed into a '{1}'. Valid values are '{2}'.", value,
                typeof(T), text);
            throw new InvalidOperationException(message);
        }
    }
}