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
    public GameObject musicPrefab;
    public GameObject bossMusicPrefab;


    public List<int> roomSizeLevelParams;
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
    [SerializeField]
    private float _globalStepCooldown;
    [SerializeField]
    private float _stepCooldown;
    [SerializeField]
    private float _stepCooldownTimer;
    [SerializeField]
    private float _hitCooldown;
    private bool _standingObjectsUpdated;
    private bool _isDiscoMode = true;
    [SerializeField]
    private bool _isButtonPressed = false;
    private int _discoTick;
    [SerializeField]
    private Vector2 _playerDirection;
    [SerializeField]
    private bool canBePressed;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Level") == 0)
            PlayerPrefs.SetInt("Hp", 150);
        roomSize = roomSizeLevelParams[PlayerPrefs.GetInt("Level")];
        _roomTiles = generator.GenerateRoom(roomSize);
        _activePlayer = generator.CreatePlayer(roomSize);
        _enemies = new List<Enemy>();
        for (int i = 0; i < goblinLevelParams[PlayerPrefs.GetInt("Level")]; ++i)
            _enemies.Add(generator.CreateEnemy(roomSize, 1));
        for (int i = 0; i < skeletonLevelParams[PlayerPrefs.GetInt("Level")]; ++i)
            _enemies.Add(generator.CreateEnemy(roomSize, 0));
        for (int i = 0; i < mushroomLevelParams[PlayerPrefs.GetInt("Level")]; ++i)
            _enemies.Add(generator.CreateEnemy(roomSize, 2));
        if (PlayerPrefs.GetInt("Level") % 6 == 5)
        {
            _enemies.Add(generator.CreateEnemy(roomSize, 3));
            bossMusicPrefab.SetActive(true);
            musicPrefab.SetActive(false);
            _enemies[_enemies.Count - 1].animator.SetTrigger("Afro");
        }
        else
        {
            musicPrefab.SetActive(true);
        }
            
            
        _stepCooldownTimer = _stepCooldown;
        _standingObjectsUpdated = true;
        _discoTick = 1;
        _stepCooldown = (60 / _targetBPM);
        _hitCooldown = 60 / _targetBPM * _stepHitRate;
        _globalStepCooldown = _stepCooldown;
        _playerDirection = Vector2.zero;
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

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canBePressed = _stepCooldownTimer <= _hitCooldown && _stepCooldownTimer > 0;
        Vector2 direction = Vector2.up * Input.GetAxis("MoveVertical") + Vector2.right * Input.GetAxis("MoveHorizontal");
        if (direction.magnitude > 0 && !_isButtonPressed)
            _isButtonPressed = true;
        else if (_isButtonPressed && direction.magnitude == 0)
            _isButtonPressed = false;
        _stepCooldownTimer -= Time.fixedDeltaTime;
        _globalStepCooldown -= Time.fixedDeltaTime; 
        if (_globalStepCooldown <= 0 && !_standingObjectsUpdated)
            UpdateVisibleObjects();
        if (_stepCooldownTimer <= _hitCooldown && _isButtonPressed)
        {
            if (_activePlayer)
                _playerDirection = direction;
            _stepCooldownTimer += _stepCooldown;
            _standingObjectsUpdated = false;
        }
        else if ((_stepCooldownTimer >= _hitCooldown || _stepCooldownTimer < 0) && _isButtonPressed)
        {
            if (_stepCooldownTimer < 0)
                _stepCooldownTimer += _stepCooldown;
        }
        if (_stepCooldownTimer < 0)
            _stepCooldownTimer += _stepCooldown; 
        if (_globalStepCooldown <= 0)
        {
            _globalStepCooldown += _stepCooldown;
            if (_activePlayer)
            {
                _activePlayer.Move(new Vector2(_playerDirection.x, _playerDirection.y));
                _playerDirection = Vector2.zero;
            }
            foreach (Enemy _enemy in _enemies)
                _enemy.Move(_activePlayer.gameObject.transform.position);
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
                PlayerPrefs.SetInt("Hp", Mathf.Min(_activePlayer.hp + 30 + 15 * PlayerPrefs.GetInt("Level"), 150));
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                SceneManager.LoadScene("Game");
            }
        }
        else
            deathScreen.SetActive(true);
    }
}
