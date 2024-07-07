using System.IO;
using UnityEngine;

namespace Save
{
    public static class SaveManager
    {
        public static void Save()
        {
            var saveData = new SaveData
            {
                sapphire = Player.Instance.sapphire,
                timesExtraLifeBought = Player.Instance.timesExtraLifeBought,
                timesBetterLootBought = Player.Instance.timesBetterLootBought
            };
            var dataJson = JsonUtility.ToJson(saveData);
            File.WriteAllText(FilePath(), dataJson);
        }

        public static void Load()
        {
            if (!File.Exists(FilePath())) return;
            var dataJson = File.ReadAllText(FilePath());
            var saveData = JsonUtility.FromJson<SaveData>(dataJson);

            Player.Instance.sapphire = saveData.sapphire;
            Player.Instance.timesExtraLifeBought = saveData.timesExtraLifeBought;
            Player.Instance.timesBetterLootBought = saveData.timesBetterLootBought;
        }

        private static string FilePath()
        {
            return Path.Combine(Application.persistentDataPath, "save.dat");
        }
    }
}