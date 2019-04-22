using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Actor
{
    [Header("Turret")]
    [SerializeField] float fPatrolAngle;
    [SerializeField] float fHeight;
    [SerializeField] float fDetectDelta;
    [SerializeField] float fDetectSpeed;
   [SerializeField] LineRenderer[] Lrs;
    [SerializeField] float fRecoverTime = 10f;
    [SerializeField] Transform HpBar ;
    Transform camTr;
    float fAngleInRad;
    float fCollpaseTime;
    float fLengthOfLr;
    float fHeightOfLr;
    float[] fRandomInterval;

    [SerializeField] MeshRenderer[] mr;
    [SerializeField] Material m_DieEye;
    [SerializeField] Material m_Eye;
    // Start is called before the first frame update
    void Start()
    {
        fRandomInterval = new float[Lrs.Length];
        fAngleInRad = fPatrolAngle * Mathf.PI / 180f;
        fLengthOfLr = fHeight / Mathf.Cos(fAngleInRad);
        fHeightOfLr = -fHeight * Mathf.Sin(fAngleInRad);
        camTr = Camera.main.transform;
        for (int i = 0; i < Lrs.Length; i++)
        {
            fRandomInterval[i] = Random.Range(0, 90);
            Lrs[i].SetPosition(1, new Vector3(0, fHeightOfLr, fLengthOfLr));
        }

    }

    public override void GetDamage(float _dmg)
    {
        base.GetDamage(_dmg);

        HpBar.transform.GetChild(0).GetComponent<Image>().fillAmount = currentHp / maxHp;

    }

    public override void Die()
    {
        base.Die();
        HpBar.gameObject.SetActive(false);
        GameManager.instance.GetScore(1150);

        for (int i = 0; i < mr.Length; i++)
        {
            mr[i].material = m_DieEye;
        }

        for (int i = 0; i < Lrs.Length; i++)
        {
            Lrs[i].SetPosition(1, Vector3.zero);
        }
            StartCoroutine(IE_Recover());
    }
    IEnumerator IE_Recover()
    {
        yield return new WaitForSeconds(fRecoverTime);
        HpBar.gameObject.SetActive(true);
        bDead = false;
        currentHp = maxHp;
        HpBar.transform.GetChild(0).GetComponent<Image>().fillAmount = currentHp / maxHp;
        for (int i = 0; i < mr.Length; i++)
        {
            mr[i].material = m_Eye;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!bDead)
        {
            HpBar.LookAt(camTr);
            RayCastToPlayer();
            SetLrPositions();
        }
        
    }

    public void SetLrPositions()
    {
        fCollpaseTime += fDetectSpeed * Time.deltaTime;
        for (int i = 0; i < Lrs.Length; i++)
        {
            Lrs[i].SetPosition(1, new Vector3(Mathf.Sin(fCollpaseTime + fRandomInterval[i]) *fDetectDelta , fHeightOfLr, fLengthOfLr));
        }
    }

    private void RayCastToPlayer()
    {
        for (int i = 0; i < Lrs.Length; i++)
        {
            Vector3 refPos = Lrs[i].transform.position;
            Vector3 startPos = Lrs[i].GetPosition(0) + refPos;
            Vector3 endPos = Lrs[i].GetPosition(1) + refPos;
            Vector3 dir = endPos - startPos;

            dir = Quaternion.Euler(0, Lrs[i].transform.localEulerAngles.y, 0) * dir;

            Ray ray = new Ray(startPos, dir);
            Debug.DrawRay(ray.origin,ray.direction);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, fLengthOfLr))
            { 
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<CharacterMovement>().GetDamage(1f);
                }
            }
        }
    }
}
