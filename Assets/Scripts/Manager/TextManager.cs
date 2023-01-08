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

        Debug.Log("count : " + _count);
        if (_count == _len && !_affich_all)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Dans leur orgueil, ils ont appelé la mission « poiéin », du verbe « créer » en grec ancien.N’est - ce pas le plus grand des orgueils de se croire créateur et démiurge quand on détruit tout ? \n " +
                                                        "Et moi… Je me souviens maintenant. Je n’étais qu’un outil. Simple robot outil qui a survécu à l’humain.Simple robot qui doit reconstruire le monde. \n" +
                                                        "Ce n’est qu’un début.";

            gText.SetActive(true);
            _affich_all = true;
        }
        else if (_count >= _troisQuart && !_affich_troisQuart)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Oui, c’est ça. Le monde chutait… L’épuisement des ressources était inévitable. Les humains ne pouvaient pas se résoudre à disparaître. Ils créèrent des petites créatures capables de se reproduire à l’infini. Des créatures remplies des ressources naturelles qui s’épuisaient.";
            gText.SetActive(true);
            _affich_troisQuart = true;

        }
        else if (_count >= _demi && !_affich_demi)
        {
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Un laboratoire… grouillant de petites créatures élémentaires. De toutes les couleurs, de toutes les tailles, comme celles-ci. Mais pourquoi étaient-elles là ? Que sont-elles ? Et quel était mon rôle ?";
            gText.SetActive(true);
            _affich_demi = true;
        }
        else if (_count >= _quart && !_affich_quart)
        {
            Debug.Log("quart atteins");
            // afficher le text
            gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ces petits êtres qui grouillent… Ils me rappellent vaguement quelque chose. Je me souviens… une pièce étroite aux murs métalliques… des blouses blanches… des fioles. Un laboratoire !";
            Debug.Log(gText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            gText.SetActive(true);
            Debug.Log(gText.activeInHierarchy);
            _affich_quart = true;
        }

        _count = 0;
        Debug.Log("temps");
        yield return new WaitForSeconds(10f);
        Debug.Log("temps écoulé coucou");
        gText.SetActive(false);
        Debug.Log(gText.activeInHierarchy);
        _check = true;
    }
}
