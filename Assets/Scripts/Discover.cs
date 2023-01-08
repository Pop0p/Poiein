using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discover : MonoBehaviour
{
    private static Dictionary<TYPE, bool> _poiein;

    private void Awake()
    {
        _poiein = new Dictionary<TYPE, bool>();
        _poiein.Add(TYPE.Fire, false);
        _poiein.Add(TYPE.Water, false);
        _poiein.Add(TYPE.Rock, false);
        _poiein.Add(TYPE.Savage, false);
        _poiein.Add(TYPE.Soil, false);
        _poiein.Add(TYPE.Vegetal, false);
    }

    public static void SetDiscover(TYPE e)
    {
        Debug.Log("set discover");
        if (!_poiein[e])
        {
            _poiein[e] = true;
        }
    }

    public static Dictionary<TYPE, bool> GetDiscover()
    {
        return _poiein;
    }
}
