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
    private Vignette vignette;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        bool isValid1 = volume.profile.TryGetSettings<ColorGrading>(out ccSettings);
        bool isValid2 = volume.profile.TryGetSettings<Vignette>(out vignette);
        if (!isValid1 && isValid2)
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

    public void GetDamage(float _lerp)
    {

        GetComponent<Animator>().Play("GetDamage");
        vignette.intensity.value = new FloatParameter(){value = Mathf.Lerp(0.3f, 0.45f, _lerp)};
        Color _color = new Color(Mathf.Lerp(0, 0.7f, _lerp), 0, 0);
        vignette.color.value = _color;
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
