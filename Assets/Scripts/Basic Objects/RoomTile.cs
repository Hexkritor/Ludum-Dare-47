using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTile : MonoBehaviour
{
    public enum Type
    {
        FLOOR,
        WALL,
        BUSY
    };

    //linkage
    public Sprite[] floorSprites;
    public Sprite[] wallSprites;

    public VisibleObject standingObject;
    public SpriteRenderer backgroundRendrer;
    public SpriteRenderer environmentRenderer;
    public SpriteRenderer discoMode;

    public Color[] discoColors;

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
        type = _type;
        switch (_type)
        {
            case Type.FLOOR:
                //_renderer.sprite = floorSprite;
                _collider.enabled = false;
                break;
            case Type.WALL:
                //_renderer.sprite = wallSprite;
                _collider.enabled = true;
                break;
        }
    }

    public void SetBackgroundStyle(int style)
    {
        backgroundRendrer.sprite = floorSprites[style];
    }

    public void SetWallStyle(int style)
    {
        environmentRenderer.sprite = wallSprites[style];
        if (style == 4)
            backgroundRendrer.sprite = null;
        environmentRenderer.sortingOrder = Mathf.FloorToInt(9000 - gameObject.transform.position.y * 10);
    }

    public void SetDiscoMode(int mode)
    {
        if (backgroundRendrer.sprite != null)
            discoMode.color = discoColors[mode];
    }
}
