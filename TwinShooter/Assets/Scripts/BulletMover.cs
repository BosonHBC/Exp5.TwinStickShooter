using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private Rigidbody rb;
    private float fDamage;
    private float fShootSpd;

    [SerializeField]
    private GameObject particlePrefab;

    private float fCounter;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.forward;
        rb.velocity = dir * fShootSpd;

        fCounter += Time.deltaTime;

        if(fCounter > 10f)
            Destroy(gameObject);

    }

    public void SetBullet(float _dmg, float _spd)
    {
        fDamage = _dmg;
        fShootSpd = _spd;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particle = Instantiate(particlePrefab);
        particle.transform.position = collision.contacts[0].point;
        particle.transform.SetParent(transform.parent);
        Destroy(gameObject);
    }
}
