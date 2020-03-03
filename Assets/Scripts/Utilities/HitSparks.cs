using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSparks : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform _playerGO;

    private void Awake()
    {

    }
    void Start()
    {
        //_playerGO = GameObject.Find("Player").transform;
        //transform.SetParent(_playerGO, false);

        GetComponent<ParticleSystem>().Play();

        Destroy(this.gameObject, 1.5f);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
