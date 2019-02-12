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

   private int collisionCount;
   private Vector3 dir;
    private float fCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce (dir * fShootSpd, ForceMode.Impulse);

        fCounter += Time.deltaTime;

        if(fCounter > 10f)
            Destroy(gameObject);

    }

    public void SetBullet(float _dmg, float _spd, float _diffuse)
    {
        dir = (transform.forward + Random.Range(-_diffuse, _diffuse) * transform.right).normalized;
        fDamage = _dmg;
        fShootSpd = _spd;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particle = Instantiate(particlePrefab);
        particle.transform.position = collision.contacts[0].point;
        particle.transform.SetParent(transform.parent);

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
        {
            Actor actor = collision.collider.GetComponent<Actor>();
            if (actor )
            {
                if (!actor.bDead)
                {
                    actor.GetDamage(fDamage);
                }
            }
            else
            {
                Debug.LogError("No Actor attach to the actor object!");
            }
            Destroy(gameObject);
        }
        else
        {
            dir = collision.contacts[0].normal.normalized;
            collisionCount++;
        }
        if(collisionCount >= 3)
        {
            Destroy(gameObject);
        }
    }
}
