using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject[] Dex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateDiscover()
    {
        Dictionary<TYPE, bool> discover = Discover.GetDiscover();
        Debug.Log("coucocu");
        Debug.Log("fire ? " + discover[TYPE.Fire]);
        foreach (var item in discover)
        {
            switch (item.Key)
            {
                case TYPE.Fire:
                    Dex[1].SetActive(item.Value);
                    break;
                case TYPE.Water:
                    Dex[0].SetActive(item.Value);
                    break;
                case TYPE.Soil:
                    Dex[2].SetActive(item.Value);
                    break;
                case TYPE.Rock:
                    Dex[4].SetActive(item.Value);
                    break;
                case TYPE.Vegetal:
                    Dex[3].SetActive(item.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
