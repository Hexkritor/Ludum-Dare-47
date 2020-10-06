using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : VisibleObject
{

    public SpriteRenderer shadowRenderer;
    public Effect attackEffect;
    public Animator animator;
    public ParticleSystem particleSystem;

    public play_sound sound;

    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _attackDamage;
    // Start is called before the first frame update
    public int hp { get { return _hp; } }

    private void Start()
    {
        _hp = PlayerPrefs.GetInt("Hp");
    }

    public void Hit(int damage)
    {
        _hp -= damage;
        print(_hp.ToString());
        if (_hp <= 0)
        {
            gameManager.RemoveVisibleObject(gameObject.transform.position);
            sound.Play_Death_Sound();
            Destroy(gameObject);
        }
    }

    public void Move(Vector2 direction)
    {
        if (!_rigidBody)
            _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        if (direction.magnitude > 0)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                direction = Vector2.right * Mathf.Sign(direction.x);
                _renderer.flipX = direction.x < 0;
                ParticleSystemRenderer __renderer = particleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                __renderer.flip = (direction.x < 0) ? Vector3.right : Vector3.zero;
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
                    animator.SetBool("isAttacking", true);
                    Invoke("ResetAnimation", 0.2f);
                }
            }
            else if (gameManager.CanMove((Vector2)gameObject.transform.position + direction))
            {
                gameManager.RemoveVisibleObject(gameObject.transform.position);
                gameManager.SetVisibleObject((Vector2)gameObject.transform.position + direction, this);
                _rigidBody.velocity = direction * _movementSpeed;
                animator.SetBool("isDashing", true);
            }
            Invoke("Stop", 1 / _movementSpeed);
            particleSystem.Play();
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
        animator.SetBool("isDashing", false);
        particleSystem.Stop();
    }
    protected override void UpdateLayerPosition()
    {
        base.UpdateLayerPosition();
        shadowRenderer.sortingOrder = Mathf.FloorToInt(9000 - gameObject.transform.position.y * 10) - 1;
    }
    private void ResetAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

}
