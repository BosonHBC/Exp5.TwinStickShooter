using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusControl : MonoBehaviour
{
    public static StatusControl instance;
    //public static float timeScale = 1;
    private void Awake()
    {
        if (instance == null || instance != this)
            instance = this;
    }
    [SerializeField]
    private Image hpFill;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Image stopBarFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHp(float currentHp, float maxHp)
    {
        hpFill.fillAmount = currentHp / maxHp;
        hpText.text = currentHp + " / " + maxHp;
    }

    public void SetStopBar(float ratio)
    {
        stopBarFill.fillAmount = ratio;
    }
}
