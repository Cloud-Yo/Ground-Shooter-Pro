using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _strength = 5;
    [SerializeField] private HealthManager _playerHM;
    [SerializeField] private GameObject _splash;
    [SerializeField] private GameObject _myParent;

    
    // Start is called before the first frame update
    void Start()
    {
        _myParent = GameObject.Find("RunnerContainer");
        transform.SetParent(_myParent.transform);
        _playerHM = GameObject.Find("TankUIPanel").GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 15;
        transform.Translate(Vector2.down * _speed * Time.deltaTime);

        if(transform.position.y < -10f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

            collision.GetComponent<Player>().TakeDamage(_strength);
            Instantiate(_splash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Obstacle" )
        {
            Instantiate(_splash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
