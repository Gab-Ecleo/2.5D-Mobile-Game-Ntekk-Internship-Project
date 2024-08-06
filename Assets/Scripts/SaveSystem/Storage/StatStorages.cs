namespace SaveSystem.Storage
{
    public class StatStorages
    {
        private const string SaveFile = "/stats.json";

        public void CreateStatData()
        {
            LocalStorage.Create(SaveFile);
        }

        public void DeleteStatData()
        {
            LocalStorage.Delete(SaveFile);
        }

        public void SaveStatData(StatData data)
        {
            LocalStorage.Write(data, SaveFile);
        }

        public StatData GetStatData()
        {
            var data = new StatData();
            LocalStorage.Read(ref data, SaveFile);
            return data;
        }
    }
}