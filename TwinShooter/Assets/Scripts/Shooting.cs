using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    

    [SerializeField]
    private float fInterval;
    [SerializeField]
    private float fDamage;
    [SerializeField]
    private float fShootSpd;
    Transform spawnPoint;

    private float collpaseTime;
    private bool bCDing;
    
    // Start is called before the first frame update
    void Start()
    {
        bCDing = false; 
    }

    // Update is called once per frame
    void Update()
    {
        CoolDown();
        if (Input.GetMouseButton(0) && !bCDing)
        {
            Fire();
        }
    }

    void CoolDown()
    {
        if (bCDing)
        {
            collpaseTime += Time.deltaTime;
            if (collpaseTime >= fInterval)
            {
                bCDing = false;
                collpaseTime = 0;
            }
        }

    }

    void Fire()
    {
        Instantiate(bulletPrefab);
    }
}
