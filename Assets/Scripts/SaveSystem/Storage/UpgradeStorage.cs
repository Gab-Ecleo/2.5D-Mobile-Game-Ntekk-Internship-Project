namespace SaveSystem.Storage
{
    public class UpgradeStorage
    {
        private const string SaveFile = "/upgrade.json";

        public void CreateUpgradeData()
        {
            LocalStorage.Create(SaveFile);
        }

        public void DeleteUpgradeData()
        {
            LocalStorage.Delete(SaveFile);
        }

        public void SaveUpgradeData(UpgradeData data)
        {
            LocalStorage.Write(data, SaveFile);
        }

        public UpgradeData GetUpgradeData()
        {
            var data = new UpgradeData();
            LocalStorage.Read(ref data, SaveFile);
            return data;
        }
    }
}