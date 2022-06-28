using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : Collidable
{
    public string nextLevel;

    protected override void Start()
    {
        base.Start();
        ColliderListener.OnPlayerCollideEvent += OnPlayerEnterPortal;
    }

    private void OnPlayerEnterPortal(Collider2D hit)
    {
        GameManager.Instance.SaveState();
        SceneManager.LoadScene(nextLevel);
    }
}