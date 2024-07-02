using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneGenerator : MonoBehaviour
{
    //Temp later will be accessible from the game manager
    public ZoneGenManager zoneGenManager;

    [SerializeField]private Tilemap walkeableTileMap;

    [SerializeField]private Vector2Int zoneSize;

    //To be added to the seed
    [SerializeField] private int zoneNumber;


    //Place the ground tiles in a square
    private void Start() {
        //Set the seed
        Random.InitState(zoneGenManager.seed + zoneNumber);

        int rng = Random.Range(0, 1000000);
        //Generate the zone's seed. 50% chance of being a square or a circle
        if(rng%2 == 0) {
            drawSquare(new Vector3Int(0, 0, 0), zoneSize, walkeableTileMap, zoneGenManager.groundTile);
        }
        else
            drawCircle(new Vector3Int(0, 0, 0), zoneSize.x, walkeableTileMap, zoneGenManager.groundTile);

        print("Rng" + rng);
    }

    //Draw tiles in a square with position as center
    private void drawSquare(Vector3Int pos,Vector2Int size, Tilemap tilemap, TileBase tile) {

        for(int x = pos.x - (size.x/2); x < pos.x + (size.x / 2); x++) {
            for(int y = pos.y - (size.y/2); y < pos.y + (size.y / 2); y++) {
                tilemap.SetTile(new Vector3Int(x,y, 0), tile);
            }
        }
    }
    //Returns true if point is inside the circle with center and radius
    private bool inCircle(Vector3Int center,int radius,Vector2Int point) {
        return (point.x - center.x) * (point.x - center.x) + (point.y - center.y) * (point.y - center.y) <= radius * radius;
    }

    //Draw tiles in a circle
    private void drawCircle(Vector3Int pos,int radius, Tilemap tilemap, TileBase tile) {
        for(int x = pos.x - (radius/2); x < pos.x + (radius / 2); x++) {
            for(int y = pos.y - (radius/2); y < pos.y + (radius / 2); y++) {
                if(inCircle(pos, radius/2, new Vector2Int(x,y))) {
                    tilemap.SetTile(new Vector3Int(x,y, 0), tile);
                }
            }
        }
    }

    
}
