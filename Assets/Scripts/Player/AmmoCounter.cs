using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private Sprite[] _digiNumsArray;
    [SerializeField] private Sprite[] _digiMaxNumsArray;
    [SerializeField] private SpriteRenderer _digiNums01;
    [SerializeField] private SpriteRenderer _digiNums10;
    [SerializeField] private SpriteRenderer _digiMaxNums01;
    [SerializeField] private SpriteRenderer _digiMaxNums10;
    [SerializeField] private Color _mainCol, _TripShotCol, _grapeShotCol, _homingMissleCol;


    private void Start()
    {
        _digiNums01.color = _mainCol;
        _digiNums10.color = _mainCol;
        _digiMaxNums01.color = _mainCol;
        _digiMaxNums10.color = _mainCol;

    }

    public void UpdateAmmoCounter(int ammo)
    {
        if(ammo > 50)
        {
            ammo = 50;
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

    public void UpdateMaxAmmo(int max)
    {
        if (max > 50)
        {
            max = 50;
        }
        if (max <= 9)
        {
            _digiMaxNums10.sprite = _digiMaxNumsArray[0];
            _digiMaxNums01.sprite = _digiMaxNumsArray[max];

        }
        else if (max > 9)
        {
            int tens = max / 10;
            int ones = max % 10;
            _digiMaxNums10.sprite = _digiMaxNumsArray[tens];
            _digiMaxNums01.sprite = _digiMaxNumsArray[ones];
        }
    }

    public void ChangeAmmoColor(int type)
    {
        switch(type)
        {
            case 0: //main ammo color
                _digiNums01.color = _mainCol;
                _digiNums10.color = _mainCol;
                _digiMaxNums01.color = _mainCol;
                _digiMaxNums10.color = _mainCol;
                break;
            case 1: //triple shot color
                _digiNums01.color = _TripShotCol;
                _digiNums10.color = _TripShotCol;
                _digiMaxNums01.color = _TripShotCol;
                _digiMaxNums10.color = _TripShotCol;
                break;
            case 2: //grapeShot color
                _digiNums01.color = _grapeShotCol;
                _digiNums10.color = _grapeShotCol;
                _digiMaxNums01.color = _grapeShotCol;
                _digiMaxNums10.color = _grapeShotCol;
                break;
            case 3: //homing Missle color
                _digiNums01.color = _homingMissleCol;
                _digiNums10.color = _homingMissleCol;
                _digiMaxNums01.color = _homingMissleCol;
                _digiMaxNums10.color = _homingMissleCol;
                break;
            default:
                _digiNums01.color = _mainCol;
                _digiNums10.color = _mainCol;
                _digiMaxNums01.color = _mainCol;
                _digiMaxNums10.color = _mainCol;
                break;
        }
    }

}
