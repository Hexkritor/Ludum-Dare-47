using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public Animator animator;

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Horizontal", direction.x);
        gameObject.transform.position += Vector3.up * 0.5f;
    }
}
