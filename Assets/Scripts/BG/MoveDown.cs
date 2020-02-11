using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    [SerializeField] private float _speed = 0f;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 4f;

        transform.Translate(Vector2.down * _speed * Time.deltaTime);    

        if(transform.position.y <= -11f)
        {
            transform.position = new Vector2(0, 11);
        }

    }
}
