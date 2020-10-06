using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager))]
public class RoomGenerator : MonoBehaviour 
{
    public GameManager gameManager;
    public RoomTile roomTile;
    public Player player;
    public List<Enemy> enemies;
    public TextMeshPro debugText;
    public RhythmUI rhythmUI;

    private RoomTile.Type[,] roomTypes;


    public RoomTile[,] GenerateRoom(int size)
    {
        // instantiate tiles
        size += 2;
        RoomTile[,] tiles = new RoomTile[size, size];
        roomTypes = new RoomTile.Type[size, size];
        for (int j = 0; j < tiles.GetLength(1); ++j)
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                RoomTile _tile = Instantiate(roomTile, Vector2.up * j + Vector2.right * i, Quaternion.Euler(Vector3.zero));
                tiles[i, j] = _tile;
            }

        SetupTileTypes(tiles);
        if (PlayerPrefs.GetInt("Level") % 6 != 5)
        {
            try
            {
                SetupWalls(tiles);
            }
            catch (System.Exception e)
            {
                print(e.Message);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        PaintBackground(tiles);
        PaintWalls(tiles);
        //DebugTiles();

        return tiles;
    }


    public Player CreatePlayer(int size)
    {
        int x = 0, y = 0;
        while (true)
        {
            if (roomTypes[x, y] == RoomTile.Type.FLOOR)
                break;
            x = Random.Range(1, size);
            y = Random.Range(1, size);
        }
        roomTypes[x, y] = RoomTile.Type.BUSY;
        Player _player = Instantiate(player, new Vector2(x, y), Quaternion.Euler(Vector3.zero));
        _player.gameManager = gameManager;
        gameManager.SetVisibleObject(_player.gameObject.transform.position, _player);
        return _player;
    }

    public Enemy CreateEnemy(int size, int id)
    {
        int x = 0, y = 0;
        while (true)
        {
            if (roomTypes[x, y] == RoomTile.Type.FLOOR)
                break;
            x = Random.Range(1, size);
            y = Random.Range(1, size);
        }
        roomTypes[x, y] = RoomTile.Type.BUSY;

        Enemy _enemy = Instantiate(enemies[id], new Vector2(x, y), Quaternion.Euler(Vector3.zero));
        _enemy.gameManager = gameManager;
        gameManager.SetVisibleObject(_enemy.gameObject.transform.position, _enemy);
        return _enemy;
    }

    private void SetupTileTypes(RoomTile[,] tiles)
    {

        for (int j = 0; j < tiles.GetLength(1); ++j)
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                tiles[i, j].SetRoomType((i == 0 || i == tiles.GetLength(0) - 1 || j == 0 || j == tiles.GetLength(1) - 1) ? RoomTile.Type.WALL : RoomTile.Type.FLOOR);
                roomTypes[i, j] = (i == 0 || i == tiles.GetLength(0) - 1 || j == 0 || j == tiles.GetLength(1) - 1) ? RoomTile.Type.WALL : RoomTile.Type.FLOOR;
            }
    }

