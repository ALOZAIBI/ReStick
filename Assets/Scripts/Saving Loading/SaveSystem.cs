//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//https://www.youtube.com/watch?v=XOjd_qU2Ido&t=693s
public static class SaveSystem
{
    //this is called in rewardselect screen after the player clicks confirm after the ability was selected
    public static void saveZone(Zone zone) {
        BinaryFormatter formatter = new BinaryFormatter();
        //xrt is just a random file extension im using because it soiunds and looks cool
        string path = Application.persistentDataPath + "/" + zone.zoneName + ".xrt";
        FileStream stream = new FileStream(path, FileMode.Create);

        ZoneData data = new ZoneData(zone);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("File saved to" + path);
    }

    //returns true if saveFile exists 
    //this function is called in zoneStart to load the zone if a savefile exists
    public static bool loadZone(Zone zone) {
        string path = Application.persistentDataPath + "/" + zone.zoneName + ".xrt";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ZoneData data = formatter.Deserialize(stream) as ZoneData;

            zone.completed = data.completed;
            //load abilities here but for now only completed just to test the thing
            stream.Close();

            return true;
        }
        else {
            Debug.Log("file doesn't exist in "+ path);
            return false;
        }
    }
}
