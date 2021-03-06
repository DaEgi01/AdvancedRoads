using System;
using System.Reflection;
using UnityEngine;
namespace PrefabIndeces {
    public static class Util {
        public static Version Take(this Version version, int fieldCount) =>
            new Version(version.ToString(fieldCount));
       
        public static Version VersionOf(Type t, int fieldCount = 4) =>
            t.Assembly.GetName().Version.Take(fieldCount);

        public static Version VersionOf(this object obj, int fieldCount = 4) =>
            VersionOf(obj.GetType(), fieldCount);

        public static void CopyProperties(object target, object origin) {
            Assert(target.GetType().IsSubclassOf(origin.GetType()));
            FieldInfo[] fields = origin.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fields) {
                //Extensions.Log($"Copying field:<{fieldInfo.Name}> ...>");
                object value = fieldInfo.GetValue(origin);
                string strValue = value?.ToString() ?? "null";
                //Extensions.Log($"Got field value:<{strValue}> ...>");
                fieldInfo.SetValue(target, value);
                //Extensions.Log($"Copied field:<{fieldInfo.Name}> value:<{strValue}>");
            }
        }

        public static void CopyProperties<T>(object target, object origin) {
            Assert(target is T, "target is T");
            Assert(origin is T, "origin is T");
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo fieldInfo in fields) {
                //Extensions.Log($"Copying field:<{fieldInfo.Name}> ...>");
                object value = fieldInfo.GetValue(origin);
                //string strValue = value?.ToString() ?? "null";
                //Extensions.Log($"Got field value:<{strValue}> ...>");
                fieldInfo.SetValue(target, value);
                //Extensions.Log($"Copied field:<{fieldInfo.Name}> value:<{strValue}>");
            }
        }

        internal static void Assert(bool con, string m = "") {
            if (!con) throw new Exception("Assertion failed: " + m);
        }

        public static ushort Clamp2UShort(int value) => (ushort)Mathf.Clamp(value, 0, ushort.MaxValue);
    }
}
