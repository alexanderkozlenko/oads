﻿using System.Reflection;
using System.Resources;

namespace Community.Office.AddinServer.Resources
{
    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager =
            new ResourceManager(typeof(Strings).Namespace + "." + typeof(Strings).Name, typeof(Strings).GetTypeInfo().Assembly);

        public static string GetString(string name)
        {
            return _resourceManager.GetString(name);
        }
    }
}