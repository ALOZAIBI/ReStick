using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

//To ensure that the zone generator is executed before the zone script starts
[DefaultExecutionOrder(-100)]
public class ZoneGenerator : MonoBehaviour
{

    [SerializeField]private Tilemap walkeableTileMap;

    [SerializeField]private Tilemap collisionTileMap;

    [SerializeField]private Tilemap placeableAreaTileMap;

    //From the zoneSize we can get the extremities of the zone -+(zoneSize/2)
    [SerializeField]private int zoneSize;

    //0 = [4,8[ 
    //1 = [8,15[
    //2 = [15,30[
    //3 = [30,60[
    [SerializeField]private int sizeClass;


    //This will be used to determine which enemies and how many to spawn etc...
    [SerializeField] private float difficultyPoints;

    //Navigation surface to be baked
    [SerializeField] private NavMeshSurface navMeshSurface;



    //Place the ground tiles in a square
    private void Start() {
        //Set the seed
        Random.InitState(ZoneGenManager.singleton.seed + UIManager.singleton.zoneNumber);

        Debug.Log("Seed " + ZoneGenManager.singleton.seed + " ZoneNumber " + UIManager.singleton.zoneNumber);

        setSize();

        genShape();
     
        genPlacementArea();

        spawnEnemies();
    }

