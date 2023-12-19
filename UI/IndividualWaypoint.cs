using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndividualWaypoint : MonoBehaviour
{
    public Image waypointIcon;
    public TextMeshProUGUI playerText;
    public GameObject otherPlayer;
    public Camera targetCam;
    private Canvas thisCanvas;
    private RectTransform thisCanvasRect;

    [SerializeField] private float minDistance;
    [SerializeField] private float fullDistance;
    public Vector3 adjustment;
    private float alphaValue;
    private float defaultMinDist;
    [SerializeField] private LayerMask lm;

    // Start is called before the first frame update
    void Start()
    {
        thisCanvas = this.GetComponent<Canvas>();
        thisCanvasRect = this.GetComponent<RectTransform>();
        defaultMinDist = minDistance;
    }

    // Update is called once per frame
    void Update()
    {

        if(otherPlayer != null)
        {

            float distance = Vector3.Distance(otherPlayer.transform.position, transform.position);
            //fade over distance 
            if(distance < minDistance + (minDistance/2))
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, (otherPlayer.transform.position - transform.position), out hit, minDistance, lm))
                {
                    if(hit.transform.tag != "Player")
                    {
                        alphaValue = distance / fullDistance;
                    }else
                    {
                        alphaValue = (distance - minDistance) / (fullDistance - minDistance);
                    }
                    
                }else
                {
                    alphaValue = (distance - minDistance) / (fullDistance - minDistance);
                }
                
                

            }
            else if(distance < minDistance)
            {
                alphaValue = (distance - minDistance) / (fullDistance - minDistance);
            }
            else if(distance > fullDistance)
            {
                alphaValue = 1f;
            }
            else
            {
                
                alphaValue = (distance - minDistance) / (fullDistance - minDistance);

            }

            Vector2 pos = WorldSpaceToCanvas(thisCanvasRect, targetCam, otherPlayer.transform.position + adjustment);

            //print(Vector3.Dot((otherPlayer.transform.position - this.transform.position), transform.TransformDirection(Vector3.forward)));
            

            TextMeshProUGUI text = waypointIcon.GetComponentInChildren<TextMeshProUGUI>();
            if(Vector3.Dot((otherPlayer.transform.position - this.transform.position), transform.TransformDirection(Vector3.forward)) < 0)
            {
                waypointIcon.color = new Color(waypointIcon.color.r, waypointIcon.color.g, waypointIcon.color.b, 0f);
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
            }
            else if(text.text != "REVIVE")
            {
                waypointIcon.color = new Color(waypointIcon.color.r, waypointIcon.color.g, waypointIcon.color.b, alphaValue);
                text.color = new Color(text.color.r, text.color.g, text.color.b, alphaValue);
            }else
            {
                waypointIcon.color = new Color(waypointIcon.color.r, waypointIcon.color.g, waypointIcon.color.b, 1f);
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            }
            
            waypointIcon.GetComponent<RectTransform>().anchoredPosition = pos;
            //adjustedPosition.x *= thisCanvasRect.rect.width / (float)targetCam.pixelWidth;
            //adjustedPosition.y *= thisCanvasRect.rect.height / (float)targetCam.pixelHeight;
            
                    // set it
            //waypointIcon.transform.position = adjustedPosition - thisCanvasRect.sizeDelta / 2f;
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
