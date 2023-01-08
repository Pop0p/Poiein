using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefabFire;
    [SerializeField] private GameObject _prefabWater;
    [SerializeField] private GameObject _prefabSoil;
    [SerializeField] private GameObject _prefabRock;
    [SerializeField] private GameObject _prefabVegetal;
    public TYPE[] Element;
    private float _timer;


    // Start is called before the first frame update
    void Start()
    {
        _timer = Random.Range(12, 24);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InPause)
            return;
        if (_timer <= 0)
        {
            GameObject pref = null;
            var el =  Element[Random.Range(0, Element.Length)];
            switch (el)
            {
                case TYPE.Fire:
                    pref = _prefabFire;
                    break;
                case TYPE.Water:
                    pref = _prefabWater;
                    break;
                case TYPE.Soil:
                    pref = _prefabSoil;
                    break;
                case TYPE.Rock:
                    pref = _prefabRock;
                    break;
                case TYPE.Vegetal:
                    pref = _prefabVegetal;
                    break;
            }
            GameObject cube = Instantiate(pref);

            // Utiliser un pool ?? + Ajouter un max ???
            var rand = Random.value;
            if (rand <= 0.95)
            {
                cube.GetComponent<Poyoyoyo>().isTiny = true;
                cube.transform.localScale = Vector3.one * 0.75f;
            }
            cube.GetComponent<Poyoyoyo>().Spawn(Random.onUnitSphere);
            cube.GetComponent<Poyoyoyo>().Element = el;
            cube.transform.parent = GameObject.Find("Poiens").transform;
            cube.transform.position = transform.position + (Vector3.up * 4);
            _timer = Random.Range(24, 60);
        }

        _timer -= Time.deltaTime;
    }
}
