using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    public Transform entranceSurface;
    public Transform exitSurface;
    public Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 m = exitSurface.localToWorldMatrix * entranceSurface.worldToLocalMatrix * playerCam.localToWorldMatrix;
        transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
