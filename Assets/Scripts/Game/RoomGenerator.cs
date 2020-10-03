using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class RoomGenerator : MonoBehaviour 
{
    public GameManager gameManager;
    public RoomTile roomTile;
    public Player player;
    public List<Enemy> enemies;

    public RoomTile[,] GenerateRoom(int size)
    {
        // instantiate tiles
        size += 2;
        RoomTile[,] tiles = new RoomTile[size, size];
        for (int j = 0; j < tiles.GetLength(1); ++j)
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                RoomTile _tile = Instantiate(roomTile, Vector2.up * j + Vector2.right * i, Quaternion.Euler(Vector3.zero));
                tiles[i, j] = _tile;
            }

        SetupTileTypes(tiles);

        return tiles;
    }

    public Player CreatePlayer(int size)
    {
        Player _player = Instantiate(player, new Vector2(UnityEngine.Random.Range(1, size), UnityEngine.Random.Range(1, size)), Quaternion.Euler(Vector3.zero));
        _player.gameManager = gameManager;
        gameManager.SetVisibleObject(_player.gameObject.transform.position, _player);
        return _player;
    }

    public Enemy CreateEnemy(int size)
    {
        int x = 0, y = 0;
        while (!gameManager.CanMove(Vector2.up * y + Vector2.right * x))
        {
            x = UnityEngine.Random.Range(1, size);
            y = UnityEngine.Random.Range(1, size);
        }
        Enemy _enemy = Instantiate(enemies[UnityEngine.Random.Range(0,enemies.Count)], new Vector2(x, y), Quaternion.Euler(Vector3.zero));
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
            }
    }
}
