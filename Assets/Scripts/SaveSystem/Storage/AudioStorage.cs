namespace SaveSystem.Storage
{
    public class AudioStorage
    {
        private const string SaveFile = "/audioSettings.json";

        public void CreateAudioData()
        {
            LocalStorage.Create(SaveFile);
        }

        public void DeleteAudioData()
        {
            LocalStorage.Delete(SaveFile);
        }

        public void SaveAudioData(AudioData data)
        {
            LocalStorage.Write(data, SaveFile);
        }

        public AudioData GetAudioData()
        {
            var data = new AudioData();
            LocalStorage.Read(ref data, SaveFile);
            return data;
        }
    }
}