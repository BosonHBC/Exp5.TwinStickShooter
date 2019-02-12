using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecallBody : MonoBehaviour
{
    private bool bIsRecalling;
    public bool BIsRecalling
    {
        get
        {
            return bIsRecalling;
        }
    }
    private TimeControl control;

    [Header("CD")]
    // CD
    [SerializeField] private float recallCD;
    [SerializeField] private Image recallFill;
    private Text fillText;
    private float fCollpaseTime;
    private bool bCding;

    [Header("Recall")]
    [SerializeField] private float fMaxRecordTime = 3f;
    private List<Vector3> positions;
    private List<float> hps;

    private TrailRenderer tr;
    private CharacterMovement thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<TimeControl>();
        thePlayer = GetComponent<CharacterMovement>();
        positions = new List<Vector3>();
        hps = new List<float>();
        tr = GetComponent<TrailRenderer>();
        fillText = recallFill.transform.parent.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !bCding)
        {
            StartRecall();
        }
        RecallCD();

    }

    private void FixedUpdate()
    {
        if (bIsRecalling)
            Recall();
        else
            Record();
    }
    void Recall()
    {
        if (positions.Count > 0)
        {
            transform.position = positions[0];
            positions.RemoveAt(0);

            thePlayer.SetHp(hps[0]);
            hps.RemoveAt(0);
            float curr = thePlayer.GetHp();
            float max = thePlayer.GetMaxHp();
            PPController.instance.GetDamage(1f - curr / max);
            StatusControl.instance.SetHp(curr, max);
        }
        else
        {
            StopRecall();
        }
    }
    void Record()
    {
        positions.Insert(0, transform.position);
        hps.Insert(0, thePlayer.GetHp());
        if (positions.Count > Mathf.Round(fMaxRecordTime / Time.fixedUnscaledDeltaTime))
        {
            positions.RemoveAt(positions.Count - 1);
            hps.RemoveAt(hps.Count - 1);
        }

    }

    void StartRecall()
    {
        if (!control.BStopTime && !bIsRecalling)
        {
            thePlayer.IsHitable(false);
            bIsRecalling = true;
            Time.timeScale = 3f;
            tr.enabled = true;
            PPController.instance.FadeToDark(0.4f);
        }
    }
    void StopRecall()
    {
        if (bIsRecalling)
        {
            thePlayer.IsHitable(true);
            bIsRecalling = false;
            Time.timeScale = 1f;
            bCding = true;
            PPController.instance.FadeToNormal(0.2f);
            StartCoroutine(disableTr());
        }

    }

    IEnumerator disableTr()
    {
        yield return new WaitForSeconds(tr.time / 2);
        tr.enabled = false;
    }

    void RecallCD()
    {
        if (bCding)
        {
            fCollpaseTime += Time.deltaTime;
            fillText.text = ((int)(recallCD - fCollpaseTime) + 1).ToString();
            recallFill.fillAmount = 1 - fCollpaseTime / recallCD;
            if (fCollpaseTime >= recallCD)
            {
                bCding = false;
                fCollpaseTime = 0;
                fillText.text = "E";
            }

        }
    }
}
