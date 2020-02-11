using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject alienL1;
    [SerializeField] public bool _playerAlive = true;
    [SerializeField] private GameObject _RunnerParent;
    [SerializeField] private GameObject _powerUp;


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(alienL1, new Vector3(0, 9, 0), Quaternion.identity);
        }

    }

    IEnumerator SpawnRunner()
    {

            while (_playerAlive)
            {

                yield return new WaitForSeconds(Random.Range(2f, 4f));
                Vector2 posToSpawn = new Vector2(Random.Range(-10f, 10f), 10f);
               // int[] xScale = { 1, -1 };
                //int newAlienScale = Random.Range(0, 2);
                GameObject newAlien = Instantiate(alienL1, posToSpawn, Quaternion.identity);
                //newAlien.transform.localScale = new Vector2(xScale[newAlienScale], 1);
                newAlien.transform.parent = _RunnerParent.transform;

            }
       

    }

    IEnumerator SpawnPowerUp()
    {


            while (_playerAlive)
            {
                yield return new WaitForSeconds(Random.Range(7f, 15f));
                Vector2 posToSpawn = new Vector2(Random.Range(-10f, 10f), 10f);
                int powerUpNum = Random.Range(0, 3);
                GameObject newPowerUp = Instantiate(_powerUp, posToSpawn, Quaternion.identity);
                newPowerUp.GetComponent<PowerUp>().PowerUpType(powerUpNum);
            }


    }

    public void ActivateSpawn(bool spawnBool)
    {
        if(spawnBool)
        {
            StartCoroutine(SpawnRunner());
            StartCoroutine(SpawnPowerUp());
        }
        else if(!spawnBool)
        {
            StopCoroutine(SpawnRunner());
            StopCoroutine(SpawnPowerUp());
        }

    }
}
