using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public float minTime;
    public float maxTime;

    public GameObject[] objects;
    public GameObject[] rareObjects;

    public float rareObjectPCT = 0.01f;
    public bool canSpawn = false;
    public bool startSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        if(rareObjects.Length <= 0)
        {
            rareObjectPCT = -1f;
        }

        if (startSpawn)
        {
            spawn();
        }
        else
        {
            StartCoroutine(coolDown());
        }
    }

    public void spawn()
    {
        int index = Random.Range(0, objects.Length - 1);
        GameObject spawnIn;
        if ((float) Random.Range(0f, 1.0f) <= rareObjectPCT)
        {
            if (rareObjects.Length == 1)
            {
                index = 0;
            }
            else
            {
                index = Random.Range(0, rareObjects.Length - 1);
            }
            Debug.Log("Error");
            spawnIn = rareObjects[index];
            Debug.Log("I've spawned a surprise!");
        } else
        {
            spawnIn = objects[index];
        }

        GameObject.Instantiate(spawnIn, transform.position, Quaternion.Euler(-90, Random.Range(-180,180), spawnIn.transform.rotation.z));

        StartCoroutine(coolDown());
    }

    IEnumerator coolDown()
    {
        yield return new WaitForSeconds((float) Random.Range(minTime, maxTime));
        spawn();
    }
}
