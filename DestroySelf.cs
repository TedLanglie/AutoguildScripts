using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float _TimeTillDestroy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyOwnObject());
    }

    private IEnumerator DestroyOwnObject()
    {
        yield return new WaitForSeconds(_TimeTillDestroy);
        Destroy(gameObject);
    }
}
