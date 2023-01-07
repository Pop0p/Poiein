using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class Poyoyoyo : MonoBehaviour
{
    private Rigidbody _rb;
    private NavMeshAgent _agent;

    public float AspirationTime = 0.15f;
    private float _currentTime = 0;
    private Vector3 _position;
    private bool _grabed = false;
    private Transform _owner;

    public bool Catch;
    public TYPE Element;
    public bool isTiny;
    public bool hasSplitted;

    private Vector3 _previousVelocity;
    private Vector3 _agentDestination;
    private bool _agentHadPath;

    private bool _splashing;
    private bool _unSplashing;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        IEnumerator Jump()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(4, 16));
                if (_owner == null && _rb.velocity.y == 0)
                {
                    _rb.velocity = _agent.velocity;
                    _agentHadPath = _agent.hasPath;
                    _agentDestination = _agent.destination;
                    _agent.enabled = false;
                    _rb.AddForce(Vector3.up * Random.Range(4, 9), ForceMode.Impulse);
                }
            }
        }
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        if (_grabed)
        {
            if (_currentTime == 0)
                _position = transform.position;
            if (_currentTime < AspirationTime)
            {
                transform.position = Vector3.Lerp(_position, _owner.position - (Vector3.up * 0.75f), _currentTime / AspirationTime);
                _currentTime += Time.deltaTime;
            }
            else
            {
                transform.position = _owner.position - (Vector3.up * 0.75f);
            }
        }
        else
        {
            _currentTime = 0;
        }

        if (_rb.velocity.y != 0)
        {
            float yScale = Mathf.Lerp(1, 2f, Mathf.Abs(_rb.velocity.y) / 8);
            transform.localScale = new Vector3(1 / yScale, yScale, 1);
        }
    }

    private void FixedUpdate()
    {
        _previousVelocity = _rb.velocity;
    }

    public void OnArmIn()
    {
    }
    public void OnArmOut()
    {
    }
    public void OnGrabIn(Transform owner)
    {
        if (!isTiny && !hasSplitted)
        {
            for (var i = 0; i < 2; i++)
            {
                var child = Instantiate(gameObject);
                child.transform.localScale = Vector3.one * 0.75f;
                child.transform.position = transform.position;
                child.GetComponent<Poyoyoyo>().Spawn(UnityEngine.Random.onUnitSphere);
                child.GetComponent<Poyoyoyo>().isTiny = true;
                child.GetComponent<Poyoyoyo>().Catch = false;
                child.GetComponent<Poyoyoyo>()._owner = null;
                child.GetComponent<Poyoyoyo>()._grabed = false;

            }
            transform.localScale = Vector3.one * 0.75f;
            hasSplitted = true;
        }


        _rb.isKinematic = true;
        _grabed = true;
        _owner = owner;

        _rb.velocity = _agent.velocity;
        _agentHadPath = _agent.hasPath;
        _agentDestination = _agent.destination;
        _agent.enabled = false;

    }
    public void OnGrabOut()
    {
        _rb.isKinematic = false;
        _grabed = false;
        _owner = null;
    }
    public void OnThrow()
    {
        _rb.isKinematic = false;
        _grabed = false;
        _owner = null;
        _rb.AddForce(transform.up * -10, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Poyoyoyo comp) && _previousVelocity.y < 0)
        {
            _rb.AddForce(Random.insideUnitSphere * 2 + transform.up * 2, ForceMode.Impulse);
        }
        if (!Catch && collision.transform.CompareTag("Neutre") && _previousVelocity.y < 0)
        {
            _agent.enabled = true;
            if (_agentHadPath && _agent.isOnNavMesh)
                _agent.destination = _agentDestination;

            if (_previousVelocity.y < -.5f) ;
            StartCoroutine(Splash());

        }

        if (Catch && (collision.gameObject.layer == 13))
        {
            switch (Element)
            {
                case TYPE.Fire:
                    collision.gameObject.layer = 9;
                    collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case TYPE.Water:
                    collision.gameObject.layer = 10;
                    collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case TYPE.Soil:
                    collision.gameObject.layer = 8;
                    collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case TYPE.Vegetal:
                    collision.gameObject.layer = 11;
                    collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case TYPE.Rock:
                    collision.gameObject.layer = 12;
                    collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
                    break;
                default:
                    break;
            }
            collision.gameObject.GetComponent<Spawner>().NavMeshAgentTypeID = _agent.agentTypeID;
            collision.gameObject.GetComponent<Spawner>().enabled = true;
            collision.gameObject.GetComponent<Spawner>().Element = Element;
            Catch = false;
            gameObject.SetActive(false);
        }
    }
    public void Spawn(Vector3 direction)
    {
        GetComponent<Collider>().enabled = false;
        _agent.enabled = false;
        _rb.AddForce((Vector3.up * 6) + direction * Random.Range(2, 4), ForceMode.Impulse);
        IEnumerator RenableCollision()
        {
            yield return new WaitForSeconds(.1f);
            GetComponent<Collider>().enabled = true;
        }
        StartCoroutine(RenableCollision());

    }

    IEnumerator Splash()
    {
        if (_splashing)
            yield return null;
        float current = 0;
        float duration = Random.Range(0.15f, .4f);
        float x_splash = Random.Range(1.4f, 2.8f);
        float z_splash = Random.Range(1.4f, 2.8f);
        Vector3 begin = transform.localScale;

        while (true)
        {
            transform.localScale = Vector3.Lerp(begin, new Vector3(x_splash, 0, z_splash), current / duration);
            current += Time.deltaTime;
            if (current > duration)
            {
                StartCoroutine(UnSplash());
                break;
            }

            _splashing = true;
            yield return null;
        }
        _splashing = false;
        yield return null;
    }

    IEnumerator UnSplash()
    {
        if (_unSplashing)
            yield return null;
        float current = 0;
        float duration = Random.Range(0.25f, .45f);

        Vector3 begin = transform.localScale;
        while (true)
        {
            transform.localScale = Vector3.Lerp(begin, Vector3.one, current / duration);
            current += Time.deltaTime;
            if (current > duration)
                break;
            _unSplashing = true;
            yield return null;
        }
        _unSplashing = false;
        yield return null;
    }
}
