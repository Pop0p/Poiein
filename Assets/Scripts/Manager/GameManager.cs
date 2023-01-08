using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool InPause;


    public GameObject NeutreEau;
    public GameObject NeutreFeu;
    public GameObject NeutreVegetal;
    public GameObject NeutreRock;
    public GameObject NeutreSoil;

    public GameObject EauTerre; // CHECK
    public GameObject EauVegetal;

    public GameObject FeuTerre; // CHECK
    public GameObject FeuVegetal; // CHECK

    public GameObject RocheEau; // CHECK
    public GameObject RocheTerre; // CHECK
    public GameObject RocheVegetal; // CHECK
    public GameObject RocheFeu; // CHECK

    public GameObject TerreVegetal; // CHECK

    public GameObject CommandLeft;
    public GameObject CommandRight;
    public GameObject CommandKeyboard;
    public GameObject CommandIncubateur;
    private bool one;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }
    // Start is called before the first frame update
    void Start()
    {
        one = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InPause = !InPause;
            if (InPause)
                MenuManager.Instance.OnPause();
            else
                MenuManager.Instance.OnPlay();
        }

        if (one)
        {
            StartCoroutine(DisplayCommandLeft());
            one = false;
        }

    }

    private IEnumerator DisplayCommandLeft()
    {
        CommandLeft.SetActive(true);
        yield return new WaitForSeconds(5f);
        CommandLeft.SetActive(false);
        StartCoroutine(DisplayCommandRight());
    }

    private IEnumerator DisplayCommandRight()
    {
        CommandRight.SetActive(true);
        yield return new WaitForSeconds(5f); 
        CommandRight.SetActive(false);
        StartCoroutine(DisplayCommandKeyboard());
    }

    private IEnumerator DisplayCommandKeyboard()
    {
        CommandKeyboard.SetActive(true);
        yield return new WaitForSeconds(5f);
        CommandKeyboard.SetActive(false);
        StartCoroutine(DisplayCommandIncubateur());
    }

    private IEnumerator DisplayCommandIncubateur()
    {
        CommandIncubateur.SetActive(true);
        yield return new WaitForSeconds(5f);
        CommandIncubateur.SetActive(false);
    }
}
