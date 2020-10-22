namespace PrefabMetadata.Helpers {
    using PrefabMetadata.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class PrefabMetadataHelpers {
        /// <summary>
        /// returns the latest version of PrefabMetadata.dll in the app domain
        /// </summary>
        public static Assembly GetLatestAssembly() {
            Assembly ret = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (assembly.GetName().Name != "PrefabMetadata")
                    continue;
                if (ret == null || ret.GetName().Version < assembly.GetName().Version) {
                    ret = assembly;
                }
            }
            return ret;
        }

        /// <summary>
        /// returns an extended clone of <paramref name="info"/>
        /// that can accept metadata.
        /// </summary>
        public static IInfoExtended<NetInfo.Segment> Extend(this NetInfo.Segment info) {
            MethodInfo m =
                 GetLatestAssembly()
                .GetType(nameof(NetInfoMetaDataExtension.Segment))
                .GetMethod(nameof(NetInfoMetaDataExtension.Segment.Extend));
            return m.Invoke(null, new[] { info }) as IInfoExtended<NetInfo.Segment>;
        }

        /// <summary>
        /// returns an extended clone of <paramref name="info"/>
        /// that can accept metadata.
        /// </summary>
        public static IInfoExtended<NetInfo.Node> Extend(this NetInfo.Node info) {
            MethodInfo m =
                 GetLatestAssembly()
                .GetType(nameof(NetInfoMetaDataExtension.Node))
                .GetMethod(nameof(NetInfoMetaDataExtension.Node.Extend));
            return m.Invoke(null, new[] { info }) as IInfoExtended<NetInfo.Node>;
        }

        /// <summary>
        /// returns an extended clone of <paramref name="info"/>
        /// that can accept metadata.
        /// </summary>
        public static IInfoExtended<NetLaneProps.Prop> Extend(this NetLaneProps.Prop info) {
            MethodInfo m =
                 GetLatestAssembly()
                .GetType(nameof(NetInfoMetaDataExtension.Prop))
                .GetMethod(nameof(NetInfoMetaDataExtension.Prop.Extend));
            return m.Invoke(null, new[] { info }) as IInfoExtended<NetLaneProps.Prop>;
        }

        public static MetaDataType GetMetaData<MetaDataType>(this IInfoExtended info)
            where MetaDataType : class {
            if (info.MetaData != null) {
                foreach (var item in info.MetaData) {
                    if (item is MetaDataType ret)
                        return ret;
                }
            }
            return null;
        }

        /// <summary>
        /// addas <paramref name="data"/> to <paramref name="info"/>.
        /// if <paramref name="info"/> already has a meta data of the same type, it will be replaced.
        /// </summary>
        public static void SetMetaData<MetaDataType>(this IInfoExtended info, MetaDataType data)
            where MetaDataType : class, ICloneable {
            if (data == null)
                return;
            if (info.MetaData == null)
                info.MetaData = new List<ICloneable>();
            var list = info.MetaData;
            for (int i = 0; i < list.Count; ++i) {
                if (list[i] is MetaDataType) {
                    list[i] = data;
                }
            }
            list.Add(data);
        }

        public static List<ICloneable> Clone(this List<ICloneable> list) {
            var ret = new List<ICloneable>(list);
            for (int i = 0; i < list.Count; ++i) {
                list[i] = list[i].Clone() as ICloneable;
            }
            return ret;
        }
    }
}
