using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowPlayerMovement : MonoBehaviour
{

    // 0 = stop Player Movement
    // 1 = Allow Player Movement
    [SerializeField] private int allowMoveIndex;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch (allowMoveIndex)
            {
                case 0:
                    collision.GetComponent<Player>().DisableMovement();
                    Destroy(GetComponent<Collider2D>());
                    break;
                case 1:
                    collision.GetComponent<Player>().AllowMovement();
                    Destroy(GetComponent<Collider2D>());
                    break;
            }
        }
    }
}
