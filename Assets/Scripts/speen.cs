using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speen : MonoBehaviour
{
    public float speenSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speenSpeed * Time.deltaTime, 0);
    }
}
