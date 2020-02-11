using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] private SpawnManager _mySM;
    void Start()
    {
        _mySM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _mySM.ActivateSpawn(true);
        }
    }
}
