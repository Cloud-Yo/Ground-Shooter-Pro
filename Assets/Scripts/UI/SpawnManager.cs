using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject alienL1;
    [SerializeField] private GameObject _enemyFloater;
    [SerializeField] private GameObject _enemyRammer;
    [SerializeField] private GameObject _enemyShielder;
    [SerializeField] public bool _playerAlive = true;
    [SerializeField] private GameObject _RunnerParent;
    [SerializeField] private GameObject _powerUp;

    [SerializeField] private int _rangeInt;
    [SerializeField] private int _powerUpNum;
    [SerializeField] private int _runnerCount, _floaterCount, _rammerCount;


    void Start()
    {
        _runnerCount = 0;
        _floaterCount = 0;
        _rammerCount = 0;
    }


    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            float xPos = Random.Range(-8f, 8f);
            GameObject Shileder = Instantiate(_enemyFloater, new Vector3(xPos, 9, 0), Quaternion.identity);
            //newPowerUp.GetComponent<PowerUp>().PowerUpType(7);
        }
        */

    }

    IEnumerator SpawnRunner()
    {

            while (_playerAlive)
            {

                yield return new WaitForSeconds(Random.Range(2f, 5f));
                Vector2 posToSpawn = new Vector2(Random.Range(-10f, 10f), 10f);
                GameObject newAlien = Instantiate(alienL1, posToSpawn, Quaternion.identity);
                newAlien.transform.parent = _RunnerParent.transform;
                int flip = Random.Range(0,2);
                switch(flip)
                {
                     case 0:
                         newAlien.GetComponent<SpriteRenderer>().flipX = false;
                         break;
                     case 1:
                         newAlien.GetComponent<SpriteRenderer>().flipX = true;
                         break;
                }
                _runnerCount++;

            }
       

    }

    private IEnumerator SpawnEnemyFloater()
    {
        while (_playerAlive)
        {
            if(_runnerCount > 1 && _runnerCount % 3 == 0)
            {
                yield return new WaitForSeconds(Random.Range(3f, 7f));
                Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 10f);
                GameObject newAlien = Instantiate(_enemyFloater, posToSpawn, Quaternion.identity); ;
                newAlien.transform.parent = _RunnerParent.transform;
                int flip = Random.Range(0, 2);
                switch (flip)
                {
                    case 0:
                        newAlien.GetComponentInChildren<SpriteRenderer>().flipX = false;
                        break;
                    case 1:
                        newAlien.GetComponentInChildren<SpriteRenderer>().flipX = true;
                        break;
                }
                _floaterCount++;
            }
            else
            {
                yield return null;
            }

        }

    }

    IEnumerator SpawnRammerRoutine()
    {
        while (_playerAlive)
        {
            if (_runnerCount > 5 && _runnerCount % 4 == 0)
            {
                yield return new WaitForSeconds(Random.Range(3f, 7f));
                Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 10f);
                GameObject newAlien = Instantiate(_enemyRammer, posToSpawn, Quaternion.identity); ;
                newAlien.transform.parent = _RunnerParent.transform;
                int flip = Random.Range(0, 2);
                switch (flip)
                {
                    case 0:
                        newAlien.GetComponentInChildren<SpriteRenderer>().flipX = false;
                        break;
                    case 1:
                        newAlien.GetComponentInChildren<SpriteRenderer>().flipX = true;
                        break;
                }
                _rammerCount++;
            }
            else
            {
                yield return null;
            }
        }


    }

    IEnumerator SpawnShielderRoutine()
    {
        while(_playerAlive)
        {
            if(_runnerCount > 9 && _runnerCount % 5 == 0)
            {
                yield return new WaitForSeconds(Random.Range(3f, 7f));
                Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 10f);
                GameObject newAlien = Instantiate(_enemyShielder, posToSpawn, Quaternion.identity); ;
                newAlien.transform.parent = _RunnerParent.transform;
                int flip = Random.Range(0, 2);
                switch (flip)
                {
                    case 0:
                        newAlien.GetComponentInChildren<SpriteRenderer>().flipX = false;
                        break;
                    case 1:
                        newAlien.GetComponentInChildren<SpriteRenderer>().flipX = true;
                        break;
                }
                _rammerCount++;
            }
            else
            {
                yield return null;
            }
        }
     
    }

    IEnumerator SpawnPowerUp()
    {
            while (_playerAlive)
            {
                yield return new WaitForSeconds(Random.Range(7f, 15f));
                Vector2 posToSpawn = new Vector2(Random.Range(-10f, 10f), 10f);
                PowerUpChance();
                GameObject newPowerUp = Instantiate(_powerUp, posToSpawn, Quaternion.identity);
                newPowerUp.GetComponent<PowerUp>().PowerUpType(_powerUpNum);
            }
    }

    public void ActivateSpawn(bool spawnBool)
    {
        if(spawnBool)
        {
            StartCoroutine(SpawnRunner());
            StartCoroutine(SpawnPowerUp());
            StartCoroutine(SpawnEnemyFloater());
            StartCoroutine(SpawnRammerRoutine());
            StartCoroutine(SpawnShielderRoutine());
        }
        else if(!spawnBool)
        {
            StopAllCoroutines();
        }

    }

    private void PowerUpChance()
    {
        _rangeInt = Random.Range(0, 101);
        if (_rangeInt <= 5)
        {
            _powerUpNum = 4; // health 5% chance
        }
        else if (_rangeInt > 5 && _rangeInt <= 15)
        {
            _powerUpNum = 7; //mimic 10% chance
        }
        else if (_rangeInt > 15 && _rangeInt <= 25)
        {
            _powerUpNum = 6; //missle 10% chance
        }
        else if (_rangeInt > 25 && _rangeInt <= 35)
        {
            _powerUpNum = 5; //Grape Shot 10% chance
        }
        else if (_rangeInt > 35 && _rangeInt <= 45)
        {
            _powerUpNum = 0; //Triple Shot 10% chance
        }
        else if (_rangeInt > 45 && _rangeInt <= 65)
        {
            _powerUpNum = 1; //Shields 20% chance
        }
        else if (_rangeInt > 65 && _rangeInt <= 100)
        {
            _powerUpNum = 2; //Ammo 35% chance
        }
    }
}
