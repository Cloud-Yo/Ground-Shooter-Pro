using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextureOffset :  MonoBehaviour
{
    [SerializeField] private float _speed =1.0f;
    [SerializeField] private Material _myMat;
    [SerializeField] private GameManager _myGM;

    private void Start()
    {
        _myMat = GetComponent<SpriteRenderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed;
        _myMat.mainTextureOffset += new Vector2(0, 1) * _speed * Time.deltaTime; 
    }
}
