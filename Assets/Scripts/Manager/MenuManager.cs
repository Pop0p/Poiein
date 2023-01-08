using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public bool InGame;

    public static MenuManager Instance;
    public GameObject MenuPause;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    public void OnPause()
    {
        MenuPause.GetComponent<Pause>().UpdateDiscover();
        MenuPause.SetActive(true);
    }
    public void OnPlay()
    {
        if (!InGame)
            SceneManager.LoadScene(1);
        else
        {
            transform.Find("UI_PAUSE").gameObject.SetActive(false);
            GameManager.Instance.InPause = false;
        }
    }
    public void OnExit()
    {
        if (!InGame)
        {
            if (UnityEngine.Application.isPlaying)
                EditorApplication.ExitPlaymode();
            else
                Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
