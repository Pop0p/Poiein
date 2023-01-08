using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Incubateur : MonoBehaviour
{
    public Transform[] Emplacements;
    public Transform Retour;
    public GameObject[] TypePoiein;

    private int _nbPoiein;
    private GameObject[] _poieinFusion;

    private void Start()
    {
        _poieinFusion = new GameObject[2];
        _nbPoiein = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        Debug.Log(collision.gameObject.GetComponent<Poyoyoyo>().Catch);
        if (collision.gameObject.layer == 7 && (collision.transform.parent != Emplacements[0] && collision.transform.parent != Emplacements[1]) && collision.gameObject.GetComponent<Poyoyoyo>().Catch)
        {
            collision.transform.position = Emplacements[_nbPoiein].position;
            _poieinFusion[_nbPoiein] = collision.gameObject;
            collision.transform.parent = Emplacements[_nbPoiein];
            ++_nbPoiein;
        }

        Debug.Log(_nbPoiein);
        if (_nbPoiein == 2)
        {
            Fusion();
        }
    }

    private void Fusion()
    {
        TYPE element1 = _poieinFusion[0].GetComponent<Poyoyoyo>().Element;
        TYPE element2 = _poieinFusion[1].GetComponent<Poyoyoyo>().Element;

        int index = 0;

        switch (element1)
        {
            case TYPE.Fire:
                if (element2 == TYPE.Soil)
                {
                    index = 4;
                }
                break;
            case TYPE.Water:
                if (element2 == TYPE.Soil)
                {
                    index = 4;
                }
                break;
            case TYPE.Soil:
                if (element2 == TYPE.Water)
                {
                    index = 4;
                }
                else if (element2 == TYPE.Fire)
                {
                    index = 2;
                }
                break;
            default:
                Debug.Log("Pas bon");
                break;
        }

        GameObject g = Instantiate(TypePoiein[index]);
        g.transform.position = Retour.position;
        g.SetActive(true);

        _poieinFusion[0].SetActive(false);
        _poieinFusion[1].SetActive(false);
        _nbPoiein = 0;
    }
}
