using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveWaypoint : MonoBehaviour
{
    public Transform target;
    public Image waypoint;

    public Camera targetCam;
    private RectTransform thisCanvasRect;
    
    public bool warn;

    // Update is called once per frame
    public void FeedNewPoint(Transform objectivePoint)
    {
        target = objectivePoint;
    }
    void Start()
    {
        thisCanvasRect = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        if(target != null)
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
        if(Vector3.Dot((target.position - this.transform.position), transform.TransformDirection(Vector3.forward)) < 0 || warn)
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
}
