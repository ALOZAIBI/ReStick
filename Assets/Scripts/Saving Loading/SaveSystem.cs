//using System.Collections;
//using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Text;
using System;

//https://www.youtube.com/watch?v=XOjd_qU2Ido&t=693s
//https://gitlab.com/gamedev-public/unity/-/blob/main/Scripts/IO/SaveGame/GamePersistence.cs
 static class SaveSystem
{
    //this is called in rewardselect screen after the player clicks confirm after the ability was selected
    public static void saveZone(Zone zone) {
        ZoneData data = new ZoneData(zone);

        //xrt is just a random file extension im using because it soiunds and looks cool
        string path = Application.persistentDataPath + "/" + zone.zoneName + ".xrt";
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
        string path = Application.persistentDataPath + "/" + zone.zoneName + ".xrt";
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
            return true;
        }
        else {
            Debug.Log("file doesn't exist in " + path);
            return false;
        }

    }
}
