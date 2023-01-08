using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Incubateur : MonoBehaviour
{
    public Transform[] Emplacements;
    public Transform Retour;
    public GameObject[] TypePoiein;

    private int _nbPoiein;
    private GameObject[] _poieinFusion;

    private bool collisionWithHand;
    private PlayerHand _playerHand;

    private void Awake()
    {
        _playerHand = GameObject.FindGameObjectWithTag("Hand").GetComponent<PlayerHand>();
    }
    private void Start()
    {
        _poieinFusion = new GameObject[2];
        _nbPoiein = 0;
    }

    public void Drop()
    {
        if (collisionWithHand && !_playerHand.has_one && _playerHand.had_one)
        {
            _playerHand.Previous_Poyo.OnIncubateurIn();
            _playerHand.Previous_Poyo.transform.position = Emplacements[_nbPoiein].position;
            _poieinFusion[_nbPoiein] = _playerHand.Previous_Poyo.gameObject;
            ++_nbPoiein;

            if (_nbPoiein == 2)
            {
                StartCoroutine(Fusion());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHand ph))
        {
            collisionWithHand = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHand ph))
        {
            collisionWithHand = false;
        }
    }

    IEnumerator Fusion()
    {
        yield return new WaitForSeconds(.75f);
        TYPE element1 = _poieinFusion[0].GetComponent<Poyoyoyo>().Element;
        TYPE element2 = _poieinFusion[1].GetComponent<Poyoyoyo>().Element;

        int index = -1;

        // EAU + SOIL = VEGETAL
        // EAU + FIRE = ROCK
        // SOIL + FIRE = ROCK
        // FIRE + VEGETAL = SOIL
        switch (element1)
        {
            case TYPE.Water:
                if (element2 == TYPE.Soil)
                    index = 4;
                else if (element2 == TYPE.Fire)
                    index = 2;
                break;
            case TYPE.Soil:
                if (element2 == TYPE.Water)
                    index = 4;
                else if (element2 == TYPE.Fire)
                    index = 2;
                break;
            case TYPE.Fire:
                if (element2 == TYPE.Water || element2 == TYPE.Soil)
                    index = 2;
                if (element2 == TYPE.Vegetal)
                    index = 1;
                break;
            case TYPE.Vegetal:
                if (element2 == TYPE.Fire)
                    index = 1;
                break;
        }

        if (index == -1)
        {
            _poieinFusion[0].GetComponent<Poyoyoyo>().OnIncubateurOut();
            _poieinFusion[1].GetComponent<Poyoyoyo>().OnIncubateurOut();

            _poieinFusion[0] = null;
            _poieinFusion[1] = null;
            _nbPoiein = 0;
        }
        else
        {
            GameObject g = Instantiate(TypePoiein[index]);
            g.transform.position = Retour.position;
            g.GetComponent<Poyoyoyo>().OnIncubateurOut();


            _poieinFusion[0].SetActive(false);
            _poieinFusion[1].SetActive(false);
            _poieinFusion[0] = null;
            _poieinFusion[1] = null;
            _nbPoiein = 0;
        }
    }
}
