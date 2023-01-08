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

    private bool shrinking = false;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHand ph))
        {
            if (_playerHand.has_one)
            {
                ph.Current_Poyo.OnGrabOut();
                ph.Current_Poyo = null;
                ph._animator.SetBool("CATCH", false);
                _playerHand.Previous_Poyo.OnIncubateurIn();
                ph.has_one = false;
                _playerHand.Previous_Poyo.transform.position = Emplacements[_nbPoiein].position;
                _poieinFusion[_nbPoiein] = _playerHand.Previous_Poyo.gameObject;
                ++_nbPoiein;

                if (_nbPoiein == 2)
                {
                    StartCoroutine(Fusion());
                }
            }
        }
    }


    IEnumerator Fusion()
    {
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
            yield return new WaitForSeconds(.75f);
            _poieinFusion[0].GetComponent<Poyoyoyo>().OnIncubateurOut();
            _poieinFusion[1].GetComponent<Poyoyoyo>().OnIncubateurOut();

            _poieinFusion[0] = null;
            _poieinFusion[1] = null;
            _nbPoiein = 0;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(MenuManager.Instance.FusionSound);
            StartCoroutine(SHRINK());
            yield return new WaitForSeconds(3.75f);
            GameObject g = Instantiate(TypePoiein[index]);
            g.transform.position = Retour.position;
            g.GetComponent<Poyoyoyo>().OnIncubateurOut();
            g.transform.parent = GameObject.Find("Poiens").transform;

            _poieinFusion[0].SetActive(false);
            _poieinFusion[1].SetActive(false);
            _poieinFusion[0] = null;
            _poieinFusion[1] = null;
            _nbPoiein = 0;
        }
    }

    IEnumerator SHRINK()
    {
        if (shrinking)
            yield return null;

        shrinking = true;
        Vector3 start_one = _poieinFusion[0].transform.localScale;
        Vector3 start_two = _poieinFusion[1].transform.localScale;

        Vector3 start_pos_one = _poieinFusion[0].transform.position;
        Vector3 start_pos_two = _poieinFusion[1].transform.position;
        float current = 0;
        float duration = 3.75f;
        while (true)
        {
            _poieinFusion[0].transform.localScale = Vector3.Lerp(start_one, Vector3.zero , current / duration);
            _poieinFusion[1].transform.localScale = Vector3.Lerp(start_two, Vector3.zero , current / duration);

            _poieinFusion[0].transform.position = Vector3.Lerp(start_pos_one, start_pos_one + (Vector3.down * 1.5f), current / (duration * 2));
            _poieinFusion[1].transform.position = Vector3.Lerp(start_pos_two, start_pos_two + (Vector3.down * 1.5f), current / (duration * 2));


            if (current >= duration)
                break;

            current += Time.deltaTime;
            yield return null;
        }
        shrinking = true;
        yield return null;
    }
}
