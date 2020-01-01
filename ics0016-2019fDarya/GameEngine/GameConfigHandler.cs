using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GameEngine
{
    public static class GameConfigHandler
    {
//        private const string FileName = "gamesettings.json";
//        
//        private const string GameSavePath =
//            "C:\\Users\\Daria\\RiderProjects\\charp2019fall\\Connect4\\Connect4\\bin\\Debug\\netcoreapp3.0\\saved_games";

//        public static void SaveConfig(GameSettings settings, string fileName = FileName)
//        {
//            using (var write = System.IO.File.CreateText(fileName))
//            {
//                var jsonString = JsonConvert.SerializeObject(settings);
//                write.Write(jsonString);
//            }
//        }

//        public static GameSettings LoadConfig(string fileName = FileName)
//        {
//            if (File.Exists(fileName))
//            {
//                var jsonString = File.ReadAllText(fileName);
//                var res = JsonConvert.DeserializeObject<GameSettings>(jsonString);
//                return res;
//            }
//            
//            return new GameSettings();
//        }

        /*public static void SaveGame(GameSettings settings)
        {
            Directory.CreateDirectory(GameSavePath);
            
            var fileName = GameSavePath + "\\" + settings.SaveName;

            using (var write = File.CreateText(fileName))
            {
                var jsonString = JsonConvert.SerializeObject(settings);
                write. Write(jsonString);
            }
        }*/

        /*public static List<string> GetSavedGames()
        {
            DirectoryInfo gameFolder = new DirectoryInfo(GameSavePath);
            FileInfo[] files = gameFolder.GetFiles("*.json");
            
            List<string> savedGames = new List<string>();
            
            foreach(FileInfo file in files )
            {
                string[] fullNames = file.Name.Split(".");
                savedGames.Add(fullNames[0]);
            }
            
            return savedGames;
        }*/
    }
}
