using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public string whatIDelete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(whatIDelete))
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
