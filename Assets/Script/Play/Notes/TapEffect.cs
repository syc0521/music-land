using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    void Update()
    {
        StartCoroutine(DestroyEffect());
    }
    private IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(0.267f);
        Destroy(gameObject);
        yield break;
    }

}
