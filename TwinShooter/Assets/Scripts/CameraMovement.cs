using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private LayerMask rayCastMask;
    [SerializeField] private float fRayCastDistance = 200;

    private Vector2 screenSize;
    private Vector2 canvasSize;
    private RectTransform crossHair;
    private CharacterMovement player;
    //Camera
    private Camera cam;
    private float fDistanceToGround;

    Vector2 mousePos;
    bool bMouseInScreen;

    // UI
    private RectTransform reloadBar;
    private float barOffsetY = 100;
    void Start()
    {
        player = GameManager.instance.player.GetComponent<CharacterMovement>();
        cam = Camera.main;
        crossHair = transform.Find("Crosshair").GetComponent<RectTransform>();
        screenSize = new Vector2(Screen.width, Screen.height);
        RectTransform _rect = GetComponent<RectTransform>();

        canvasSize = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y);
        Cursor.visible = false;

        // Camera
        RaycastHit hit;
        Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward), out hit, fRayCastDistance, rayCastMask);
        fDistanceToGround = Vector3.Distance(cam.transform.position, hit.point);

        // UI
        reloadBar = transform.GetChild(1).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        MouseTrack();
        if (bMouseInScreen)
            CameraRayCast();

        PlayerUIPosition();
    }

    void MouseTrack()
    {
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        Vector2 percentage = new Vector2(mousePos.x / screenSize.x, mousePos.y / screenSize.y);
        crossHair.anchoredPosition = canvasSize * percentage;

        bMouseInScreen = true;
        if (mousePos.x > screenSize.x || mousePos.x < 0 || mousePos.y > screenSize.y || mousePos.y < 0)
            bMouseInScreen = false;
    }

    void CameraRayCast()
    {
        Ray ray = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.blue);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * fDistanceToGround, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, fRayCastDistance, rayCastMask))
        {
            Debug.DrawLine(player.transform.position, hit.point);
            player.GetComponent<CharacterMovement>().lookPoint = hit.point;
        }

        Vector3 diff = (hit.point - player.transform.position) / 3 + player.transform.position;
        diff.y = 0;
        //if(Vector3.Distance(diff, player.transform.position) > 20f)
        //{
        //    diff = (diff - player.transform.position).normalized * 20f;
        //}

        Vector3 cameraMovePoint = diff - cam.transform.forward * fDistanceToGround;

        if (Vector3.Distance(cam.transform.position, cameraMovePoint) > 0.1f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, cameraMovePoint, 0.1f);
        }
        else
        {
            cam.transform.position = cameraMovePoint;
        }

    }

    void PlayerUIPosition()
    {
        Vector2 ViewportPosition = cam.WorldToViewportPoint(player.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasSize.x) - (canvasSize.x * 0.5f)),
        ((ViewportPosition.y * canvasSize.y) - (canvasSize.y * 0.5f)));

        //now you can set the position of the ui element
        reloadBar.anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, barOffsetY);
    }

}
