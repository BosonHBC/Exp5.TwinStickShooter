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
    // Start is called before the first frame update
    void Start()
    {
        
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
        bStopTime = true;
        PPController.instance.FadeToDark(0.4f);
    }

    public void SlowdownTime(float _scale)
    {

    }

    public void ResumeTime()
    {
        bStopTime = false;
        PPController.instance.FadeToNormal(0.2f);

    }
}
