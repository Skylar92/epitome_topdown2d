using UnityEngine;

public class Stateful : MonoBehaviour
{
    protected virtual void Start()
    {
        DontDestroyOnLoad(transform.root.gameObject);
    }

}
