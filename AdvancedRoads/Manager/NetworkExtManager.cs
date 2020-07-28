namespace AdvancedRoads {
    using System;
    using Util;
    using KianCommons;

    [Serializable]
    public class NetworkExtManager {
        #region LifeCycle
        public static NetworkExtManager Instance { get; private set; } = new NetworkExtManager();

        public static byte[] Serialize() => SerializationUtil.Serialize(Instance);

        public static void Deserialize(byte[] data) {
            if (data == null) {
                Instance = new NetworkExtManager();
                Log.Debug($"NodeBlendManager.Deserialize(data=null)");

            } else {
                Log.Debug($"NodeBlendManager.Deserialize(data): data.Length={data?.Length}");
                Instance = SerializationUtil.Deserialize(data) as NetworkExtManager;
            }
        }

        public void OnLoad() {
            RefreshAllNodes();
        }

        #endregion LifeCycle

        public NetNodeExt[] NodeBuffer = new NetNodeExt[NetManager.MAX_NODE_COUNT];
        public NetSegment[] SegmentBuffer = new NetNodeExt[NetManager.MAX_NODE_COUNT];
        public NetLaneExt[] LaneBuffer = new NetNodeExt[NetManager.MAX_NODE_COUNT];
        public 


        #region data tranfer
        public static byte[] CopyNodeData(ushort nodeID) =>
            Instance.CopyNodeDataImp(nodeID);

        public static void PasteNodeData(ushort nodeID, byte[] data) =>
            Instance.PasteNodeDataImp(nodeID, data);


        private byte[] CopyNodeDataImp(ushort nodeID) {
            var nodeData = buffer[nodeID];
            if (nodeData == null) {
                Log.Debug($"node:{nodeID} has no custom data");
                return null;
            }
            return SerializationUtil.Serialize(nodeData);
        }

        private void PasteNodeDataImp(ushort nodeID, byte[] data) {
            if (data == null) {
                ResetNodeToDefault(nodeID);
            } else {
                buffer[nodeID] = SerializationUtil.Deserialize(data) as NetNodeExt;
                buffer[nodeID].NodeID = nodeID;
                RefreshData(nodeID);
            }
        }
        #endregion

        public NetNodeExt InsertNode(NetTool.ControlPoint controlPoint, NodeTypeT nodeType = NodeTypeT.Crossing) {
            if(ToolBase.ToolErrors.None != NetUtil.InsertNode(controlPoint, out ushort nodeID))
                return null;
            HelpersExtensions.Assert(nodeID!=0,"nodeID");

            int nPedLanes = controlPoint.m_segment.ToSegment().Info.CountPedestrianLanes();
            if (nodeType == NodeTypeT.Crossing && nPedLanes<2)
                buffer[nodeID] = new NetNodeExt(nodeID);
            else
                buffer[nodeID] = new NetNodeExt(nodeID, nodeType);
            return buffer[nodeID];
        }

        public NetNodeExt GetOrCreate(ushort nodeID) {
            NetNodeExt data = Instance.buffer[nodeID];
            if (data == null) {
                data = new NetNodeExt(nodeID);
                buffer[nodeID] = data;
            }
            return data;
        }

        /// <summary>
        /// releases data for <paramref name="nodeID"/> if uncessary. Calls update node.
        /// </summary>
        /// <param name="nodeID"></param>
        public void RefreshData(ushort nodeID) {
            if (nodeID == 0 || buffer[nodeID] == null)
                return;
            if (buffer[nodeID].IsDefault()) {
                ResetNodeToDefault(nodeID);
            } else {
                buffer[nodeID].Refresh();
            }
        }

        public void ResetNodeToDefault(ushort nodeID) {
            if(buffer[nodeID]!=null)
                Log.Debug($"node:{nodeID} reset to defualt");
            else
                Log.Debug($"node:{nodeID} is alreadey null. no ne");
            buffer[nodeID] = null;
            NetManager.instance.UpdateNode(nodeID);
        }

        public void RefreshAllNodes() {
            foreach (var nodeData in buffer)
                nodeData?.Refresh();
        }

        public void OnBeforeCalculateNode(ushort nodeID) {
            // nodeID.ToNode still has default flags.
            if (buffer[nodeID] == null)
                return;
            if (!NetNodeExt.IsSupported(nodeID)) {
                buffer[nodeID] = null;
                return;
            }

            buffer[nodeID].Calculate();

            if (!buffer[nodeID].CanChangeTo(buffer[nodeID].NodeType)) {
                buffer[nodeID] = null;
            }
        }

        //public void ChangeNode(ushort nodeID) {
        //    Log.Info($"ChangeNode({nodeID}) called");
        //    NodeBlendData data = GetOrCreate(nodeID);
        //    data.ChangeNodeType();
        //    Instance.buffer[nodeID] = data;
        //    RefreshData(nodeID);
        //}
    }
}