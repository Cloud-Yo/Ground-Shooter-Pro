using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] private SpawnManager _mySM;
    [SerializeField] private bool _isSpawning = false;
    void Start()
    {
        _mySM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isSpawning == false)
        {
            _mySM.ActivateSpawn(true);
            _isSpawning = true;
        }
    }
}
