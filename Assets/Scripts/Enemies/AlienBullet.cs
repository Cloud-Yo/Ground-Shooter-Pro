using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _strength = 5;
    [SerializeField] private GameObject _splash;
    [SerializeField] private GameObject _myParent;

    
    // Start is called before the first frame update
    void Start()
    {
        _myParent = GameObject.Find("RunnerContainer");
        transform.SetParent(_myParent.transform);

    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * 15;
        transform.Translate(Vector2.down * _speed * Time.deltaTime, Space.Self);

        if((transform.parent.position - transform.position).magnitude > 12f)
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
        else if(collision.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            Instantiate(_splash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