    private void setSize() {
        // ----------Set Size----------
        int randomSizeClass = Random.Range(0, 100);
        //15% chance of being size class 0
        if (randomSizeClass < 15) {
            sizeClass = 0;
        }
        //50% chance of being size class 1
        else if (randomSizeClass < 65) {
            sizeClass = 1;
        }
        //25% chance of being size class 2
        else if (randomSizeClass < 90) {
            sizeClass = 2;
        }
        //10% chance of being size class 3
        else {
            sizeClass = 3;
        }


        //get random size using size class
        switch (sizeClass) {
            case 0:
                zoneSize = Random.Range(6, 10);
                break;
            case 1:
                zoneSize = Random.Range(10, 15);
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


    }

    private void genShape() {
        //----------Draw Base---------
        int shape = Random.Range(0, 2);
        //50% chance of being a square or a circle
        if (shape % 2 == 0) {
            drawSquare(new Vector3Int(0, 0, 0), zoneSize, walkeableTileMap, ZoneGenManager.singleton.groundTile);
        }
        else
            drawCircle(new Vector3Int(0, 0, 0), zoneSize, walkeableTileMap, ZoneGenManager.singleton.groundTile);

        //----------Draw Perimeter---------
        if (shape % 2 == 0) {
            drawSquarePerimeter(new Vector3Int(0, 0, 0), zoneSize, collisionTileMap, ZoneGenManager.singleton.wallTile);
        }
        else {
            drawCirclePerimeter(new Vector3Int(0, 0, 0), zoneSize, collisionTileMap, ZoneGenManager.singleton.wallTile, 1);
        }

        navMeshSurface.BuildNavMesh();

    }

    private void genPlacementArea() {
        //For now placement area is just a simple square
        //Get random placementArea size which is at most half the size of the zone and at least 3(Half the minimum size of the zone)
        int placementAreaSize = Random.Range(3, (zoneSize) / 2);

        //Get Random Coordiante for the placement area
        int placementAreaX = Random.Range(-(zoneSize) / 2, (zoneSize) / 2);
        int placementAreaY = Random.Range(-(zoneSize) / 2, (zoneSize) / 2);

        //Draw the placement area
        drawSquare(new Vector3Int(placementAreaX, placementAreaY, 0), placementAreaSize, placeableAreaTileMap, ZoneGenManager.singleton.groundTile, walkeableTileMap, new[] { collisionTileMap });

    }

    private void spawnEnemies() {
        //Difficulty Points isn't exactly zoneNumber, add some leeway
        difficultyPoints = UIManager.singleton.zoneNumber + Random.Range(-UIManager.singleton.zoneNumber * .25f, UIManager.singleton.zoneNumber * .25f) + 1;

        float totalDPsoFar = 0;

        int iterations = 0;
        int maxIter = 100;
        while (totalDPsoFar <= (difficultyPoints - 0.5f) && iterations < maxIter) {

            //So that in the next while loop we don't recheck the same enemy
            List<Character> thisEnemyPool = new List<Character>(ZoneGenManager.singleton.listOfSpawneableEnemies);

            Debug.Log("A:"+ iterations+" DP"+totalDPsoFar);
            //Get random enemy who's DP is at most 3/4 the DP of the zone and that is less than the remaining DP
            Character enemy = ZoneGenManager.singleton.listOfSpawneableEnemies[Random.Range(0, ZoneGenManager.singleton.listOfSpawneableEnemies.Length)];
            while ((enemy.difficultyPoints > difficultyPoints - totalDPsoFar || enemy.difficultyPoints > 0.75f * difficultyPoints) && iterations < maxIter) {
                int randIndex = Random.Range(0, thisEnemyPool.Count);
                enemy = thisEnemyPool[randIndex];
                thisEnemyPool.RemoveAt(randIndex);
                Debug.Log("Removed "+enemy.name + " DP"+enemy.difficultyPoints);
                
                iterations++;
            }

            Debug.Log("B:" + iterations + " DP" + totalDPsoFar);
            //Get random position for the enemy that isn't on an excluded tile
            float enemyX = Random.Range(-(zoneSize) / 2f, (zoneSize) / 2);
            float enemyY = Random.Range(-(zoneSize) / 2f, (zoneSize) / 2);
            while (!canPlaceOnTile(enemyX, enemyY, walkeableTileMap, new[] { collisionTileMap, placeableAreaTileMap }) && iterations < maxIter) {
                enemyX = Random.Range(-(zoneSize) / 2f, (zoneSize) / 2);
                enemyY = Random.Range(-(zoneSize) / 2f, (zoneSize) / 2);
                iterations++;
            }

            iterations++;


            //Spawn the enemy
            Character temp = Instantiate(enemy, new Vector3(enemyX, enemyY, 0), Quaternion.Euler(0, 0, 0));

            Debug.Log("Spawned "+temp.name);
            //Add the DP of the enemy to the total DP
            totalDPsoFar += enemy.difficultyPoints;

            Debug.Log("C:" + iterations + " DP" + totalDPsoFar);
        }

        Debug.Log(totalDPsoFar+" Total HERE");
    }
    private bool canPlaceOnTile(float x,float y,Tilemap needsToBeOn = null, Tilemap[] exclude = null) {
        //If it has to be on another tilemap , we check if that tile on that tilemap has a tile placed
        if (needsToBeOn!=null && !needsToBeOn.HasTile(new Vector3Int((int)x, (int)y, 0))) {
            return false;
        }

        //If it has to exclude another tilemap, we check if that tile on that tilemap has a tile placed
        if (exclude != null)
            foreach (Tilemap tm in exclude) {
                if (tm.HasTile(new Vector3Int((int)x, (int)y, 0))) {
                    return false;
                }
            }

        return true;
    }
    /// <summary>
    /// Draw Tiles in a square with pos as center, needsToBeOn and exclude are used to draw placementArea
    /// </summary>
    /// <param name="pos">center</param>
    /// <param name="size">size of square</param>
    /// <param name="tilemap">tilemap to draw on</param>
    /// <param name="tile">tile used</param>
    /// <param name="needsToBeOn">if this is set, we'll only paint on tiles that are filled with this tilemap</param>
    /// <param name="exclude">if this is set, we won't paint on places these tilemap have tiles</param>
    private void drawSquare(Vector3Int pos,int size, Tilemap tilemap, TileBase tile,Tilemap needsToBeOn = null,Tilemap[] exclude = null) {

        for(int x = pos.x - (size/2); x < pos.x + (size / 2); x++) {
            for(int y = pos.y - (size/2); y < pos.y + (size / 2); y++) {
                if(canPlaceOnTile(x,y,needsToBeOn, exclude))
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
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
