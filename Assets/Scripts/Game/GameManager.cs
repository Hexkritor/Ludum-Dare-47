using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //constants
    [Range (10,70)]
    public int roomSize;
    [Range(0, 99)]
    public int numberOfEnemies;
    //linkage
    public RoomGenerator generator;
    public Camera camera;

    private RoomTile[,] _roomTiles;
    private Player _activePlayer;
    private List<Enemy> _enemies;
    [SerializeField]
    private float _stepCooldown;
    private float _stepCooldownTimer;
    private bool _standingObjectsUpdated;

    // Start is called before the first frame update
    void Start()
    {
        _roomTiles = generator.GenerateRoom(roomSize);
        _activePlayer = generator.CreatePlayer(roomSize);
        _enemies = new List<Enemy>();
        for (int i = 0; i < numberOfEnemies; ++i)
        { 
            _enemies.Add(generator.CreateEnemy(roomSize));
        }
        _stepCooldownTimer = 0;
        _standingObjectsUpdated = true;
    }


    public bool CanMove(Vector2 position)
    {
        Vector2Int intPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        try
        {
            return _roomTiles[intPosition.x, intPosition.y].type != RoomTile.Type.WALL && !_roomTiles[intPosition.x, intPosition.y].standingObject;
        }
        catch 
        {
            return false;
        }
    }

    public VisibleObject GetVisibleObject(Vector2 position)
    {
        return _roomTiles[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].standingObject;
    }

    public void RemoveVisibleObject(Vector2 position)
    {
        _roomTiles[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].standingObject = null;
    }

    public void SetVisibleObject(Vector2 position, VisibleObject standingObject)
    {
        _roomTiles[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].standingObject = standingObject;
    }

    private void UpdateVisibleObjects()
    {
        for (int j = 0; j < _roomTiles.GetLength(1); ++j)
            for (int i = 0; i < _roomTiles.GetLength(0); ++i)
                _roomTiles[i, j].standingObject = null;
        SetVisibleObject(_activePlayer.transform.position, _activePlayer);
        foreach (Enemy _enemy in _enemies)
            SetVisibleObject(_enemy.transform.position, _enemy);
        _standingObjectsUpdated = true;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _stepCooldownTimer = (_stepCooldownTimer - Time.fixedDeltaTime <= 0) ? 0 : _stepCooldownTimer - Time.fixedDeltaTime;
        if (_stepCooldownTimer == 0 && !_standingObjectsUpdated)
            UpdateVisibleObjects();
        if (_stepCooldownTimer == 0 && (Input.GetAxis("MoveVertical") != 0 || Input.GetAxis("MoveHorizontal") != 0))
        {
            foreach (Enemy _enemy in _enemies)
                _enemy.Move(_activePlayer.gameObject.transform.position);
            if (_activePlayer)
                _activePlayer.Move(new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical")));
            _stepCooldownTimer = _stepCooldown;
            _standingObjectsUpdated = false;
        }
    }

    private void LateUpdate()
    {
        camera.transform.position = new Vector3 (_activePlayer.transform.position.x, _activePlayer.transform.position.y, camera.transform.position.z);
    }
}
