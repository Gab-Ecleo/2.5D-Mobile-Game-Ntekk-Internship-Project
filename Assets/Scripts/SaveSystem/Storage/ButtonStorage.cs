namespace SaveSystem.Storage
{
    public class ButtonStorage
    {
        private const string SaveFile = "/buttons.json";

        public void CreateButtonData()
        {
            LocalStorage.Create(SaveFile);
        }

        public void DeleteButtonData()
        {
            LocalStorage.Delete(SaveFile);
        }

        public void SaveButtonData(ButtonData data)
        {
            LocalStorage.Write(data, SaveFile);
        }

        public ButtonData GetButtonData()
        {
            var data = new ButtonData();
            LocalStorage.Read(ref data, SaveFile);
            return data;
        }
    }
}