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
    [SerializeField] private bool _weaponsHot = false;
    [SerializeField] private GameObject _tripShot;
    [SerializeField] private GameObject _tripShotIcon;
    [SerializeField] private ParticleSystem _muzzleFlash, _MFR, _MFL;
    [SerializeField] public float _canFire = -1f;
    [SerializeField] private float _fireRate = 5f;
    [SerializeField] private Object _myBullet;
    [SerializeField] private GameObject _muzzleEmpty;
    [SerializeField] private GameObject rotTarget;

    [SerializeField] private AudioSource _myAudioSource;
    [SerializeField] private AudioClip _shootClip, _tripShootClip;
    [SerializeField] private AudioClip _shellEjectClip, _tripShellEjectClip;
    [SerializeField] private AudioClip _sideCannonOnClip, _sideCannonOffClip;

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
        
    }

    void Update()
    {

       //RotTurret();

        FireBullet();
        
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
            _myAnimator.SetTrigger("FireMain");
            _muzzleFlash.Emit(1);
            if (!_tripShotActive)
            {
                Instantiate(_myBullet, _muzzleEmpty.transform.position, Quaternion.Euler(0,0,transform.eulerAngles.z));
                _myAudioSource.PlayOneShot(_shootClip);
            }
            else if (_tripShotActive)
            {
                Instantiate(_tripShot, transform.position, Quaternion.identity);
                _sideCannonAnimator.SetTrigger("tripleShot");
                _myAudioSource.PlayOneShot(_tripShootClip);
                _MFR.Emit(1);
                _MFL.Emit(1);

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
        _tripShotActive = true;
        _sideCannonAnimator.SetBool("TSActive", _tripShotActive);
        _myAudioSource.PlayOneShot(_sideCannonOnClip);
        _tripShotIcon.SetActive(true);
        _fireRate /= 2;
        StartCoroutine(TripShotCoolDown(5.0f));
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


}
