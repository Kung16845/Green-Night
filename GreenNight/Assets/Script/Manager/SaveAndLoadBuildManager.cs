using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
public class SaveAndLoadBuildManager : MonoBehaviour
{
    [Header("Data Build")]
    public List<Building> listAllBuild;
    [Header("Saving Path")]
    [SerializeField] string saveBuildDataPath;
    // Start is called before the first frame update
    public void SaveBuildInScenes()
    {
        if (string.IsNullOrEmpty(saveBuildDataPath))
        {
            Debug.LogError("No save path specified");
            return;
        }
        // รอเขียน class เก็บข้อมูล Build ที่สร้าง
        // var dataBuildInSceans = JsonConvert.SerializeObject(this.dataObjectInSceans, Formatting.Indented); // Serialize the data with pretty print
        var dataPath = Application.dataPath;
        var targetFilePath = Path.Combine(dataPath, saveBuildDataPath);

        var directoryPath = Path.GetDirectoryName(targetFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // File.WriteAllText(targetFilePath, dataBuildInSceans);
    }
    public void LoadBuildInScenes()
    {
        var dataPath = Application.dataPath;
        var targetFilePath = Path.Combine(dataPath, saveBuildDataPath);

        var directoryPath = Path.GetDirectoryName(targetFilePath);

        if (Directory.Exists(directoryPath) == false)
        {
            Debug.LogError("No save folder at provided path");
            return;
        }

        if (File.Exists(targetFilePath) == false)
        {
            Debug.LogError("No save file at provided path");
            return;
        }

        var dataJson = File.ReadAllText(targetFilePath);
        // dataObjectInSceans = JsonConvert.DeserializeObject<List<DataObjectLoad>>(dataJson);
    }
}
