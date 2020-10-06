using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    public GameManager gameManager;

    protected SpriteRenderer _renderer;
    protected Rigidbody2D _rigidBody;

    protected bool _isVisible;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!_renderer)
            _renderer = gameObject.GetComponent<SpriteRenderer>();
        if (!_rigidBody)
            _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _isVisible = true;
    }

    protected virtual void UpdateLayerPosition()
    {
        if (!_renderer)
            _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Mathf.FloorToInt(9000 - gameObject.transform.position.y * 10);
    }

    protected virtual void LateUpdate()
    {
        UpdateLayerPosition();
    }

    //public void UpdateVision(float )

}
