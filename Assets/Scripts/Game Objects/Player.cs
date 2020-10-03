using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : VisibleObject
{

    public SpriteRenderer shadowRenderer;
    public Effect attackEffect;

    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _attackDamage;
    // Start is called before the first frame update

    public void Hit(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            gameManager.RemoveVisibleObject(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    public void Move(Vector2 direction)
    {
        if (!_rigidBody)
            _rigidBody = gameObject.GetComponent<Rigidbody2D>();

        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            direction = Vector2.right * Mathf.Sign(direction.x);
            _renderer.flipX = direction.x < 0;
        }
        else
            direction = Vector2.up * Mathf.Sign(direction.y);
        if (gameManager.GetVisibleObject((Vector2)gameObject.transform.position + direction) != null)
        {
            if (gameManager.GetVisibleObject((Vector2)gameObject.transform.position + direction).GetType() == typeof(Enemy))
            {
                Enemy _enemy = gameManager.GetVisibleObject((Vector2)gameObject.transform.position + direction) as Enemy;
                _enemy.Hit(_attackDamage);
                Effect _effect = Instantiate(attackEffect, gameObject.transform.position + (Vector3)direction, Quaternion.Euler(Vector3.zero));
                _effect.SetDirection(direction);
            }
        }
        else if (gameManager.CanMove((Vector2)gameObject.transform.position + direction))
        {
            gameManager.RemoveVisibleObject(gameObject.transform.position);
            gameManager.SetVisibleObject((Vector2)gameObject.transform.position + direction, this);
            _rigidBody.velocity = direction * _movementSpeed;
        }
        Invoke("Stop", 1 / _movementSpeed);
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
    }
    protected override void UpdateLayerPosition()
    {
        base.UpdateLayerPosition();
        shadowRenderer.sortingOrder = Mathf.FloorToInt(9000 - gameObject.transform.position.y * 10);
    }

}
