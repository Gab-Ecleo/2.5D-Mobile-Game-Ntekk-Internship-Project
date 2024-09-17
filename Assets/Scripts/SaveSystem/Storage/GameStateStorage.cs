namespace SaveSystem.Storage
{
    public class GameStateStorage
    {
        private const string SaveFile = "/gameStates.json";

        public void CreateGameStateData()
        {
            LocalStorage.Create(SaveFile);
        }

        public void DeleteGameStateData()
        {
            LocalStorage.Delete(SaveFile);
        }

        public void SaveGameStateData(GameStateData data)
        {
            LocalStorage.Write(data, SaveFile);
        }

        public GameStateData GetGameStateData()
        {
            var data = new GameStateData();
            LocalStorage.Read(ref data, SaveFile);
            return data;
        }
    }
}