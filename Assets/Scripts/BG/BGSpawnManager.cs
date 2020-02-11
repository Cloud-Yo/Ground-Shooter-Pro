using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawnManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _pebbles = new Sprite[11];
    [SerializeField] private Sprite[] _potMarks = new Sprite[13];
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 posToSpawn;
    [SerializeField] private float _interval = 1f;
    [SerializeField] private Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
