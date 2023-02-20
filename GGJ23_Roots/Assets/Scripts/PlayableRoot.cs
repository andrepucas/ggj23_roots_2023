using System;
using System.Collections;
using UnityEngine;

public class PlayableRoot : MonoBehaviour
{
    public static event Action OnDead;
    public static event Action OnLevelEnd;

    [Header("TRAITS")]
    [SerializeField] private GameObject _moveBehaviour;
    [SerializeField] private Color _mainColor, _particlesColor;
    
    [Header("COMPONENTS")]
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _input;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particles;

    public IMoveBehaviour MoveBehaviour {get; set;}
    public Color MainColor {get; set;}
    public Color ParticlesColor {get; set;}

    private Transform _parent;
    private Collider2D _collider;
    private bool _isControllable;
    private ParticleSystem.MainModule _particlesModule;

    private void Awake()
    {
        _parent = transform.parent;
        _particlesModule = _particles.main;
        _collider = GetComponent<Collider2D>();
    }

    public void Reset()
    {
        transform.parent = _parent;
        _isControllable = false;
        _rb.velocity = Vector2.zero;

        MoveBehaviour = _moveBehaviour.GetComponent<IMoveBehaviour>();
        MainColor = _mainColor;
        ParticlesColor = _particlesColor;
        _collider.enabled = true;

        _input.SetActive(false);
        MoveBehaviour.Reset();
        UpdateTraits();
        StartCoroutine(ReenableTrail());
    }

    public void UpdateTraits()
    {
        _sprite.color = MainColor;
        _trail.startColor = MainColor;
        _particlesModule.startColor = ParticlesColor;
    }

    public void EnableInputHint() => _input.SetActive(true);
    
    public void Interact()
    {
        _isControllable = true;
        MoveBehaviour.HandleInput();
    }
    
    private void Update()
    {
        if (_isControllable) _rb.velocity += MoveBehaviour.Move();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Bounds")
        {
            if (col.gameObject.tag == "Obstacle") _audio.PlayOneShot(_clips[0], 1);
            else _audio.PlayOneShot(_clips[1], 1);
            
            _collider.enabled = false;
            _isControllable = false;
            _input.SetActive(false);

            OnDead?.Invoke();
        }

        else if (col.gameObject.tag == "EndLevel")
        {
            OnLevelEnd?.Invoke();
            _isControllable = false;
            _input.SetActive(false);
        }

        else if (col.gameObject.tag == "Zone")
        {
            if (col.GetComponent<IMoveBehaviour>().GetType().Name != MoveBehaviour.GetType().Name)
            {
                _audio.PlayOneShot(_clips[0], 1);

                _collider.enabled = false;
                _isControllable = false;
                _input.SetActive(false);

                OnDead?.Invoke();
            }
        }

        else if (col.gameObject.tag == "Water")
        {
            _audio.PlayOneShot(_clips[3], 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Root")
        {
            PlayableRoot other = col.gameObject.GetComponent<PlayableRoot>();

            if (other.GetInstanceID() < GetInstanceID()) return;

            _audio.PlayOneShot(_clips[2], 1);
            
            Debug.Log("BOOP");

            IMoveBehaviour auxMove = other.MoveBehaviour;
            Color auxMainColor = other.MainColor;
            Color auxPartColor = other.ParticlesColor;

            // Change other.
            other.MoveBehaviour = MoveBehaviour;
            other.MainColor = MainColor;
            other.ParticlesColor = ParticlesColor;

            // Change self.
            MoveBehaviour = auxMove;
            MainColor = auxMainColor;
            ParticlesColor = auxPartColor;

            UpdateTraits();
            other.UpdateTraits();
        }
    }

    public void Stop()
    {
        _isControllable = false;
        transform.parent = null;
        _rb.velocity = Vector2.zero;
    }

    private IEnumerator ReenableTrail()
    {
        _trail.time = 0;
        yield return new WaitForSeconds(.5f);
        _trail.time = 10;
    }
}
