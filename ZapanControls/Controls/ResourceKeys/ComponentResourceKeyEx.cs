using System;
using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ComponentResourceKeyEx
    {
        /// <summary>
        /// Return ResourceKey from resourceId
        /// </summary>
        /// <param name="resKey"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public static ComponentResourceKey GetRegisteredKey(this ComponentResourceKey resKey, Type ownerType, string resourceId)
        {
            return resKey ?? new ComponentResourceKey(ownerType, resourceId);
        }
    }
}
