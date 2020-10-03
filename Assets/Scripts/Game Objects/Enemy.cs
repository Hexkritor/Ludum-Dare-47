using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : VisibleObject
{
    [SerializeField]
    protected int _stepsToMove;
    protected int _stepsToMoveLeft;
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _moveToPlayerRate;
    private bool _isMoving;

    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _attackDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _stepsToMoveLeft = _stepsToMove;
        _isMoving = false;
    }

    public void Hit(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            gameManager.RemoveVisibleObject(gameObject.transform.position);
            gameManager.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }

    public void Move(Vector2 playerPosition)
    {
        if (!_rigidBody)
            _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        --_stepsToMoveLeft;
        if (_stepsToMoveLeft <= 0)
        {
            _isMoving = true;
            _stepsToMoveLeft = _stepsToMove;
            Vector2 direction = Vector2.zero;
            if (Mathf.Abs(playerPosition.x - gameObject.transform.position.x) >= Mathf.Abs(playerPosition.y - gameObject.transform.position.y))
            { 
                direction = Vector2.right * Mathf.Sign(playerPosition.x - gameObject.transform.position.x);
                _renderer.flipX = Mathf.Sign(direction.x) < 0;
            }
            else
                direction = Vector2.up * Mathf.Sign(playerPosition.y - gameObject.transform.position.y);

            if (UnityEngine.Random.value <= _moveToPlayerRate)
            {
                if (gameManager.GetVisibleObject((Vector2)gameObject.transform.position + direction) != null)
                {
                    if (gameManager.GetVisibleObject((Vector2)gameObject.transform.position + direction).GetType() == typeof(Player))
                    {
                        Player _enemy = gameManager.GetVisibleObject((Vector2)gameObject.transform.position + direction) as Player;
                        _enemy.Hit(_attackDamage);
                    }
                }
                else if (gameManager.CanMove((Vector2)gameObject.transform.position + direction))
                {
                    gameManager.RemoveVisibleObject(gameObject.transform.position);
                    gameManager.SetVisibleObject((Vector2)gameObject.transform.position + direction, this);
                    _rigidBody.velocity = Vector2.right * Mathf.Sign(direction.x) * _movementSpeed;
                }
            }
            else
            {
                List<Vector2> movements = new List<Vector2>() { Vector2.down, Vector2.up, Vector2.left, Vector2.right };
                while (movements.Count > 0)
                {
                    int random = UnityEngine.Random.Range(0, movements.Count);
                    if (gameManager.CanMove((Vector2)gameObject.transform.position + movements[random]))
                    {
                        gameManager.RemoveVisibleObject(gameObject.transform.position);
                        gameManager.SetVisibleObject((Vector2)gameObject.transform.position + movements[random], this);
                        _rigidBody.velocity = movements[random] * _movementSpeed;
                        _renderer.flipX = movements[random].x < 0;
                        break;
                    }
                    movements.RemoveAt(random);
                }
            }
            Invoke("Stop", 1 / _movementSpeed);
        }
    }

    public void Stop()
    {
        if (!_rigidBody)
            _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _rigidBody.velocity = Vector2.zero;
        gameObject.transform.position = new Vector3Int(
            Mathf.RoundToInt(gameObject.transform.position.x),
            Mathf.RoundToInt(gameObject.transform.position.y),
            Mathf.RoundToInt(gameObject.transform.position.z));
        _isMoving = false;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (!_rigidBody)
            _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        if (!_isMoving && _rigidBody.velocity.magnitude > 0)
            Stop();
    }
}
