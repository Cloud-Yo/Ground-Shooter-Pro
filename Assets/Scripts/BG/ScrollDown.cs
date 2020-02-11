using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDown : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 4f;
        transform.Translate( Vector2.down * _speed * Time.deltaTime);

        if(transform.position.y <= -10)
        {
            Destroy(this.gameObject);
        }
    }
}
