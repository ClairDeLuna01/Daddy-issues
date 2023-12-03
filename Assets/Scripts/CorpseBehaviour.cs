using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseBehaviour : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // load the last save on click or when pressing r
        if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
        {
            gameManager.LoadSave();
            Destroy(gameObject);
        }
    }
}
