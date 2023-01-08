using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private GameObject[] Tiles;
    private int[] _layersTiles;

    private int _len;
    private int _quart;
    private int _demi;
    private int _troisQuart;
    private bool _change;

    private int _count;

    private bool _affich_all;
    private bool _affich_demi;
    private bool _affich_quart;
    private bool _affich_troisQuart;

    public GameObject gText;
    private bool _check;

    private void Awake()
    {
        Tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    // Start is called before the first frame update
    void Start()
    {
        _len = Tiles.Length;
        _quart = _len / 4;
        _demi = _len / 2;
        _troisQuart = _len * 3 / 4;
        _affich_all = false;
        _affich_demi = false;
        _affich_quart = false;
        _affich_troisQuart = false;
        _count = 0;

        _layersTiles = new int[_len];
        for (int i = 0; i < _len; i++)
        {
            _layersTiles[i] = Tiles[i].layer;
        }

        _check = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_check)
        {
            _check = false;
            StartCoroutine(CheckLayers());
        }
        // gText.SetActive(false);
    }


    private IEnumerator CheckLayers()
    {
        // _change = false;
        Tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].activeInHierarchy && (Tiles[i].layer != 13))
            {
                _count += 1;
            }
        }

        if (_count == _len && !_affich_all)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "In their pride, they called the mission “poiein”, from the verb “to create” in ancient Greek. Isn't it the greatest pride to believe oneself creator and demiurge when one destroys everything? \n " +
                                                        "And I... I remember now. I was just a tool. Simple robot tool that has survived the human. Simple robot who must rebuild the world. \n" +
                                                        "And it's only a beginning.";

            gText.SetActive(true);
            _affich_all = true;
        }
        else if (_count >= _troisQuart && !_affich_troisQuart)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Yes that's it. The world was falling… Resource depletion was inevitable. Humans couldn't bring themselves to disappear. They created small creatures capable of infinite reproduction. Creatures filled with natural resources that were running out.";
            gText.SetActive(true);
            _affich_troisQuart = true;

        }
        else if (_count >= _demi && !_affich_demi)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "A laboratory… crawling with small elemental creatures. All colors, all sizes, like these ones. But why these things were there? What are they? And what was my role?";
            gText.SetActive(true);
            _affich_demi = true;
        }
        else if (_count >= _quart && !_affich_quart)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "These little beings that swarm… They vaguely remind me of something. I remember…a narrow room with metal walls…white coats…vials. A laboratory !";
            Debug.Log(gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            gText.SetActive(true);
            Debug.Log(gText.activeInHierarchy);
            _affich_quart = true;
        }

        _count = 0;
        yield return new WaitForSeconds(10f);
        gText.SetActive(false);
        _check = true;
    }
}
