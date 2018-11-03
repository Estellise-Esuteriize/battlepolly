using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataController : MonoBehaviour {

    public static DataController instance = null;

    public float defaultTrashCash = 500f;

    public bool isDebugging = true;

    private string filename;
    private string path;
    private string file;

    private DataFile dataFile;


    private string[] itemName = new string[] { "Heart", "Phone", "Sheild", "Magnet", "Sweeper", "Bomb", "GearsOfWar" };
    private int[] itemPrice = new int[] { 300, 300, 100, 50, 600, 900, 850 };


    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

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

            dataFile = JsonUtility.FromJson<DataFile>(output);
        }
        else {

            dataFile = new DataFile {
                trash_cash = defaultTrashCash,
                music = true,
                items = new List<Inventory>()
            };

            for (int i = 0; i < itemName.Length; i++) {

                Inventory inventory;
                inventory.item_name = itemName[i];
                inventory.item_count = 0;
                inventory.item_price = itemPrice[i];
            
                dataFile.items.Add(inventory);

            }

            string output = JsonUtility.ToJson(dataFile);

            File.WriteAllText(file, output);
        }
    }


    public DataFile GetData() {
        return dataFile;
    }

    public void SetData(DataFile dataFile) {
        this.dataFile = dataFile;
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


}
