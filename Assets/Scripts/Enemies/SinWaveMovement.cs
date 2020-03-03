using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveMovement : MonoBehaviour
{

    [SerializeField] private float _sinAmplitude = 1f;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private LayerMask obstacleLayerMask;
 
    void Start()
    {
        _sinAmplitude = Random.Range(0.2f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.parent.transform.position.x + Mathf.Sin(Time.time) * _sinAmplitude;
        float y = transform.parent.position.y;

        transform.position = new Vector2(x, y);
        DodgeObstacle();

    }

    private void DodgeObstacle()
    {
        RaycastHit2D boxCaster = Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0f, Vector2.down, 3f, obstacleLayerMask);
        Color rayColor;

        if (boxCaster.collider != null)
        {
            rayColor = Color.green;

            Collider2D _col = boxCaster.collider;
            Vector2 _colCenter = _col.bounds.center;

            if (transform.position.x > _colCenter.x)
            {
                float xDir = _sinAmplitude * 3;
                GetComponentInParent<FloaterParent>().DodgeObstacle(xDir);

            }
            else if (transform.position.x <= _colCenter.x)
            {
                float xDir = _sinAmplitude * -3;
                GetComponentInParent<FloaterParent>().DodgeObstacle(xDir);
            }

        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(myCollider.bounds.center, Vector2.down, rayColor);
    }

   
}
