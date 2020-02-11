using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnim : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 4f;
        transform.Translate(Vector2.down* _speed * Time.deltaTime);
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
