using ColossalFramework;
using HarmonyLib;

namespace AdaptiveRoads.Patches.Segment {
    
    //[HarmonyPatch(typeof(NetNode), nameof(NetNode.CalculateNode))]
    class CalculateSegment {
        static void Postfix(ref NetNode __instance) {
            //Log.Debug("CalculateNode.PostFix() was called");
            //ushort nodeID = NetUtil.GetID(__instance);
            //if (!NetUtil.IsNodeValid(nodeID)) return;
            //NetworkExtensionManager.Instance.OnBeforeCalculateNode(nodeID);
        } // end postfix
    }
}