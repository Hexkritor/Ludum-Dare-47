using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTile : MonoBehaviour
{
    public enum Type
    {
        FLOOR,
        WALL
    };

    //linkage
    public Sprite floorSprite;
    public Sprite wallSprite;

    public VisibleObject standingObject;

    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;

    public Type type { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (!_renderer)
            _renderer = gameObject.GetComponent<SpriteRenderer>();
        if (!_collider)
            _collider = gameObject.GetComponent<BoxCollider2D>();
    }

    public void SetRoomType(Type _type)
    {
        if (!_renderer)
            _renderer = gameObject.GetComponent<SpriteRenderer>();
        if (!_collider)
            _collider = gameObject.GetComponent<BoxCollider2D>();
        switch (_type)
        {
            case Type.FLOOR:
                _renderer.sprite = floorSprite;
                _collider.enabled = false;
                break;
            case Type.WALL:
                _renderer.sprite = wallSprite;
                _collider.enabled = true;
                break;
        }
    }
}
