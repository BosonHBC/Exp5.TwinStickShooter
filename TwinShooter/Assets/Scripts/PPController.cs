using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPController : MonoBehaviour
{
    public static PPController instance;
    private void Awake()
    {
        if (instance == null || instance != this)
            instance = this;
    }
    private PostProcessVolume volume;
    private ColorGrading ccSettings;
    
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        bool isValid = volume.profile.TryGetSettings<ColorGrading>(out ccSettings);

        if (!isValid)
            return;

    }

    // Update is called once per frame
    void Update()
    {

    }

   public void FadeToDark(float _t)
    {
        StartCoroutine(FadeParameter(new float[] { 50f, 0f }, new float[] { -100f, 100f }, _t));
    }
    public void FadeToNormal(float _t)
    {
        StartCoroutine(FadeParameter(new float[] { -100f, 100f }, new float[] { 50f, 0f }, _t));

    }
    IEnumerator FadeParameter(  float[] _start, float[] _end, float _fadeTime = 0.5f)
    {
        float _timeStartFade = Time.time;
        float _timeSinceStart = Time.time - _timeStartFade;
        float _lerpPercentage = _timeSinceStart / _fadeTime;

        FloatParameter[] _fp = new FloatParameter[2];

        while (true)
        {
            _timeSinceStart = Time.time - _timeStartFade;
            _lerpPercentage = _timeSinceStart / _fadeTime;

            for (int i = 0; i < _fp.Length; i++)
            {
                float currentValue = Mathf.Lerp(_start[i], _end[i], _lerpPercentage);
                _fp[i] = new FloatParameter() { value = currentValue};
            }
            ccSettings.saturation.value = _fp[0];
            ccSettings.contrast.value = _fp[1];


            if (_lerpPercentage >= 1) break;


            yield return new WaitForEndOfFrame();
        }

    }
}
