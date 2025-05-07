using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void SaveFloor(int floor)
    {
        SaveData data = new SaveData { currentFloor = floor };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
        Debug.Log("Saved floor: " + floor);
    }

    public static int LoadFloor()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Loaded floor: " + data.currentFloor);
            return data.currentFloor;
        }
        else
        {
            Debug.Log("No save file found. Starting from default floor.");
            return 8; // Default floor if no save
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}
