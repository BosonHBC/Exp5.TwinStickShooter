using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    private bool bStopTime;
    public bool BStopTime
    {
        get { return bStopTime; }
    }

    private RecallBody recall;
    // Start is called before the first frame update
    void Start()
    {
        recall = GetComponent<RecallBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StopTime();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ResumeTime();
    }
    public void StopTime()
    {
        if (!recall.BIsRecalling && !bStopTime)
        {
            bStopTime = true;
            PPController.instance.FadeToDark(0.4f);
            GetComponent<TrailRenderer>().enabled = true;
            Time.timeScale = 0.1f;
        }
    }

    public void SlowdownTime(float _scale)
    {

    }

    public void ResumeTime()
    {
        if (bStopTime)
        {
            bStopTime = false;
            PPController.instance.FadeToNormal(0.2f);
            GetComponent<TrailRenderer>().enabled = false;
            Time.timeScale = 1f;
        }

    }
}
