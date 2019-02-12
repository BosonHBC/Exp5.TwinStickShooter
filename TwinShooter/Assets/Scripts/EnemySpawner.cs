using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    //public static float timeScale = 1;
    private void Awake()
    {
        if (instance == null || instance != this)
            instance = this;
    }

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float radius;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(enemyPrefab);
            go.transform.position = RandomNavmeshLocation(radius);
            go.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        int max = Random.Range(0, 3);
        for (int i = 0; i < max; i++)
        {
            GameObject go = Instantiate(enemyPrefab);
            go.transform.position = RandomNavmeshLocation(radius);
            go.transform.SetParent(transform);
        }

    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
