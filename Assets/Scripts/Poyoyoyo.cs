using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    private bool _inIncubateur;
    public bool TryFusion;
    private bool _first_ground = true;

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
                    DoJump(false);
                }
            }
        }
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InPause)
            return;
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

        if (_rb.velocity.y < 0)
        {
            float size = isTiny ? 0.75f : 1;
            float yScale = Mathf.Lerp(size, size * 2, Mathf.Abs(_rb.velocity.y) / 8);
            transform.localScale = new Vector3(size / yScale, yScale, size);
        }

        if (transform.position.y < -100)
        {
            gameObject.SetActive(false);
        }
    }

    private void DoJump(bool dir)
    {
        GetComponent<AudioSource>().PlayOneShot(MenuManager.Instance.JumpSound);
        _rb.velocity = _agent.velocity;
        _agentHadPath = _agent.hasPath;
        _agentDestination = _agent.destination;
        _agent.enabled = false;
        if (dir)
            _rb.AddForce(Vector3.forward + Random.insideUnitSphere * 4 + transform.up * 2, ForceMode.Impulse);
        else
            _rb.AddForce(Vector3.up * Random.Range(4, 9), ForceMode.Impulse);
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
                child.transform.parent = GameObject.Find("Poiens").transform;

            }
            transform.localScale = Vector3.one * 0.75f;
            hasSplitted = true;
            isTiny = true;
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
    public void OnIncubateurIn()
    {
        Catch = false;
        _rb.isKinematic = true;
        _agentHadPath = _agent.hasPath;
        _agentDestination = _agent.destination;
        _agent.enabled = false;
        _inIncubateur = true;
    }
    public void OnIncubateurOut()
    {
        _rb.isKinematic = false;
        _agent.enabled = true;
        if (_agentHadPath && _agent.isOnNavMesh)
            _agent.destination = _agentDestination;
        _inIncubateur = false;
        DoJump(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_inIncubateur)
            return;
        if (collision.transform.TryGetComponent(out Poyoyoyo comp) && _previousVelocity.y < 0)
        {
            _rb.AddForce(Random.insideUnitSphere * 2 + transform.up * 2, ForceMode.Impulse);
        }
        if (!Catch && collision.transform.CompareTag("Sol") && _previousVelocity.y < 0 && gameObject.activeSelf)
        {
            _agent.enabled = true;
            if (_agentHadPath && _agent.isOnNavMesh)
                _agent.destination = _agentDestination;

            if (_previousVelocity.y < 0)
            {
                if (_first_ground)
                {
                    _first_ground = false;
                    GetComponent<SlimeDeplacement>().enabled = true;
                    GetComponent<NavMeshAgent>().enabled = true;
                }
                StartCoroutine(Splash());
                return;
            }

        }



        if (collision.gameObject.layer >= 8)
        {
            Catch = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TryFusion && other.tag == "Tile")
        {
            Debug.Log(other.gameObject.layer);
            GameObject new_tile = null;
            Catch = false;
            TryFusion = false;
            bool pos = false;
            switch (Element)
            {
                case TYPE.Fire:
                    if (other.gameObject.layer == 8)
                        new_tile = Instantiate(GameManager.Instance.FeuTerre);
                    else if (other.gameObject.layer == 11)
                        new_tile = Instantiate(GameManager.Instance.FeuVegetal);
                    else if (other.gameObject.layer == 13)
                        new_tile = Instantiate(GameManager.Instance.NeutreFeu);
                    else
                        return;
                    new_tile.transform.position = other.transform.position;
                    other.gameObject.layer = 9;
                    break;
                case TYPE.Water:
                    if (other.gameObject.layer == 12)
                        new_tile = Instantiate(GameManager.Instance.RocheEau);
                    else if (other.gameObject.layer == 8)
                        new_tile = Instantiate(GameManager.Instance.EauTerre);
                    else if (other.gameObject.layer == 11)
                        new_tile = Instantiate(GameManager.Instance.EauVegetal);
                    else if (other.gameObject.layer == 13)
                        new_tile = Instantiate(GameManager.Instance.NeutreEau);
                    else
                        return;
                    pos = true;
                    new_tile.transform.position = other.transform.position - (Vector3.forward * 0.35f);
                    new_tile.gameObject.layer = 10;
                    break;
                case TYPE.Soil:
                    if (other.gameObject.layer == 9)
                        new_tile = Instantiate(GameManager.Instance.FeuTerre);
                    else if (other.gameObject.layer == 12)
                        new_tile = Instantiate(GameManager.Instance.RocheTerre);
                    else if (other.gameObject.layer == 11)
                        new_tile = Instantiate(GameManager.Instance.TerreVegetal);
                    else if (other.gameObject.layer == 10)
                        new_tile = Instantiate(GameManager.Instance.EauTerre);
                    else if (other.gameObject.layer == 13)
                        new_tile = Instantiate(GameManager.Instance.NeutreSoil);
                    else
                        return;
                    new_tile.transform.position = other.transform.position;

                    other.gameObject.layer = 8;
                    break;
                case TYPE.Vegetal:
                    if (other.gameObject.layer == 11)
                        new_tile = Instantiate(GameManager.Instance.FeuVegetal);
                    else if (other.gameObject.layer == 12)
                        new_tile = Instantiate(GameManager.Instance.RocheVegetal);
                    else if (other.gameObject.layer == 8)
                        new_tile = Instantiate(GameManager.Instance.TerreVegetal);
                    else if (other.gameObject.layer == 10)
                        new_tile = Instantiate(GameManager.Instance.EauVegetal);
                    else if (other.gameObject.layer == 13)
                        new_tile = Instantiate(GameManager.Instance.NeutreVegetal);
                    else
                        return;
                    new_tile.transform.position = other.transform.position;

                    other.gameObject.layer = 11;
                    break;
                case TYPE.Rock:
                    if (other.gameObject.layer == 10)
                        new_tile = Instantiate(GameManager.Instance.RocheEau);
                    else if (other.gameObject.layer == 11)
                        new_tile = Instantiate(GameManager.Instance.RocheVegetal);
                    else if (other.gameObject.layer == 8)
                        new_tile = Instantiate(GameManager.Instance.RocheTerre);
                    else if (other.gameObject.layer == 13)
                        new_tile = Instantiate(GameManager.Instance.NeutreRock);
                    else
                        return;
                    new_tile.transform.position = other.transform.position;

                    other.gameObject.layer = 12;
                    break;
                default:
                    break;
            }

            if (!pos)
                new_tile.transform.position = new Vector3(other.transform.position.x, 0.4f, other.transform.position.z);
            new_tile.transform.parent = GameObject.Find("Tiles").transform;
            if (new_tile.TryGetComponent<Spawner>(out Spawner sp))
            {
                sp.enabled = true;
                if (other.gameObject.layer != 13)
                {
                    switch (other.gameObject.layer)
                    {
                        case 8:
                            sp.Element = new TYPE[] { Element, TYPE.Soil };
                            break;
                        case 9:
                            sp.Element = new TYPE[] { Element, TYPE.Fire };
                            break;
                        case 10:
                            sp.Element = new TYPE[] { Element, TYPE.Water };
                            break;
                        case 11:
                            sp.Element = new TYPE[] { Element, TYPE.Vegetal };
                            break;
                        case 12:
                            sp.Element = new TYPE[] { Element, TYPE.Rock };
                            break;
                    }
                }
                else
                    sp.Element = new TYPE[] { Element };
            }
            other.gameObject.SetActive(false);
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
        Debug.Log("Splash !");
        float current = 0;
        float duration = Random.Range(0.25f, .35f);
        float x_splash = isTiny ? Random.Range(.8f, 1.4f) : Random.Range(1.4f, 2.8f);
        float z_splash = isTiny ? Random.Range(.8f, 1.4f) : Random.Range(1.4f, 2.8f);
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
        Debug.Log("Unsplash");
        float current = 0;
        float duration = Random.Range(0.25f, .35f);

        Vector3 begin = transform.localScale;
        while (true)
        {
            transform.localScale = Vector3.Lerp(begin, isTiny ? (Vector3.one * 0.75f) : Vector3.one, current / duration);
            current += Time.deltaTime;
            if (current > duration)
            {
                transform.localScale = isTiny ? (Vector3.one * 0.75f) : Vector3.one;
                break;
            }
            _unSplashing = true;
            yield return null;
        }
        _unSplashing = false;
        yield return null;
    }
}
