using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
