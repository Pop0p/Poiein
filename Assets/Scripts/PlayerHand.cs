using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private float SmoothTime;
    [SerializeField] readonly int layerMask = (1 << 3 | 1 << 7 | 1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12 | 1 << 13);

    public Animator _animator;

    public bool has_one = false;
    public bool had_one = false;
    bool must_throw = false;
    public Poyoyoyo Current_Poyo;
    public Poyoyoyo Previous_Poyo;

    Vector3 MoveVelocity = Vector3.zero;

    private void Awake()
    {
        _animator= GetComponent<Animator>();
    }
    private void Update()
    {
        if (GameManager.Instance.InPause)
            return;
        if (Current_Poyo != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!has_one)
                {
                    Current_Poyo.Catch = true;
                    Current_Poyo.OnGrabIn(gameObject.transform);
                    has_one = true;
                    Discover.SetDiscover(Current_Poyo.Element);
                    _animator.SetBool("CATCH", true);
                    Current_Poyo.TryFusion = false;

                }
                else
                {
                    Current_Poyo.TryFusion = false;
                    Current_Poyo.Catch = false;
                    has_one = false;
                    Current_Poyo.OnGrabOut();
                    _animator.SetBool("CATCH", false);
                    //Current_Poyo = null;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                _animator.SetBool("CATCH", false);
                Current_Poyo.Catch = false;
                Current_Poyo.TryFusion = true;
                must_throw = true;
            }
        }

        if (had_one && !has_one)
            GameObject.FindGameObjectWithTag("Incubateur").GetComponent<Incubateur>().Drop();

        had_one = has_one;
        Previous_Poyo = Current_Poyo;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.InPause)
            return;
        if (Current_Poyo != null)
            Current_Poyo.OnArmOut();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            transform.position = Vector3.SmoothDamp(transform.position, hit.point + Vector3.up * 2.5f, ref MoveVelocity, SmoothTime);
            if (hit.transform.gameObject.layer == 7 && hit.transform.TryGetComponent(out Poyoyoyo c) && !has_one)
            {
                Current_Poyo = c;
                Current_Poyo.OnArmIn();
            } else if (!has_one)
            {
                Current_Poyo = null;
            }
        }

        if (Current_Poyo != null && must_throw && has_one)
        {
            has_one = false;
            Current_Poyo.OnThrow();
            must_throw = false;
        }
    }


}
