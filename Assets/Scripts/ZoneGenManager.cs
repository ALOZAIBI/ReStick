using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneGenManager : MonoBehaviour
{
    public static ZoneGenManager singleton;
    //The base ground tile
    public TileBase groundTile;

    public TileBase wallTile;

    public int seed;


    private void Start() {
        singleton = this;
        //Generate the seed
        seed = Random.Range(0, 1000000);
    }
}
