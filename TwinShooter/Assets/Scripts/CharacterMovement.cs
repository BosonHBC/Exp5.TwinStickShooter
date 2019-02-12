using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : Actor
{
    [Header("Movement")]
    [SerializeField]
    private float fMoveSpeed;


    // private
    private Rigidbody rb;
    private Transform head;
    private Transform body;
    private RecallBody recallBody;
    [HideInInspector] public Vector3 lookPoint;
    // Start is called before the first frame update
    void Start()
    {
        head = transform.GetChild(0);
        body = transform.GetChild(1);
        rb = GetComponent<Rigidbody>();
        recallBody = GetComponent<RecallBody>();
        StatusControl.instance.SetHp(currentHp, maxHp);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!bDead)
        {
            if (!recallBody.BIsRecalling)
            {
                PerformMovement();
                PerformRotation();
            }
        }

        
    }

    public override void Die()
    {
        base.Die();
        transform.GetChild(0).GetComponent<Shooting>().enabled = false;
        GameManager.instance.GameOver();
        rb.constraints = RigidbodyConstraints.None;
    }

    public float GetHp()
    {
        return currentHp;
    }
    public float GetMaxHp()
    {
        return maxHp;
    }
    public void SetHp(float _currentHp)
    {
        currentHp = _currentHp;
    }
    public void IsHitable(bool _hitable)
    {
        GetComponent<CapsuleCollider>().isTrigger = !_hitable;
    }

    public override void GetDamage(float _dmg)
    {
        base.GetDamage(_dmg);
        StatusControl.instance.SetHp((int)currentHp, maxHp);
        PPController.instance.GetDamage(1f - currentHp/ maxHp);
    }



    void PerformMovement()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        // movement
        Vector3 dir = (new Vector3(hori, 0, vert)).normalized;
        rb.velocity = dir * fMoveSpeed * (1/Time.timeScale) * Time.fixedUnscaledDeltaTime;
       // transform.position += dir * fMoveSpeed / 50 * Time.fixedDeltaTime;
        // body rotation
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir, transform.up);
            body.rotation = Quaternion.Lerp(body.rotation, rot, 0.1f);
        }
        //transform.position += dir* fMoveSpeed * Time.deltaTime;
    }

    void PerformRotation()
    {
        head.LookAt(lookPoint, transform.up);
        head.eulerAngles = new Vector3(0, head.eulerAngles.y, 0);
    }
}
