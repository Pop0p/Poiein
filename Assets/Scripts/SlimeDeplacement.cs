using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SlimeDeplacement : MonoBehaviour
{
    private NavMeshAgent _agent;
    public int Speed;
    private GameObject[] Arrival;
    private int _rand;


    int _try = 0;
    [SerializeField] private GameObject _currentPath = null;

    private bool _idle;

    private void Awake()
    {
        Arrival = GameObject.FindGameObjectsWithTag("Arrival");
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = Speed;
    }
    void Start()
    {
        GetPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InPause || !_agent.isOnNavMesh)
            return;
        if (!_agent.pathPending && _agent.enabled)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_idle)
                    StartCoroutine(SearchForPath());
            }
        }
    }

    IEnumerator SearchForPath()
    {
        _idle = true;
        yield return new WaitForSecondsRealtime(Random.Range(3, 16));
        GetPath();
    }

    private void GetPath()
    {
        _try += 1;
        if (_agent.enabled && _agent.isOnNavMesh)
        {
            var availables = Arrival.Where(x => x.gameObject != _currentPath).ToArray();
            _rand = Random.Range(0, availables.Length);
            _currentPath = availables[_rand].gameObject;
            _idle = false;

            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(_currentPath.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                _agent.SetDestination(availables[_rand].transform.position);
                _try = 0;
            }
            else
            {
                if (_try < 3)
                    GetPath();
                else
                    StartCoroutine(SearchForPath());

            }

        }
        else
        {
            StartCoroutine(SearchForPath());
        }
    }
}
