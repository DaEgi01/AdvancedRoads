namespace AdaptiveRoads.LifeCycle
{
    using JetBrains.Annotations;
    using ICities;
    using System;
    using AdaptiveRoads.Manager;

    [UsedImplicitly]
    public class SerializableDataExtension
        : SerializableDataExtensionBase
    {
        private const string DATA_ID = "AdaptiveRoads_V1.0";

        public override void OnLoadData()
        {
            byte[] data = serializableDataManager.LoadData(DATA_ID);
            NetworkExtensionManager.Deserialize(data, new Version(1,0));
        }

        public override void OnSaveData()
        {
            byte[] data = NetworkExtensionManager.Serialize();
            serializableDataManager.SaveData(DATA_ID, data);
        }
    }
}
