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
    public AudioSource SoundEffect;

    public AudioClip PauseSound;
    public AudioClip FusionSound;
    public AudioClip FusionFailedSound;
    public AudioClip JumpSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    public void OnFusion()
    {
        SoundEffect.PlayOneShot(FusionSound);
    }
    public void OnPause()
    {
        MenuPause.GetComponent<Pause>().UpdateDiscover();
        MenuPause.SetActive(true);
        SoundEffect.time = 3;
        SoundEffect.PlayOneShot(PauseSound);
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
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
