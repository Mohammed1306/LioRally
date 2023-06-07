using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArretFinCourse : MonoBehaviour
{

    private List<GameObject> oldTriggers;

    private void Awake()
    {
        oldTriggers = new List<GameObject>(2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!oldTriggers.Contains(other.transform.parent.gameObject))
        {
            oldTriggers.Add(other.transform.parent.gameObject);
            other.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
