using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    public string keycardTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject door in GameObject.FindGameObjectsWithTag(keycardTag))
            {
                door.SetActive(false);
            }

            Destroy(gameObject);
        }
    }
}
