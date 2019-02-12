using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public static TimeControl instance;
    //public static float timeScale = 1;
    private void Awake()
    {
        if (instance == null || instance != this)
            instance = this;
    }

    private bool bStopTime;
    public bool BStopTime
    {
        get { return bStopTime; }
    }

    [SerializeField] float fMaxStopTime = 10;
    private float currentTime;

    [SerializeField] private float fSlowdownFactor = 0.2f;

    private RecallBody recall;
    // Start is called before the first frame update
    void Start()
    {
        recall = GetComponent<RecallBody>();
        currentTime = fMaxStopTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            StopTime();
        if (Input.GetMouseButtonUp(1))
            ResumeTime();

        if (!bStopTime)
        {
            if (currentTime < fMaxStopTime)
            {
                currentTime += Time.deltaTime;
                StatusControl.instance.SetStopBar(currentTime / fMaxStopTime);
            }
            else
                currentTime = fMaxStopTime;
        }
        else
        {
            if (currentTime > 0)
            {
                StatusControl.instance.SetStopBar(currentTime / fMaxStopTime);
                currentTime -= Time.unscaledDeltaTime;
            }
            else
            {
                currentTime = 0;
                ResumeTime();
            }
        }
        
    }


    public void StopTime()
    {
        if (!recall.BIsRecalling && !bStopTime)
        {
            Time.timeScale = fSlowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            bStopTime = true;
            PPController.instance.FadeToDark(0.4f * Time.timeScale);
            GetComponent<TrailRenderer>().enabled = true;
            GetComponent<TrailRenderer>().time *= Time.timeScale;

            //Debug.Log(Time.timeScale + " " + Time.fixedUnscaledDeltaTime + " " + Time.fixedDeltaTime);
            ////
            //if (Time.timeScale < 1)
            //{
            //    Time.fixedDeltaTime = 0.1f * 0.02f;
            //    transform.position += dir * fMoveSpeed / 5 * Time.fixedDeltaTime;

            //}

        }
    }
    public void ResumeTime()
    {
        if (bStopTime)
        {
            GetComponent<TrailRenderer>().time /= Time.timeScale;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            bStopTime = false;
            PPController.instance.FadeToNormal(0.2f);
            GetComponent<TrailRenderer>().enabled = false;

        }

    }
}
