using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : Collidable
{
    public string nextLevel;

    protected override void OnCollide(Collider2D hit)
    {
        if (!hit.CompareTag("Player")) return;

        GameManager.Instance.SaveState();
        SceneManager.LoadScene(nextLevel);
    }
}