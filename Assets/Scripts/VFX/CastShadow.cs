using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastShadow : MonoBehaviour
{
    [SerializeField] private Transform _shadowDaddy;
    [SerializeField] private SpriteRenderer _shadowSR;
    [SerializeField] private float _speed = 1f;
    void Start()
    {
        transform.position = Vector2.zero;
        _shadowDaddy = this.transform.parent;
        Vector2 position = new Vector2(_shadowDaddy.localPosition.x + GameManager._globalShadowOffset.x, _shadowDaddy.localPosition.y + GameManager._globalShadowOffset.y);
        transform.position = position;

    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 4f;

        transform.position = Vector2.down * _speed * Time.deltaTime;
        _shadowSR.color = GameManager._globalShadowColor;
    }
}
