using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Color _color1, _color2;
    [SerializeField] private SpriteRenderer _myRend;
    [SerializeField] private GameObject _smoke;

    void Start()
    {
        _myRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime, Space.Self);
        StartCoroutine(ColorChange());
        if(transform.position.y >= 12f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            
        }
       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Player")
        {
            //Damage Player
        }
        else if(collision.tag == "Obstacle")
        {
            Instantiate(_smoke, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
    IEnumerator ColorChange()
    {
       while(true)
        {
            _myRend.color = Color.Lerp(_color1, _color2, 1f);
            yield return new WaitForSeconds(0.01f);
            _myRend.color = Color.Lerp(_color2, _color1, 1f);
            yield return new WaitForSeconds(0.01f);
        }
  

    }
}
