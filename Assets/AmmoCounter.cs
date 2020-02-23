using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private Sprite[] _digiNumsArray;
    [SerializeField] private SpriteRenderer _digiNums01;
    [SerializeField] private SpriteRenderer _digiNums10;
    [SerializeField] private Color _mainCol, _TripShotCol, _grapeShotCol;


    private void Start()
    {
        _digiNums01.color = _mainCol;
        _digiNums10.color = _mainCol;
    }

    public void UpdateAmmoCounter(int ammo)
    {
        if(ammo > 99)
        {
            ammo = 99;
        }
        if(ammo <= 9)
        {
            _digiNums10.sprite = _digiNumsArray[0];
            _digiNums01.sprite = _digiNumsArray[ammo];

        }
        else if(ammo > 9)
        {
            int tens = ammo / 10;
            int ones = ammo % 10;
            _digiNums10.sprite = _digiNumsArray[tens];
            _digiNums01.sprite = _digiNumsArray[ones];
        }
    }

    public void ChangeAmmoColor(int type)
    {
        switch(type)
        {
            case 0: //main ammo color
                _digiNums01.color = _mainCol;
                _digiNums10.color = _mainCol;
                break;
            case 1: //triple shot color
                _digiNums01.color = _TripShotCol;
                _digiNums10.color = _TripShotCol;
                break;
            case 2: //grapeShot color
                _digiNums01.color = _grapeShotCol;
                _digiNums10.color = _grapeShotCol;
                break;
            default:
                _digiNums01.color = _mainCol;
                _digiNums10.color = _mainCol;
                break;
        }
    }

}
