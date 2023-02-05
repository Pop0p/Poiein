using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float SmoothTime;
    Vector3 TargetPosition;
    Vector3 MoveVelocity = Vector3.zero;

    [SerializeField] private Transform RotationTarget;
    private float TargetRotation;
    [SerializeField] private float RotationSmoothTime;
    float RotationVelocity = 0;


    private void Awake()
    {
        TargetPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InPause)
            return;
        float x = Convert.ToSingle(Input.GetKey(KeyCode.D)) - Convert.ToSingle(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A));
        float z = Convert.ToSingle(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W)) - Convert.ToSingle(Input.GetKey(KeyCode.S));
        if (x != 0 || z != 0)
            TargetPosition = transform.position + new Vector3(x * 2, 0, z * 2);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            TargetRotation -= 45f;
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TargetRotation += 45f;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref MoveVelocity, SmoothTime);
        RotationTarget.rotation = Quaternion.Euler(RotationTarget.eulerAngles.x, Mathf.SmoothDampAngle(RotationTarget.eulerAngles.y, TargetRotation, ref RotationVelocity, RotationSmoothTime), RotationTarget.eulerAngles.z);
    }
}
