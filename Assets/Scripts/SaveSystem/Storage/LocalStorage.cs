using UnityEngine;

namespace SaveSystem.Storage
{
    public static class LocalStorage
    {    
        
        private static string _buildPath(string path)
        {
            return Application.persistentDataPath + path;
        }
        
        private static void _checkFile(string path)
        {   
            if (System.IO.File.Exists(_buildPath(path)))
            {
                return;
     
            }
            
            // Create a new file when it doesn't exist
            Debug.Log($"{path} doesn't exist, creating a new one");
            System.IO.File.Create(_buildPath(path));
        }
        public static void Write<T>(T data, string path)
        {
            _checkFile(path);
            string json = JsonUtility.ToJson(data, true);
            System.IO.File.WriteAllText(_buildPath(path), json);
        }
        
        public static void Read<T>(ref T data, string path)
        {
            _checkFile(path);
            string json = System.IO.File.ReadAllText(_buildPath(path));
            data = JsonUtility.FromJson<T>(json);
        }
        
        public static void Delete(string path)
        {
            System.IO.File.Delete(_buildPath(path));
        }

        public static void Create(string path)
        {
           _checkFile(path);
        }
    }
}