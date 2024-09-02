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

    [SerializeField]private Tilemap collisionTileMap;

    [SerializeField]private int zoneSize;

    //0 = [4,8[ 
    //1 = [8,15[
    //2 = [15,30[
    //3 = [30,60[
    [SerializeField]private int sizeClass;

    //To be added to the seed
    [SerializeField] private int zoneNumber;

    //This will be used to determine which enemies and how many to spawn etc...
    [SerializeField] private int difficulty;


    //Place the ground tiles in a square
    private void Start() {
        //Set the seed
        Random.InitState(zoneGenManager.seed + zoneNumber);

        #region Zone Size
        // ----------Set Size----------
        int randomSizeClass = Random.Range(0, 100);
        //15% chance of being size class 0
        if(randomSizeClass < 15) {
            sizeClass = 0;
        }
        //50% chance of being size class 1
        else if(randomSizeClass < 65) {
            sizeClass = 1;
        }
        //25% chance of being size class 2
        else if(randomSizeClass < 90) {
            sizeClass = 2;
        }
        //10% chance of being size class 3
        else {
            sizeClass = 3;
        }

        
        //get random size using size class
        switch(sizeClass) {
            case 0:
                zoneSize = Random.Range(4, 8);
                break;
            case 1:
                zoneSize = Random.Range(8, 15);
                break;
            case 2:
                zoneSize = Random.Range(15, 30);
                break;
            case 3:
                zoneSize = Random.Range(30, 60);
                break;
            default:
                Debug.LogError("Size class not found");
                break;
        }
        #endregion

        //----------Draw Base---------
        int shape = Random.Range(0, 2);
        //50% chance of being a square or a circle
        if(shape%2 == 0) {
            drawSquare(new Vector3Int(0, 0, 0), zoneSize, walkeableTileMap, zoneGenManager.groundTile);
        }
        else
            drawCircle(new Vector3Int(0, 0, 0), zoneSize, walkeableTileMap, zoneGenManager.groundTile);

        //----------Draw Perimeter---------
        if (shape % 2 == 0) {
            drawSquarePerimeter(new Vector3Int(0, 0, 0), zoneSize, collisionTileMap, zoneGenManager.wallTile);
        }
        else {
            drawCirclePerimeter(new Vector3Int(0, 0, 0), zoneSize, collisionTileMap, zoneGenManager.wallTile,1);
        }


        print("Rng" + shape);
    }

    //Draw tiles in a square with position as center
    private void drawSquare(Vector3Int pos,int size, Tilemap tilemap, TileBase tile) {

        for(int x = pos.x - (size/2); x < pos.x + (size / 2); x++) {
            for(int y = pos.y - (size/2); y < pos.y + (size / 2); y++) {
                tilemap.SetTile(new Vector3Int(x,y, 0), tile);
            }
        }
    }

    private void drawSquarePerimeter(Vector3Int pos,int size,Tilemap tilemap,TileBase tile) {
        int xStartPos = pos.x - (size / 2);
        int xEndPos = pos.x + (size / 2);

        int yStartPos = pos.y - (size / 2);
        int yEndPos = pos.y + (size / 2);
        for (int x = xStartPos; x < xEndPos; x++) {
            for (int y = yStartPos; y < yEndPos; y++) {
                //If it is an edge (Start or end of x or y)
                if (y == yStartPos || y == yEndPos-1 || x == xStartPos ||x == xEndPos-1)
                    tilemap.SetTile(new Vector3Int(x,y,0), tile);
            }
        }
    }
    //Returns true if point is inside the circle with center and radius
    private bool inCircle(Vector3Int center,float radius,Vector2Int point) {
        return (point.x - center.x) * (point.x - center.x) + (point.y - center.y) * (point.y - center.y) <= radius * radius;
    }

    //Draw tiles in a circle
    private void drawCircle(Vector3Int pos,int diameter, Tilemap tilemap, TileBase tile) {
        for(int x = pos.x - (diameter/2); x < pos.x + (diameter / 2); x++) {
            for(int y = pos.y - (diameter/2); y < pos.y + (diameter / 2); y++) {
                if(inCircle(pos, diameter/2, new Vector2Int(x,y))) {
                    tilemap.SetTile(new Vector3Int(x,y, 0), tile);
                }
            }
        }
    }
    private bool onCirclePerimeter(Vector3Int center, int radius, Vector2Int point) {
        return (point.x - center.x) * (point.x - center.x) + (point.y - center.y) * (point.y - center.y) == radius * radius;
    }

    //https://www.redblobgames.com/grids/circle-drawing/#outline
    private void drawCirclePerimeter(Vector3Int pos, int diameter, Tilemap tilemap, TileBase tile,int addedThickness=0) {
        float radius = (diameter) / 2;
        for(int r = 0; r<= Mathf.Floor((radius) * Mathf.Sqrt(0.5f)); r++) {
            int d = Mathf.FloorToInt(Mathf.Sqrt(radius * radius - r * r));
            tilemap.SetTile(new Vector3Int(pos.x - d, pos.y +r,0), tile);
            tilemap.SetTile(new Vector3Int(pos.x + d, pos.y + r, 0), tile);
            tilemap.SetTile(new Vector3Int(pos.x - d, pos.y - r, 0), tile);
            tilemap.SetTile(new Vector3Int(pos.x + d, pos.y - r, 0), tile);
            tilemap.SetTile(new Vector3Int(pos.x - r, pos.y + d, 0), tile);
            tilemap.SetTile(new Vector3Int(pos.x + r, pos.y + d, 0), tile);
            tilemap.SetTile(new Vector3Int(pos.x - r, pos.y - d, 0), tile);
            tilemap.SetTile(new Vector3Int(pos.x + r, pos.y - d, 0), tile);

            if (addedThickness > 0) {
                for(int i = 1; i <= addedThickness; i++) {
                    tilemap.SetTile(new Vector3Int(pos.x - d, pos.y + r + i, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x + d, pos.y + r + i, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x - d, pos.y - r - i, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x + d, pos.y - r - i, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x - r - i, pos.y + d, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x + r + i, pos.y + d, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x - r - i, pos.y - d, 0), tile);
                    tilemap.SetTile(new Vector3Int(pos.x + r + i, pos.y - d, 0), tile);
                }
            }

        }
    }

    //------------Types of Zones
    //Circle
    //Square
    //Bridge connecting multiple islands


    //------------Enemy Spawning
    //
}
