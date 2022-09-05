using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    BoxCollider box;
    private void Start()
    {
        box = GetComponent<BoxCollider>();
        Destroy(box,0.3f);
    }

}
