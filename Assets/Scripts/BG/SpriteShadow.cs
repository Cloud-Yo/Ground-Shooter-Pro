using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    [SerializeField] private GameObject _myShadow;
    [SerializeField] private Sprite _mySprite;
    [SerializeField] private SpriteRenderer _shadowSR;
    [SerializeField] private Transform _ShadowDaddy;

    // Start is called before the first frame update
    void Start()
    {

        _ShadowDaddy = this.transform;
        _mySprite = GetComponent<SpriteRenderer>().sprite;
        _myShadow = new GameObject("Shadow");
        _myShadow.transform.SetParent(_ShadowDaddy, true);
        _myShadow.transform.rotation = _ShadowDaddy.rotation;
        _myShadow.transform.localScale = new Vector2(1,1);
        _shadowSR = _myShadow.AddComponent<SpriteRenderer>();
        _shadowSR.sortingLayerName = "BG";
        _shadowSR.sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder - 1;
        _shadowSR.sprite = _mySprite;
      
        //_myShadow.AddComponent<CastShadow>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = new Vector2(_ShadowDaddy.position.x + GameManager._globalShadowOffset.x, _ShadowDaddy.position.y + GameManager._globalShadowOffset.y);
        _myShadow.transform.position = position;
        _shadowSR.color = GameManager._globalShadowColor;
    }
}
