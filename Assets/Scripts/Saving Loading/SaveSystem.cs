//using System.Collections;
//using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Text;
using System;

//https://www.youtube.com/watch?v=XOjd_qU2Ido&t=693s
//https://gitlab.com/gamedev-public/unity/-/blob/main/Scripts/IO/SaveGame/GamePersistence.cs
//the hierarchy of the save directory is written on notebook page 45
 static class SaveSystem
{
    //creates the folders to be used in saving this will be called in UIManager's start function
    public static void initialiseSaveSlots() {
        //creates root slot folders
        string path = Application.persistentDataPath + "/slot1";
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path = Application.persistentDataPath + "/slot2";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path = Application.persistentDataPath + "/slot3";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        //creates zone folders in each slot folder
        path = Application.persistentDataPath + "/slot1" + "/zones";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path = Application.persistentDataPath + "/slot2" + "/zones";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path = Application.persistentDataPath + "/slot3" + "/zones";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

    }
    //https://lucid.app/documents/view/c95f0f5b-f458-4484-aa28-a31b190da027
    public static void saveGameState(string mapName,bool inMap) {
        GameStateData data = new GameStateData(mapName, inMap);
        Debug.Log("In save game state map nampe is =" + data.mapName + data.inMap);

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
        Debug.Log("File saved to" + path);
    }
    //if save file exists check if in map. if in map load map otherwise load world. If no save file exists add random character
    //to playerParty savegamestate then load world
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

            }
            Debug.Log("file loaded from " + path);
            return true;
        }
        else {
            Debug.Log("file doesn't exist in " + path + "Creating new Save file");
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
        string path = Application.persistentDataPath + UIManager.saveSlot + "/zone" + "/" + sceneSelect.sceneToLoad + ".xrt";
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
            Debug.Log("file loaded from " + path);
            return true;
        }
        else {
            Debug.Log("Zone hasn't been completed ");
            sceneSelect.completed = false;
            return false;
        }
    }


}
