using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private Resource _resourceRef; //this reference instead, so we can modify values directly. See CollectResource()
    [SerializeField] private int _resourceAmount = 10;
    [SerializeField] private Animation _anim;
    [SerializeField] private EventSO _pickUpEvent;

    private bool _isCollected = false;
    private ResourceSpawner _resourceSpawner;
    private List<PlayerActions> _playersInteracting = new List<PlayerActions>();

    public void BeginCollectResource(PlayerActions playerActions)
    {
        if (_playersInteracting.Contains(playerActions)) return;

        _playersInteracting.Add(playerActions);
        if (_playersInteracting.Count >= 2 && !_isCollected) 
        {
            CollectResource();
        } 
    }

    public void StopCollectResource(PlayerActions playerActions)
    {
        _playersInteracting.Remove(playerActions);
    }

    public virtual void CollectResource()
    {
        _isCollected = true;
        //With a Resource ref we can modify value of the resource right here. We can store resource values right in the ScriptableObject rather than lock it into a specific MonoBehaviour.
        if (_resourceRef != null)
        {
            _resourceRef.GatheredAmount += _resourceAmount;
        }

        _pickUpEvent?.Event.Invoke();

        if (_anim != null && !_anim.isPlaying) {
            _anim.Play();
            Invoke("HideObject", _anim.clip.length);
        }
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
        // _resourceSpawner?.ReturnSpawnObject(gameObject);
    }

    public void InitResourceObject(ResourceSpawner rS)
    {
        _resourceSpawner = rS;
        gameObject.SetActive(false);
    }

    public void SpawnObject()
    {
        gameObject.SetActive(true);
        _isCollected = false;
        // Animator being weird so i have to do this
        _anim.Play();
        Invoke("StopAnim", 0.01f);
    }

    public void StopAnim()
    {
        _anim.Stop();
    }
}
