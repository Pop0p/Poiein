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

    private void Awake()
    {
        Tiles = GameObject.FindGameObjectsWithTag("Neutre");
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

        _layersTiles = new int[_len];
        for (int i = 0; i < _len; i++)
        {
            _layersTiles[i] = Tiles[i].layer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckLayers());
    }


    private IEnumerator CheckLayers()
    {
        _change = false;
        for (int i = 0; i < _len; i++)
        {
            if (Tiles[i].layer != 13)
            {
                _count += 1;
            }
        }

        if (_count == _len && !_affich_all)
        {
            // afficher le text
            gText.GetComponent<TextMeshProUGUI>().text = "Dans leur orgueil, ils ont appelé la mission « poiéin », du verbe « créer » en grec ancien.N’est - ce pas le plus grand des orgueils de se croire créateur et démiurge quand on détruit tout ? \n " +
                                                        "Et moi… Je me souviens maintenant. Je n’étais qu’un outil. Simple robot outil qui a survécu à l’humain.Simple robot qui doit reconstruire le monde. \n" +
                                                        "Ce n’est qu’un début.";

            gText.transform.parent.gameObject.SetActive(true);
            _affich_all = true;
        }
        else if (_count >= _troisQuart && !_affich_troisQuart)
        {
            // afficher le text
            gText.GetComponent<TextMeshProUGUI>().text = "Oui, c’est ça. Le monde chutait… L’épuisement des ressources était inévitable. Les humains ne pouvaient pas se résoudre à disparaître. Ils créèrent des petites créatures capables de se reproduire à l’infini. Des créatures remplies des ressources naturelles qui s’épuisaient.";
            gText.transform.parent.gameObject.SetActive(true);
            _affich_troisQuart = true;

        }
        else if (_count >= _demi)
        {
            // afficher le text
            gText.GetComponent<TextMeshProUGUI>().text = "Un laboratoire… grouillant de petites créatures élémentaires. De toutes les couleurs, de toutes les tailles, comme celles-ci. Mais pourquoi étaient-elles là ? Que sont-elles ? Et quel était mon rôle ?";
            gText.transform.parent.gameObject.SetActive(true);
            _affich_demi = true;
        }
        else if (_count >= _quart)
        {
            // afficher le text
            gText.GetComponent<TextMeshProUGUI>().text = "Ces petits êtres qui grouillent… Ils me rappellent vaguement quelque chose. Je me souviens… une pièce étroite aux murs métalliques… des blouses blanches… des fioles. Un laboratoire !";
            gText.transform.parent.gameObject.SetActive(true);
            _affich_quart = true;
        }

        yield return new WaitForSeconds(8f);
        gText.transform.parent.gameObject.SetActive(false);
    }
}
