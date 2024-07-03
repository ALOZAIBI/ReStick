using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneGenManager : MonoBehaviour
{
    //The base ground tile
    public TileBase groundTile;

    public TileBase wallTile;

    public int seed;

    private void Start() {
        //Generate the seed
        seed = Random.Range(0, 1000000);
    }
}
