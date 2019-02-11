using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!recallBody.BIsRecalling)
        {
            PerformMovement();
            PerformRotation();
        }
    }


    void PerformMovement()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        // movement
        Vector3 dir = (new Vector3(hori, 0, vert)).normalized;
        rb.velocity = dir * fMoveSpeed * Time.deltaTime;

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
