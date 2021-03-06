namespace AdaptiveRoads.Patches.metadata {
    using HarmonyLib;
    using PrefabMetadata.API;
    using PrefabMetadata.Helpers;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using KianCommons;

    [HarmonyPatch(typeof(AssetEditorRoadUtils), "Instantiate")]
    public static class Instantiate {
        /// <summary>
        /// clone list and metadata that
        /// AssetEditorRoadUtils has copied using CopyProperties()
        /// </summary>
        public static void Postfix(NetInfo template) {
            Log.Debug($"Instantiate.PostFix({template}) was called" );
            foreach (var item in template.m_nodes) {
                if (item is IInfoExtended item2)
                    item2.MetaData = item2.MetaData?.Clone();

            }
            foreach (var item in template.m_segments) {
                if (item is IInfoExtended item2)
                    item2.MetaData = item2.MetaData?.Clone();
            }
            foreach (var lane in template.m_lanes) {
                var ar = lane?.m_laneProps?.m_props;
                if (ar == null) continue;
                foreach (var item in ar ) {
                    if (item is IInfoExtended item2)
                        item2.MetaData = item2.MetaData?.Clone();
                }
            }
        }
    }
}


