using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour {
    
    public float defaultTrashCash = 500f;
    public Levels[] level;

    public bool isDebugging = true;

    private string filename;
    private string path;
    private string file;

    private DataFile _dataFile;
    public DataFile dataFile {
        get { return _dataFile; }
        set {
            _dataFile = value;

            string output = JsonUtility.ToJson(_dataFile);

            File.WriteAllText(file, output);
        }

    }


    private string[] itemName = new string[] { "Heart", "Phone", "Sheild", "Magnet", "Sweeper", "Bomb", "GearsOfWar" };
    private int[] itemPrice = new int[] { 300, 300, 100, 50, 600, 900, 850 };


    void Awake() {

        Debugging();
        InitData();

    }

    void Debugging() {
        if (isDebugging) {
            filename = "/DataFile.json";
            path = Application.persistentDataPath;
            file = path + filename;

            if (File.Exists(file)) {
                File.Delete(file);
            }
        }
    }

    void InitData() {
        filename = "/DataFile.json";
        path = Application.persistentDataPath;
        file = path + filename;

        if (File.Exists(file)) {
            string output = File.ReadAllText(file);

            _dataFile = JsonUtility.FromJson<DataFile>(output);
        }
        else {

            _dataFile = new DataFile {
                trash_cash = defaultTrashCash,
                music = true,
                first_run = true,
                currentLevel = 1,
                level = level,
                character = 0,
                items = new List<Inventory>()
            };

            for (int i = 0; i < itemName.Length; i++) {

                Inventory inventory;
                inventory.item_name = itemName[i];
                inventory.item_count = 0;
                inventory.item_price = itemPrice[i];
            
                _dataFile.items.Add(inventory);

            }

            string output = JsonUtility.ToJson(_dataFile);

            File.WriteAllText(file, output);
        }
    }


    /*public DataFile GetData() {
        return dataFile;
    }

    public void SetData(DataFile dataFile) {
        this.dataFile = dataFile;
    }

    public bool GetFirstRun() {
        return dataFile.first_run;

    }

    public void WriteFirstRun(bool run) {
        dataFile.first_run = run;

        string output = JsonUtility.ToJson(dataFile);

        File.WriteAllText(file, output);
    }

    public float GetTrashCash() {
        return dataFile.trash_cash;
    }

    public bool GetMusic() {
        return dataFile.music;
    }

    public List<Inventory> GetAllItems() {
        return dataFile.items;
    }
    public Inventory GetOneItemByIndex(int index) {
        return dataFile.items[index];
    }

    public Inventory GetOneInventoryByName(string name) {
        
        for (int i = 0; i < dataFile.items.Count; i++) {
            if (name == dataFile.items[i].item_name)
                return dataFile.items[i];
        }

        return new Inventory();
    }

    public Inventory GetOneInventoryByName(string name, out int index) {

        index = -1;

        for (int i = 0; i < dataFile.items.Count; i++) {
            if (name == dataFile.items[i].item_name) {
                index = i;
                return dataFile.items[i];
            }
        }

        return new Inventory();
    }

    public void WriteTrashCash(float trash) {
        dataFile.trash_cash = trash;

        string output = JsonUtility.ToJson(dataFile);

        File.WriteAllText(file, output);
    }

    public void WriteMusic(bool music) {
        dataFile.music = music;

        string output = JsonUtility.ToJson(dataFile);

        File.WriteAllText(file, output);
    }

    public void WriteAllInventories(List<Inventory> inventories) {
        dataFile.items = inventories;

        string output = JsonUtility.ToJson(dataFile);

        File.WriteAllText(file, output);
    }

    public void WriteOneItemByIndex(Inventory item, int index) {
        dataFile.items[index] = item;
    }

    public void WriteOneItemByName(Inventory item, string name) {
        for (int i = 0; i < dataFile.items.Count; i++) {
            if (dataFile.items[i].item_name == name) {
                dataFile.items[i] = item;
            }
        }
    }

    */


}
