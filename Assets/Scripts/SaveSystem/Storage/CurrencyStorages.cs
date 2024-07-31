namespace SaveSystem.Storage
{
    public class CurrencyStorage
    {
        private const string SaveFile = "/currency.json";

        public void CreateCurrencyData()
        {
            LocalStorage.Create(SaveFile);
        }

        public void DeleteCurrencyData()
        {
            LocalStorage.Delete(SaveFile);
        }

        public void SaveCurrencyData(CurrencyData data)
        {
            LocalStorage.Write(data, SaveFile);
        }

        public CurrencyData GetCurrencyData()
        {
            var data = new CurrencyData();
            LocalStorage.Read(ref data, SaveFile);
            return data;
        }
    }
}