using UnityEngine;
using System.IO;

public class ArmourManager : MonoBehaviour {
    public string chest = "none";
    public string legs = "none";
    public string head = "none";

    private string savePath;

    private void Start() {
        savePath = Application.persistentDataPath + "/armour.json";
        Debug.Log("Saving game data to: " + savePath);
    }

    [ContextMenu("Save")]
    public void Save() {
        Armour armour = new Armour(this);
        string json = JsonUtility.ToJson(armour);
        File.WriteAllText(savePath, json);
    }

    [ContextMenu("Load")]
    public void Load() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            Armour armour = JsonUtility.FromJson<Armour>(json);
            this.chest = armour.chest;
            this.legs = armour.legs;
            this.head = armour.head;
        } else {
            Debug.Log("Unable to load file: " + savePath);
        }
    }
}
