using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float fDeathDelay;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestroy", fDeathDelay);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
