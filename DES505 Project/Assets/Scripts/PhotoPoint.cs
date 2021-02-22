using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotoPoint : MonoBehaviour
{
    [Tooltip("player needs to take pictures within this distance")]
    public float photoDistance = 5f;
    [Tooltip("The radius of the sphere centered on the photo point")]
    public float photoRange = 3f;
    public LayerMask layerMask;

    public bool isFound { get; private set; }

    public UnityAction onFound;

    void Start()
    {
        PhotoManager.playerController.onPhoto += OnPhoto;
        isFound = false;
    }

    void OnPhoto(Ray ray)
    {
        float dist = Vector3.Distance(transform.position, ray.origin);
        if(dist < photoDistance)
        {
            float distPointToRay = Vector3.Cross(ray.direction, transform.position - ray.origin).magnitude;
            if(distPointToRay < photoRange)
            {
                if(!Physics.Raycast(ray, photoDistance, layerMask))
                {
                    isFound = true;
                    PhotoManager.playerController.onPhoto -= OnPhoto;
                    // call PhotoManager to record
                    if (onFound != null)
                        onFound();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, photoDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, photoRange);
    }
}
