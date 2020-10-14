using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //constants
    [Range (10,70)]
    public int roomSize;
    [Range(0, 99)]
    public int numberOfEnemies;
    //linkage
    public RoomGenerator generator;
    public HP_Bar bar;
    public Camera camera;
    public GameObject deathScreen;


    public List<int> goblinLevelParams;
    public List<int> mushroomLevelParams;
    public List<int> skeletonLevelParams;

    private RoomTile[,] _roomTiles;
    private Player _activePlayer;
    private List<Enemy> _enemies;
    [SerializeField]
    private float _targetBPM;
    [SerializeField]
    private float _stepHitRate;
    private float _stepCooldown;
    private float _stepCooldownTimer;
    private float _hitCooldown;
    private float _hitCooldownTimer;
    private bool _standingObjectsUpdated;
    private bool _isDiscoMode = true;
    private bool _isButtonPressed;
    private int _discoTick;

    // Start is called before the first frame update
    void Start()
    {
        _roomTiles = generator.GenerateRoom(roomSize);
        _activePlayer = generator.CreatePlayer(roomSize);
        _enemies = new List<Enemy>();
        for (int i = 0; i < goblinLevelParams[PlayerPrefs.GetInt("Level")]; ++i)
            _enemies.Add(generator.CreateEnemy(roomSize, 1));
        for (int i = 0; i < skeletonLevelParams[PlayerPrefs.GetInt("Level")]; ++i)
            _enemies.Add(generator.CreateEnemy(roomSize, 0));
        for (int i = 0; i < mushroomLevelParams[PlayerPrefs.GetInt("Level")]; ++i)
            _enemies.Add(generator.CreateEnemy(roomSize, 2));
        _stepCooldownTimer = 0;
        _standingObjectsUpdated = true;
        _discoTick = 1;
        _stepCooldown = (60 / _targetBPM) * (1 - _stepHitRate);
        _hitCooldown = 60 / _targetBPM * _stepHitRate;
    }


    public bool CanMove(Vector2 position)
    {
        Vector2Int intPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        if (intPosition.x >= 1 && intPosition.x < _roomTiles.GetLength(0) - 1 && intPosition.y >= 1 && intPosition.y < _roomTiles.GetLength(1) - 1)
        {
            if (_roomTiles[intPosition.x, intPosition.y].type == RoomTile.Type.WALL || _roomTiles[intPosition.x, intPosition.y].standingObject != null)
                return false;
            else
                return true;
        }
        else
            return false;
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
        if (_activePlayer)
            SetVisibleObject(_activePlayer.transform.position, _activePlayer);
        foreach (Enemy _enemy in _enemies)
            SetVisibleObject(_enemy.transform.position, _enemy);
        _standingObjectsUpdated = true;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    private void SetDiscoMode()
    {
        for (int i = 0; i < _roomTiles.GetLength(0); ++i)
            for (int j = 0; j < _roomTiles.GetLength(1); ++j)
                _roomTiles[i, j].SetDiscoMode(((i + j + _discoTick) % 2 == 0) ? 0 : _discoTick);
        switch (_discoTick)
        {
            case 1:
                _discoTick = 2;
                break;
            case 2: 
                _discoTick = 1;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _stepCooldownTimer = (_stepCooldownTimer - Time.fixedDeltaTime <= 0) ? 0 : _stepCooldownTimer - Time.fixedDeltaTime;
        if (_stepCooldownTimer == 0 && !_standingObjectsUpdated)
            UpdateVisibleObjects();
        if (_hitCooldownTimer == 0)
        if (_stepCooldownTimer == 0 && (Input.GetAxis("MoveVertical") != 0 || Input.GetAxis("MoveHorizontal") != 0))
        {
            if (_activePlayer)
                _activePlayer.Move(new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical")));
            foreach (Enemy _enemy in _enemies)
                _enemy.Move(_activePlayer.gameObject.transform.position);
            _stepCooldownTimer = _stepCooldown;
            _standingObjectsUpdated = false;
            if (_isDiscoMode)
            SetDiscoMode();
        }
    }

    private void LateUpdate()
    {

        if (_activePlayer)
        {
            bar.SetHP(_activePlayer.hp);
            camera.transform.position = new Vector3(_activePlayer.transform.position.x, _activePlayer.transform.position.y, camera.transform.position.z);
            if (_enemies.Count == 0)
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                SceneManager.LoadScene("Game");
            }
        }
        else
            deathScreen.SetActive(true);
    }
}
