using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryChecker : MonoBehaviour
{
    Projectile parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Projectile>();
    }

    public void Deflect()
    {
        parent.Deflect();
    }
}
