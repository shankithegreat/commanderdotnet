namespace Nomad.Commons.Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class FtpMethodCache
    {
        private static Dictionary<string, object> FtpMethodMap;
        private static FieldInfo MethodInfoField;

        private static object CreateFtpMethod(string method)
        {
            Type type = typeof(FtpWebRequest);
            Assembly assembly = Assembly.GetAssembly(type);
            Type type2 = assembly.GetType("System.Net.FtpMethodInfo", false);
            Type enumType = assembly.GetType("System.Net.FtpOperation", false);
            Type type4 = assembly.GetType("System.Net.FtpMethodFlags", false);
            object obj2 = System.Enum.Parse(enumType, "Other");
            object obj3 = System.Enum.Parse(type4, "None");
            object[] args = new object[4];
            args[0] = method;
            args[1] = obj2;
            args[2] = obj3;
            return Activator.CreateInstance(type2, BindingFlags.NonPublic | BindingFlags.Instance, null, args, null);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SetFtpMethod(FtpWebRequest request, string method)
        {
            object obj2;
            if (FtpMethodMap == null)
            {
                FtpMethodMap = new Dictionary<string, object>();
                FtpMethodMap.Add("APPE", null);
                FtpMethodMap.Add("DELE", null);
                FtpMethodMap.Add("RETR", null);
                FtpMethodMap.Add("MDTM", null);
                FtpMethodMap.Add("SIZE", null);
                FtpMethodMap.Add("NLST", null);
                FtpMethodMap.Add("LIST", null);
                FtpMethodMap.Add("MKD", null);
                FtpMethodMap.Add("PWD", null);
                FtpMethodMap.Add("RMD", null);
                FtpMethodMap.Add("RENAME", null);
                FtpMethodMap.Add("STOR", null);
                FtpMethodMap.Add("STOU", null);
            }
            if (!FtpMethodMap.TryGetValue(method, out obj2))
            {
                obj2 = CreateFtpMethod(method);
                FtpMethodMap.Add(method, obj2);
            }
            if (obj2 == null)
            {
                request.Method = method;
            }
            else
            {
                if (MethodInfoField == null)
                {
                    MethodInfoField = typeof(FtpWebRequest).GetField("m_MethodInfo", BindingFlags.NonPublic | BindingFlags.Instance);
                }
                MethodInfoField.SetValue(request, obj2);
            }
        }
    }
}

