using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private bool _isNewSystem = false;

    private void Start()
    {
        if (_ps == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_ps.isStopped)
        {
            Destroy(gameObject);
        }
    }

    public void SetRadius(float radius)
    {
        if (_isNewSystem)
        {
            transform.localScale *= radius;
        }
        else
        {
            ParticleSystem.MainModule main = _ps.main;
            main.startSpeed = radius / main.duration;
        }
        
    }
}
