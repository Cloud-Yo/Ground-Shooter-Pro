using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private ParticleSystem _ShellParticle;
    [SerializeField] private ParticleSystem _PowerDownPS;
    [SerializeField] private Animator _sideCannonAnimator;
    [SerializeField] private Player _player;
    [SerializeField] private bool _tripShotActive;
    [SerializeField] private bool _grapeShotActive;
    [SerializeField] private bool _weaponsHot = false;
    [SerializeField] private GameObject _tripShot;
    [SerializeField] private GameObject _tripShotIcon;
    [SerializeField] private GameObject _grapeShot;
    [SerializeField] private GameObject _grapeShotIcon;
    [SerializeField] private ParticleSystem _muzzleFlash, _MFR, _MFL;
    [SerializeField] public float _canFire = -1f;
    [SerializeField] private float _fireRate = 5f;
    [SerializeField] private float _specialFireRate;
    [SerializeField] private Object _myBullet;
    [SerializeField] private GameObject _muzzleEmpty;
    [SerializeField] private GameObject rotTarget;
    [SerializeField] private int _ammoMain = 15, _currentAmmo, _specialAmmo;
    [SerializeField] private AmmoCounter _myAmmoCounter;

    [SerializeField] private AudioSource _myAudioSource;
    [SerializeField] private AudioClip _shootClip, _tripShootClip;
    [SerializeField] private AudioClip _shellEjectClip, _tripShellEjectClip;
    [SerializeField] private AudioClip _sideCannonOnClip, _sideCannonOffClip;
    [SerializeField] private AudioClip _noAmmoClip;

    void Start()
    {
        _muzzleEmpty = GameObject.Find("MuzzleFlash");
        _tripShotActive = false;
        _myAnimator = GetComponent<Animator>();
        _ShellParticle = GetComponent<ParticleSystem>();
        _myAnimator.ResetTrigger("FireMain");
        _myAudioSource = GetComponent<AudioSource>();
        _player = GetComponentInParent<Player>();
        _sideCannonAnimator = GameObject.Find("SideCannons").GetComponent<Animator>();
        _muzzleFlash = GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>();
        _MFR = GameObject.Find("MuzzleFlashRight").GetComponent<ParticleSystem>();
        _MFL = GameObject.Find("MuzzleFlashLeft").GetComponent<ParticleSystem>();
        _myAmmoCounter = GameObject.Find("TankUIPanel").GetComponent<AmmoCounter>();
        _currentAmmo = _ammoMain;
        _myAmmoCounter.UpdateAmmoCounter(_currentAmmo);
        _specialFireRate = _fireRate / 2;

        
    }

    void Update()
    {

       //RotTurret();

        FireBullet();
        if(_currentAmmo > 99)
        {
            _currentAmmo = 99;
        }
    }

    public void EjectShell()
    {
        if(_tripShotActive)
        {
            
            _ShellParticle.Play();
            _myAnimator.ResetTrigger("FireMain");
            _myAudioSource.PlayOneShot(_tripShellEjectClip);
        }
        else if(!_tripShotActive)
        {
          
            _ShellParticle.Emit(1);
            _myAnimator.ResetTrigger("FireMain");
            _myAudioSource.PlayOneShot(_shellEjectClip);
        }
        
    }

    private void FireBullet()
    {
        if (Input.GetButton("Fire1") && Time.time > _canFire && _weaponsHot)
        {
            _canFire = Time.time + _fireRate;
           

            if (!_tripShotActive && !_grapeShotActive && _currentAmmo > 0)
            {
                _myAnimator.SetTrigger("FireMain");
                _muzzleFlash.Emit(1);
                _currentAmmo--;
                _myAmmoCounter.UpdateAmmoCounter(_currentAmmo);
                Instantiate(_myBullet, _muzzleEmpty.transform.position, Quaternion.Euler(0,0,transform.eulerAngles.z));
                _myAudioSource.PlayOneShot(_shootClip);
            }
            else if (_tripShotActive)
            {
                if(_specialAmmo >= 3)
                {
                    _specialAmmo -= 3;
                    _myAmmoCounter.UpdateAmmoCounter(_specialAmmo);
                    Instantiate(_tripShot, transform.position, Quaternion.identity);
                    _sideCannonAnimator.SetTrigger("tripleShot");
                    _myAudioSource.PlayOneShot(_tripShootClip);
                    _MFR.Emit(1);
                    _MFL.Emit(1);
                }
                else if(_specialAmmo == 0)
                {
                    _tripShotActive = false;
                    _tripShotIcon.SetActive(false);
                    _myAudioSource.PlayOneShot(_sideCannonOffClip);
                    _player.PowerDownAudio();
                    _myAudioSource.PlayOneShot(_noAmmoClip, 0.7f);
                    _PowerDownPS.Emit(15);
                    _sideCannonAnimator.SetBool("TSActive", _tripShotActive);
                    _fireRate = _specialFireRate * 2;
                    ReloadAmmo(_ammoMain);
                }
 
            }
            else if(_grapeShotActive)
            {
               if(_specialAmmo >= 1)
                {
                    _specialAmmo--;
                    _myAmmoCounter.UpdateAmmoCounter(_specialAmmo);
                    Instantiate(_grapeShot, transform.position, Quaternion.identity);
                    _myAnimator.SetTrigger("FireMain");
                    _muzzleFlash.Emit(1);
                    _myAudioSource.PlayOneShot(_shootClip);
                }
               else if(_specialAmmo == 0)
                {
                    _myAudioSource.PlayOneShot(_noAmmoClip, 0.7f);
                    _grapeShotActive = false;
                    _grapeShotIcon.SetActive(false);
                    _player.PowerDownAudio();
                    _PowerDownPS.Emit(15);
                    ReloadAmmo(_ammoMain);

                }
            }
            else if ( _currentAmmo == 0)
            {
                _myAudioSource.PlayOneShot(_noAmmoClip, 0.7f);
            }

        }
    }

    private void RotTurret()
    {
        Vector3 dir = rotTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, new Vector3(0,0,1));
    }

    public void ActivateTripleShot()
    {
        if(_grapeShotActive)
        {
            _grapeShotActive = false;
            _grapeShotIcon.SetActive(false);
        }
        _tripShotActive = true;
        _sideCannonAnimator.SetBool("TSActive", _tripShotActive);
        _myAudioSource.PlayOneShot(_sideCannonOnClip);
        _tripShotIcon.SetActive(true);
        if(_fireRate != _specialFireRate)
        {
            _fireRate = _specialFireRate;
        }
        
        _specialAmmo = 24;
        _myAmmoCounter.UpdateAmmoCounter(_specialAmmo);
        _myAmmoCounter.ChangeAmmoColor(1);
        
     }

    IEnumerator TripShotCoolDown(float _time)
    {
        yield return new WaitForSeconds(_time);
        _tripShotActive = false;
        _tripShotIcon.SetActive(false);
        _myAudioSource.PlayOneShot(_sideCannonOffClip);
        _player.PowerDownAudio();
        _PowerDownPS.Emit(15);
        _sideCannonAnimator.SetBool("TSActive", _tripShotActive);
        _fireRate *= 2;
    }

    public void ActivateWeapons()
    {
        _weaponsHot = !_weaponsHot;
    }

    public void ReloadAmmo(int ammo)
    {
        
        _currentAmmo += ammo;
        if(!_tripShotActive && !_grapeShotActive)
        {
            _myAmmoCounter.UpdateAmmoCounter(_currentAmmo);
            _myAmmoCounter.ChangeAmmoColor(0);
        }
        
    }

    public void ActivateGrapeShot()
    {
        _grapeShotActive = true;
        if(_tripShotActive)
        {
            _tripShotIcon.SetActive(false);
            _tripShotActive = false;
            _myAudioSource.PlayOneShot(_sideCannonOffClip);
            _sideCannonAnimator.SetBool("TSActive", _tripShotActive);
            _fireRate *= 2;
        }
        _grapeShotIcon.SetActive(true);
        _specialAmmo = 12;
        _myAmmoCounter.UpdateAmmoCounter(_specialAmmo);
        _myAmmoCounter.ChangeAmmoColor(2);

    }
}
