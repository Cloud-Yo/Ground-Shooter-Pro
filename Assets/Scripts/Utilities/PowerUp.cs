using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _powerUpID;
    [SerializeField] private GameObject[] _powerUpImg; 
  
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 4f;
        transform.Translate(Vector2.down * _speed * Time.deltaTime, Space.World);

        if(transform.position.y <= -10f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
            switch(_powerUpID)
            {
                case 0: //TripleShot
                    collision.GetComponentInChildren<Shoot>().ActivateTripleShot();
                    Destroy(this.gameObject);
                    break;
                case 1: //Shields
                    //collision.GetComponent<Player>();
                    collision.GetComponent<Player>().ActivateShields();
                    Destroy(this.gameObject);
                    break;
                case 2: //Speed
                    collision.GetComponent<Player>().ActivateSpeedBoost();
                    Destroy(this.gameObject);
                    break;
            }
           


        }

        else if(collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }

    public void PowerUpType(int _type)
    {
        _powerUpID = _type;
        _powerUpImg[_powerUpID].SetActive(true);
    }
}
