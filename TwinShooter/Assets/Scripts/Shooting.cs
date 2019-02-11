using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    
    [Header("Shooting")]
    // Shooting
    [SerializeField] private float fInterval;
    [SerializeField] private float fDamage;
    [SerializeField] private float fShootSpd;
    private Transform spawnPoint;
    private Transform bulletParent;
    private float collpaseTime;
    private bool bCDing;

    [Header("Reload")]
    // Reload
    [SerializeField] private float fReloadTime;
    [SerializeField] private int iMaxBullet;

    private float fReloadCollapseTime;
    private bool bRelaoding;
    private int icurrBullet;
    [Header("UIRef")]
    [SerializeField] Image reloadBar;
    [SerializeField] Text bulletText;

    // Start is called before the first frame update
    void Start()
    {
        bCDing = false;
        bulletParent = GameManager.instance.bulletParent;
        spawnPoint = transform.GetChild(1);
        icurrBullet = iMaxBullet;
    }

    // Update is called once per frame
    void Update()
    {
        CoolDown();
        if (Input.GetMouseButton(0) && !bCDing && !bRelaoding)
        {
            Fire();
        }
        Reload();
        if (Input.GetKeyDown(KeyCode.R) && !bRelaoding)
        {
            bRelaoding = true;
            reloadBar.transform.parent.gameObject.SetActive(true);

        }
    }

    void CoolDown()
    {
        if (bCDing)
        {
            collpaseTime += Time.unscaledDeltaTime;
            if (collpaseTime >= fInterval)
            {
                bCDing = false;
                collpaseTime = 0;
            }
        }

    }

    void Reload()
    {
        if (bRelaoding)
        {
            fReloadCollapseTime += Time.unscaledDeltaTime;
            reloadBar.fillAmount = fReloadCollapseTime / fReloadTime;
            if (fReloadCollapseTime >= fReloadTime)
            {
                bRelaoding = false;
                icurrBullet = iMaxBullet;
                bulletText.text = icurrBullet + " / " + iMaxBullet;
                fReloadCollapseTime = 0;
                reloadBar.transform.parent.gameObject.SetActive(false);

            }
        }
    }

    void Fire()
    {
        bCDing = true;
       GameObject go = Instantiate(bulletPrefab);
        go.transform.SetParent(bulletParent);
        go.transform.position = spawnPoint.position;
        go.transform.rotation = spawnPoint.rotation;
        icurrBullet--;
        bulletText.text = icurrBullet + " / " + iMaxBullet;
        if (icurrBullet <= 0)
        {
            bRelaoding = true;
            reloadBar.transform.parent.gameObject.SetActive(true);
        }
           

        go.GetComponent<BulletMover>().SetBullet(fDamage, fShootSpd);
    }
}
