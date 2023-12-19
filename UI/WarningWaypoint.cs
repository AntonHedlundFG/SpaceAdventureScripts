using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarningWaypoint : MonoBehaviour
{
    public Transform target;
    public Image waypoint;

    public Camera targetCam;
    private RectTransform thisCanvasRect;
    
    public bool warningActive;

    [SerializeField] private DayNightData DayNightSO;

    void OnEnable()
    {
        DayNightSO.OnNightTime.AddListener(OnNight);
        DayNightSO.OnDayStart.AddListener(OnDay);
    }
    void OnDisable()
    {
        DayNightSO.OnNightTime.RemoveListener(OnNight);
        DayNightSO.OnDayStart.RemoveListener(OnDay);
    }


    void Start()
    {
        targetCam = this.GetComponent<ObjectiveWaypoint>().targetCam;
        thisCanvasRect = this.GetComponent<RectTransform>();
        target = GameObject.Find("MothershipLocation").transform;
    }

    void Update()
    {
        this.GetComponent<ObjectiveWaypoint>().warn = warningActive;

        if(target != null && warningActive)
        {
            
            waypoint.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            waypoint.enabled = true;
            UpdateWaypointPos();
        }
        else
        {
            waypoint.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            waypoint.enabled = false;
        }
    }
    private void UpdateWaypointPos()
    {
        //update waypoint POS
        Vector2 pos = WorldSpaceToCanvas(thisCanvasRect, targetCam, target.position);
        waypoint.GetComponent<RectTransform>().anchoredPosition = pos;

        //disable double sided
        if(Vector3.Dot((target.position - this.transform.position), transform.TransformDirection(Vector3.forward)) < 0)
        {
            waypoint.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            waypoint.color = new Color(waypoint.color.r, waypoint.color.g, waypoint.color.b, 0f);
            //text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }
        else
        {
            waypoint.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            waypoint.color = new Color(waypoint.color.r, waypoint.color.g, waypoint.color.b, 1f);
            //text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        }
    }

    public static Vector2 WorldSpaceToCanvas(RectTransform canvasRect, Camera camera, Vector3 worldPos)
    {
         Vector2 viewportPosition= camera.WorldToViewportPoint(worldPos);
         Vector2 canvasPos = new Vector2
         (
             (
                 (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x*0.5f) 
             ),
             (
                 (viewportPosition.y* canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)
             )
         );
 
         return canvasPos;
    }

    private void OnNight()
    {
        warningActive = true;
    }
    private void OnDay()
    {
        print("gm");
        warningActive = false;
    }
}
