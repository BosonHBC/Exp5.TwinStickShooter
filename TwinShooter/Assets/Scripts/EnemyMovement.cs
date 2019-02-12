﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : Actor
{
    [SerializeField]
    GameObject bulletPrefab;

    [Header("Shooting")]
    // Shooting
    [SerializeField] private float fInterval;
    [SerializeField] private float fDamage;
    [SerializeField] private float fShootSpd;
    [SerializeField] private float fDiffuseSize;
    private Transform spawnPoint;
    private Transform bulletParent;
    private float collpaseTime;
    private bool bCDing;

    private NavMeshAgent agent;
    [SerializeField] private float fMaxDetectDist;
    private bool bSeeplayer;

    Transform playerTr;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTr = GameManager.instance.player.transform;
        bulletParent = GameManager.instance.bulletParent;
        spawnPoint = transform.GetChild(1);
        bCDing = true;
        InvokeRepeating("SetDestination", 0, 0.3f);


        bSeeplayer = true;
    }
    void SetDestination()
    {
        agent.SetDestination(playerTr.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (!bDead)
        {
            DetectPlayer();
            CoolDown();
        }

    }
    void DetectPlayer()
    {
        Ray ray = new Ray(spawnPoint.position, spawnPoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * fMaxDetectDist, Color.red);
        RaycastHit hit;
        bSeeplayer = false;
        if (Physics.Raycast(ray, out hit, fMaxDetectDist))
        {
            if (hit.collider.CompareTag("Player"))
            {
                bSeeplayer = true;
            }
        }
    }
    public override void Die()
    {
        base.Die();
        CancelInvoke("SetDestination");
        Destroy(agent);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<CapsuleCollider>().isTrigger = true;
        GetComponent<NavMeshObstacle>().enabled = true;
        gameObject.tag = "Untagged";
        bDead = true;

        GameManager.instance.GetScore(850);
        EnemySpawner.instance.SpawnEnemy();
    }
    public override void GetDamage(float _dmg)
    {
        base.GetDamage(_dmg);

        if (currentHp < maxHp * 0.8f)
            transform.GetChild(2).gameObject.SetActive(true);
    }

    IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this);
    }


    void Fire()
    {
        bCDing = true;
        if (bSeeplayer)
        {
            GameObject go = Instantiate(bulletPrefab);
            go.transform.SetParent(bulletParent);
            go.transform.position = spawnPoint.position;
            go.transform.rotation = spawnPoint.rotation;
            go.GetComponent<BulletMover>().SetBullet(fDamage, fShootSpd, fDiffuseSize);
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
                Fire();
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Actor actor = collision.collider.GetComponent<Actor>();
            if (!actor.bDead)
                actor.GetDamage(fDamage / 3);
        }
    }
}