    private void SetupWalls(RoomTile[,] tiles)
    {
        int x = tiles.GetLength(0) / 2, y = tiles.GetLength(0) / 2;
        int wallGenerateValue = tiles.GetLength(0) - 2;
        while (wallGenerateValue > 0)
        {

            x = Random.Range(0, tiles.GetLength(0));
            y = Random.Range(0, tiles.GetLength(1));
            int w = Random.Range(0, tiles.GetLength(0) / 4),
                h = Random.Range(0, tiles.GetLength(1) / 4);
            for (int i = x; i < tiles.GetLength(0) && i < x + w; ++i)
                for (int j = y; j < tiles.GetLength(1) && j < y + h; ++j)
                { 
                    tiles[i, j].SetRoomType(RoomTile.Type.WALL);
                    roomTypes[i, j] = RoomTile.Type.WALL;
                }
            --wallGenerateValue;
        }


        int[,] dungeonCheck = new int[tiles.GetLength(0), tiles.GetLength(1)];
        bool[,] isAdded = new bool[tiles.GetLength(0), tiles.GetLength(1)];

        int notWalls = 0;
        for (int i = 0; i < tiles.GetLength(0); ++i)
            for (int j = 0; j < tiles.GetLength(1); ++j)
                if (tiles[i, j].type != RoomTile.Type.WALL)
                    ++notWalls;
                else
                    dungeonCheck[i, j] = 1;

        Vector2Int[] moves = { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
        Vector2Int[] bfs = new Vector2Int[4 * dungeonCheck.GetLength(0)];
        int start = 0;
        int end = 1;
        while (tiles[x, y].type == RoomTile.Type.WALL)
        {
            x = Random.Range(1, tiles.GetLength(0) - 1);
            y = Random.Range(1, tiles.GetLength(1) - 1);
        }
        bfs[start] = new Vector2Int(x,y);
        while (start != end)
        {
            dungeonCheck[bfs[start].x, bfs[start].y] = 1;

            foreach (Vector2Int move in moves)
            {
                if (dungeonCheck[bfs[start].x + move.x, bfs[start].y + move.y] == 0 && !isAdded[bfs[start].x + move.x, bfs[start].y + move.y])
                {
                    bfs[end] = new Vector2Int(bfs[start].x + move.x, bfs[start].y + move.y);
                    isAdded[bfs[start].x + move.x, bfs[start].y + move.y] = true;
                    end = (end + 1) % bfs.Length;
                }
            }
            start = (start + 1) % bfs.Length;
            --notWalls;
        }
        print(notWalls);
        if (notWalls != 0)
        {
            throw new System.Exception("FUUUUUUUUU");
        }
    }

    private void PaintBackground(RoomTile[,] tiles)
    {
        int biomeType = Random.Range(0, 3);
        Dictionary<int, List<int>> biomeSprites = new Dictionary<int, List<int>>();
        biomeSprites.Add(0, new List<int>() { 0, 1, 6, 7, 12, 13, 18, 19 });
        biomeSprites.Add(1, new List<int>() { 2, 3, 8, 9, 14, 15, 20, 21 });
        biomeSprites.Add(2, new List<int>() { 4, 5, 10, 11, 16, 17, 22, 23 });

        int tileStyle = biomeSprites[biomeType][Random.Range(0, biomeSprites[biomeType].Count)];
        for (int j = 0; j < tiles.GetLength(1); ++j)
            for (int i = 0; i < tiles.GetLength(0); ++i)
                tiles[i, j].SetBackgroundStyle(tileStyle);
        int styleGenerateValue = (tiles.GetLength(0) - 2) * 4;
        while (styleGenerateValue > 0)
        {
            int x = Random.Range(0, tiles.GetLength(0)), 
                y = Random.Range(0, tiles.GetLength(1)), 
                w = Random.Range(0, tiles.GetLength(0) / 2),
                h = Random.Range(0, tiles.GetLength(1) / 2);
            tileStyle = biomeSprites[biomeType][Random.Range(0, biomeSprites[biomeType].Count)];
            for (int i = x; i < tiles.GetLength(0) && i < x + w; ++i)
                for (int j = y; j < tiles.GetLength(1) && j < y + h; ++j)
                    tiles[i, j].SetBackgroundStyle(tileStyle);
            --styleGenerateValue;
        }
        biomeSprites = new Dictionary<int, List<int>>();
        biomeSprites.Add(0, new List<int>() { 24, 25, 26, 27 });
        biomeSprites.Add(1, new List<int>() { 28, 29, 30, 31 });
        biomeSprites.Add(2, new List<int>() { 32, 33, 34, 35 });
        styleGenerateValue = (tiles.GetLength(0) - 2)/2;
        while (styleGenerateValue > 0)
        {
            int x = Random.Range(1, tiles.GetLength(0) - 2),
                y = Random.Range(1, tiles.GetLength(1) - 2);
            tiles[x, y + 1].SetBackgroundStyle(biomeSprites[biomeType][0]);
            tiles[x + 1, y + 1].SetBackgroundStyle(biomeSprites[biomeType][1]);
            tiles[x, y].SetBackgroundStyle(biomeSprites[biomeType][2]);
            tiles[x + 1, y].SetBackgroundStyle(biomeSprites[biomeType][3]);
            --styleGenerateValue;
        }
    }

    private void PaintWall(RoomTile tile, int code)
    {

        if (code == 255)
            tile.SetWallStyle(4);
        else if ((code & 253) == 253)
            tile.SetWallStyle(6);
        else if ((code & 247) == 247)
            tile.SetWallStyle(11);
        else if ((code & 223) == 223)
            tile.SetWallStyle(12);
        else if ((code & 127) == 127)
            tile.SetWallStyle(7);
        else if ((code & 124) == 124)
            tile.SetWallStyle(1);
        else if ((code & 241) == 241)
            tile.SetWallStyle(5);
        else if ((code & 199) == 199)
            tile.SetWallStyle(9);
        else if ((code & 31) == 31)
            tile.SetWallStyle(3);
        else if ((code & 7) == 7)
            tile.SetWallStyle(8);
        else if ((code & 28) == 28)
            tile.SetWallStyle(0);
        else if ((code & 112) == 112)
            tile.SetWallStyle(2);
        else if ((code & 193) == 193)
            tile.SetWallStyle(10);
        else
            tile.SetWallStyle(13);
    }

    private void PaintWalls(RoomTile[,] tiles)
    {
        Sprite[] walls = Resources.LoadAll<Sprite>("Sprites/TileSetWalls_new");
        //Debug.Log (walls.Length);
        Vector2Int[] offsets = {
            Vector2Int.up,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.right,
            Vector2Int.down + Vector2Int.right,
            Vector2Int.down,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.left,
            Vector2Int.up + Vector2Int.left
        };
        //paint inner walls

        for (int i = 1; i < tiles.GetLength(0) - 1; ++i)
            for (int j = 1; j < tiles.GetLength(1) - 1; ++j)
                if (tiles[i, j].type == RoomTile.Type.WALL)
                {
                    int code = 0;
                    for (int k = 0; k < offsets.Length; ++k)
                        if (tiles[i + offsets[k].x, j + offsets[k].y].type == RoomTile.Type.WALL)
                            code = code | (1 << k);
                    PaintWall(tiles[i, j], code);
                }
        //paint outer walls
        //down walls
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            int code = 0;
            for (int k = 0; k < offsets.Length; ++k)
                if (i + offsets[k].x < 0 || i + offsets[k].x == tiles.GetLength(0) || offsets[k].y == -1)
                    code = code | (1 << k);
                else if (tiles[i + offsets[k].x, offsets[k].y].type == RoomTile.Type.WALL)
                    code = code | (1 << k);
            PaintWall(tiles[i, 0], code);
        }
        //up walls
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            int code = 0;
            for (int k = 0; k < offsets.Length; ++k)
                if (i + offsets[k].x < 0 || i + offsets[k].x == tiles.GetLength(0) || offsets[k].y == 1)
                    code = code | (1 << k);
                else if (tiles[i + offsets[k].x, tiles.GetLength(1) - 1 + offsets[k].y].type == RoomTile.Type.WALL)
                    code = code | (1 << k);
            PaintWall(tiles[i, tiles.GetLength(1) - 1], code);
        }
        //left walls
        for (int i = 0; i < tiles.GetLength(1); ++i)
        {
            int code = 0;
            for (int k = 0; k < offsets.Length; ++k)
                if (i + offsets[k].y < 0 || i + offsets[k].y == tiles.GetLength(1) || offsets[k].x == -1)
                    code = code | (1 << k);
                else if (tiles[offsets[k].x, i + offsets[k].y].type == RoomTile.Type.WALL)
                    code = code | (1 << k);
            PaintWall(tiles[0, i], code);
        }
        //right walls
        for (int i = 0; i < tiles.GetLength(1); ++i)
        {
            int code = 0;
            for (int k = 0; k < offsets.Length; ++k)
                if (i + offsets[k].y < 0 || i + offsets[k].y == tiles.GetLength(1) || offsets[k].x == 1)
                    code = code | (1 << k);
                else if (tiles[tiles.GetLength(0) - 1 + offsets[k].x, i + offsets[k].y].type == RoomTile.Type.WALL)
                    code = code | (1 << k);
            PaintWall(tiles[tiles.GetLength(0) - 1, i], code);
        }
    }

    public void DebugTiles()
    {
        for (int j = 0; j < roomTypes.GetLength(1); ++j)
            for (int i = 0; i < roomTypes.GetLength(0); ++i)
            {
                TextMeshPro text = Instantiate(debugText, new Vector2(i, j), Quaternion.Euler(Vector3.zero));
                text.gameObject.transform.parent = gameManager.gameObject.transform;
                text.text = roomTypes[i, j].ToString();
            }
    }
}
