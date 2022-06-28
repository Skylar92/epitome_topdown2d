using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBomb : Bomb
{
    public Vector3 targetScale = new Vector3(2, 2, 0);

    protected override IEnumerator DestroyWithDelay()
    {
        while (true)
        {
            var transformLocalScale = transform.localScale;
            var newScale = new Vector3(transformLocalScale.x + 0.01f, transformLocalScale.y + 0.01f, 0);
            if (targetScale.x >= newScale.x) {
                transform.localScale = newScale;
                yield return new WaitForSeconds(timeAliveAfterArrive);
            }
            else 
                break;
        }

        yield return new WaitForSeconds(timeAliveAfterArrive);
        Destroy(gameObject);
    }
}