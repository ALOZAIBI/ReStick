//using System.Collections;
//using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

//https://www.youtube.com/watch?v=XOjd_qU2Ido&t=693s
//https://gitlab.com/gamedev-public/unity/-/blob/main/Scripts/IO/SaveGame/GamePersistence.cs
//https://lucid.app/documents/view/c95f0f5b-f458-4484-aa28-a31b190da027
//the hierarchy of the save directory is written on notebook page 45
static class SaveSystem
{
    //creates the folders to be used in saving this will be called in UIManager's start function
    public static void initialiseSaveSlots() {
        //creates root slot folders
        string path;
        string[] slots = new string[] { "/slot1", "/slot2", "/slot3" };
        foreach (string saveSlot in slots) {
            path = Application.persistentDataPath + saveSlot;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //creates zone folders in each slot folder
            path = Application.persistentDataPath + saveSlot + "/zones";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //creates WorldSaves folder in each slot folder
            path = Application.persistentDataPath + saveSlot + "/worldSave";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //creates inventory folder in each slot folder
            path = Application.persistentDataPath + saveSlot + "/worldSave/inventory";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //creates mapSaves folder in each slot folder
            path = Application.persistentDataPath + saveSlot + "/mapSave";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //creates inventory folder in each slot folder in each map
            path = Application.persistentDataPath + saveSlot + "/mapSave/inventory";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //creates inventory folder in each slot folder in each map
            path = Application.persistentDataPath + saveSlot + "/mapSave/shop/shopAbilitiesAndPurchaseInfo";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
    //this is called when loading a new map. To reinitialise shop
    //need to figure out however a diffewrent way to save shop stuff since.. if I complete map 1 then compelte map 2 then go back to map1 map 1's shop would have changed what I can do isntead is if you complete a map make the shop inaccessible you can say something like the shopkeeper left since he was bored and lonely haha
    public static void deleteShop() {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/shop";
        string[] files = Directory.GetFiles(path);
        //Debug.Log("The files are" + files[0]);

        foreach (string file in files) {
            File.Delete(file);
        }
        path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/shop/shopAbilitiesAndPurchaseInfo";
        files = Directory.GetFiles(path);
        foreach (string file in files) {
            File.Delete(file);
        }

    }
    //this will be used to name the saveFile it has to be reset to 0 Every Time we are batch saving all characters in UIManager
    public static int characterNumber = 0;
    public static void saveShopCharacters(Character character) {
        CharacterData data = new CharacterData(character);

        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/shop/shopCharacter" + characterNumber + ".xrt";
        using (FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);
            //the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));

            writer.Flush();
        }
        characterNumber++;
    }
    public static void loadShopCharacters(Shop shop) {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/shop";
        string[] files = Directory.GetFiles(path);
        //Debug.Log("The files are" + files[0]);

        foreach (string charSave in files) {
            if (File.Exists(charSave)) {
                using (FileStream fs = File.Open(charSave, FileMode.Open)) {
                    BinaryReader reader = new BinaryReader(fs);
                    //the 2 lines that follow are the encrypted version
                    //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                    //CharacterData data = JsonConvert.DeserializeObject < GameStateData>(Encoding.UTF8.GetString(encodedBytes));

                    //this is the non-encrypted version
                    CharacterData data = JsonConvert.DeserializeObject<CharacterData>(reader.ReadString());

                    UIManager.singleton.characterFactory.addCharacterAsChild(data, shop.characterHolder.transform);
                }
            }
            else
                Debug.Log("FIle doesn't exist in " + charSave);
        }
    }
    public static void saveShopAbilitiesAndPurchaseInfo(Shop shop) {
        //creates a list of abilityNames from the abilities in shop
        List<string>abilityNames = new List<string>();
        ShopData data = new ShopData(shop);
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/shop/shopAbilitiesAndPurchaseInfo/abilitiesAndPurchaseInfo.xrt";

        using(FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);

            // the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));
            writer.Flush();
        }
    }
    //returns false if there is no savefile
    public static bool loadShopAbilitiesAndPurchaseInfo(Shop shop) {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/shop/shopAbilitiesAndPurchaseInfo/abilitiesAndPurchaseInfo.xrt";
        if (File.Exists(path)) {
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                BinaryReader reader = new BinaryReader(fs);

                //the 2 lines that follow are the encrypted version
                //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                //abilityNames = JsonConvert.DeserializeObject < List<string>>(Encoding.UTF8.GetString(encodedBytes));
                ShopData data= JsonConvert.DeserializeObject<ShopData>(reader.ReadString());

                UIManager.singleton.abilityFactory.addRequestedAbilitiesToShop(shop,data.abilityNames);
                data.purchaseInfoToShop(shop);
                return true;
            }
        }
        else
            return false;
    }
    public static void saveInventoryInWorld() {
        InventoryData data = new InventoryData();
        data.gold = UIManager.singleton.playerParty.gold;
        //adds abilities
        foreach (Transform child in UIManager.singleton.playerParty.abilityInventory.transform) {
            Ability temp = child.GetComponent<Ability>();
            data.abilityNames.Add(temp.abilityName);
        }
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/worldSave/inventory/inventory.xrt";
        using (FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);

            // the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(abilityNames));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));
            writer.Flush();
        }
    }
    public static void loadInventoryInWorld() {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/worldSave/inventory/inventory.xrt";
        if (File.Exists(path)) {
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                BinaryReader reader = new BinaryReader(fs);
                //the 2 lines that follow are the encrypted version
                //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                //abilityNames = JsonConvert.DeserializeObject < List<string>>(Encoding.UTF8.GetString(encodedBytes));

                //adds abilities and gold
                InventoryData data = JsonConvert.DeserializeObject<InventoryData>(reader.ReadString());
                UIManager.singleton.abilityFactory.addRequestedAbilitiesToInventory(data.abilityNames);
                UIManager.singleton.playerParty.gold = data.gold;
            }
        }
        else
            Debug.Log("File doesn't exist in " + path);

    }
    public static void saveInventoryInMap() {
        InventoryData data = new InventoryData();
        data.gold = UIManager.singleton.playerParty.gold;
        //adds abilities
        foreach(Transform child in UIManager.singleton.playerParty.abilityInventory.transform) {
            Ability temp = child.GetComponent<Ability>();
            data.abilityNames.Add(temp.abilityName);
        }
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/inventory/inventory.xrt";
        using(FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);

            // the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(abilityNames));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));
            writer.Flush();
        }
    }
    public static void loadInventoryInMap() {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/inventory/inventory.xrt";
        if (File.Exists(path)) {
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                BinaryReader reader = new BinaryReader(fs);
                //the 2 lines that follow are the encrypted version
                //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                //abilityNames = JsonConvert.DeserializeObject < List<string>>(Encoding.UTF8.GetString(encodedBytes));

                //adds abilities and gold
                InventoryData data= JsonConvert.DeserializeObject<InventoryData>(reader.ReadString());
                UIManager.singleton.abilityFactory.addRequestedAbilitiesToInventory(data.abilityNames);
                UIManager.singleton.playerParty.gold = data.gold;
            }
        }
        else
            Debug.Log("File doesn't exist in " + path);

    }
    //characterNumber will be used to name the saveFile it has to be reset to 0 Every Time we are batch saving all characters in UIManager
    
    public static void saveCharacterInMap(Character character) {
        CharacterData data = new CharacterData(character);

        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/" + "character" + characterNumber + ".xrt";
        using (FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);
            //the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));

            writer.Flush();
        }
        characterNumber++;
        //Debug.Log("File saved to" + path);
    }
    //loads all character's map saves
    public static void loadCharactersInMap() {
        //Debug.Log("Loading char in map");
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/mapSave/";
        string[] files = Directory.GetFiles(path);

        foreach (string charSave in files) {
            if (File.Exists(charSave)) {
                using (FileStream fs = File.Open(charSave, FileMode.Open)) {
                    BinaryReader reader = new BinaryReader(fs);
                    //the 2 lines that follow are the encrypted version
                    //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                    //CharacterData data = JsonConvert.DeserializeObject < GameStateData>(Encoding.UTF8.GetString(encodedBytes));

                    //this is the non-encrypted version
                    CharacterData data = JsonConvert.DeserializeObject<CharacterData>(reader.ReadString());

                    UIManager.singleton.characterFactory.addCharacterAsChild(data,UIManager.singleton.playerParty.transform);
                    
                    //Debug.Log("Character loaded" + data.charName + data.HP);
                }
            }
            else
            Debug.Log("FIle doesn't exist in " + charSave);
        }
    }
    public static void saveCharacterInWorld(Character character) {
        CharacterData data = new CharacterData(character);

        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/worldSave/" + "character"+characterNumber+".xrt";
        using(FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);
            //the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));

            writer.Flush();
        }
        characterNumber++;
        //Debug.Log("File saved to" + path);
    }

    //loads all character's world saves
    public static void loadCharactersInWorld() {
        //Debug.Log("Loading char in world");
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/worldSave/";
        string[] files = Directory.GetFiles(path);
        //Debug.Log("The files are" + files[0]);

        foreach(string charSave in files) {
            if (File.Exists(charSave)) {
                using(FileStream fs = File.Open(charSave, FileMode.Open)) {
                    BinaryReader reader = new BinaryReader(fs);
                    //the 2 lines that follow are the encrypted version
                    //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                    //CharacterData data = JsonConvert.DeserializeObject < GameStateData>(Encoding.UTF8.GetString(encodedBytes));

                    //this is the non-encrypted version
                    CharacterData data = JsonConvert.DeserializeObject<CharacterData>(reader.ReadString());
                    Debug.Log("Adding " + data.charName);
                    UIManager.singleton.characterFactory.addCharacterAsChild(data,UIManager.singleton.playerParty.transform);
                }
            }
            Debug.Log("FIle doesn't exist in " +charSave);
        }
    }
    
    public static void saveGameState(string mapName,bool inMap) {
        GameStateData data = new GameStateData(mapName, inMap);
        //Debug.Log("In save game state map nampe is =" + data.mapName + data.inMap);

        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/gamestate.xrt";
        using(FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);
            //the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));

            writer.Flush();
        }
        //Debug.Log("File saved to" + path);
    }
    //if save file exists check if in map. if in map load map otherwise load world. If no save file exists add random character
    //to playerParty savegamestate then load world
    //returns true if in map. False Otherqwise
    public static bool loadGameState() {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/gamestate.xrt";
        if (File.Exists(path)) {
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                BinaryReader reader = new BinaryReader(fs);
                //the 2 lines that follow are the encrypted version
                //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                //GameStateData data = JsonConvert.DeserializeObject < GameStateData>(Encoding.UTF8.GetString(encodedBytes));

                //this is the non encrypted version
                GameStateData data = JsonConvert.DeserializeObject<GameStateData>(reader.ReadString());

                data.loadMapOrWorldScene();

                //Debug.Log("file loaded from " + path);
                return data.inMap;

            }
        }
        else {
            //Debug.Log("file doesn't exist in " + path + "Creating new Save file");
            GameStateData data = new GameStateData("", false);
            data.initNewSave();
            return false;
        }
    }
    //this is called in rewardselect screen after the player clicks confirm after the ability was selected
    public static void saveZone(Zone zone) {
        ZoneData data = new ZoneData(zone);

        //xrt is just a random file extension im using because it soiunds and looks cool
        string path = Application.persistentDataPath +"/"+ UIManager.saveSlot+ "/zones"+ "/" + zone.zoneName + ".xrt";
        using(FileStream fs = File.Open(path, FileMode.Create)) {
            BinaryWriter writer = new BinaryWriter(fs);
            //the 2 lines that follow are the encrypted version
            //byte[] plainTextBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            //writer.Write(Convert.ToBase64String(plainTextBytes));

            //this is the non encrypted version
            writer.Write(JsonConvert.SerializeObject(data));

            writer.Flush();
        }

        Debug.Log("File saved to" + path);
    }

    //returns true if saveFile exists 
    //this function is called in zoneStart to load the zone if a savefile exists
    public static bool loadZone(Zone zone) {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/zones" + "/" + zone.zoneName + ".xrt";
        if (File.Exists(path)) {
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                BinaryReader reader = new BinaryReader(fs);
                //the 2 lines that follow are the encrypted version
                //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                //ZoneData data = JsonConvert.DeserializeObject<ZoneData>(Encoding.UTF8.GetString(encodedBytes));

                //this is the non encrypted version
                ZoneData data = JsonConvert.DeserializeObject<ZoneData>(reader.ReadString());
                data.dataToZone(zone);
            }
            Debug.Log("file loaded from " + path);
            return true;
        }
        else {
            Debug.Log("file doesn't exist in " + path);
            return false;
        }
    }
    //loads wether or not a Zone has been completed to SceneSelect
    public static bool loadCompletionSceneSelect(SceneSelect sceneSelect) {
        string path = Application.persistentDataPath + "/" + UIManager.saveSlot + "/zones" + "/" + sceneSelect.sceneToLoad + ".xrt";
        if (File.Exists(path)) {
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                BinaryReader reader = new BinaryReader(fs);
                //the 2 lines that follow are the encrypted version
                //byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                //ZoneData data = JsonConvert.DeserializeObject<ZoneData>(Encoding.UTF8.GetString(encodedBytes));

                //this is the non encrypted version
                ZoneData data = JsonConvert.DeserializeObject<ZoneData>(reader.ReadString());
                //the actual loading
                sceneSelect.completed = data.completed;
            }
            //Debug.Log("file loaded from " + path);
            return true;
        }
        else {
            //Debug.Log("Zone hasn't been completed ");
            sceneSelect.completed = false;
            return false;
        }
    }


}
