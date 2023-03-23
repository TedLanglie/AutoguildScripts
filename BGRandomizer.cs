using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRandomizer : MonoBehaviour
{
    [SerializeField] List<Sprite> PossibleBGs = new List<Sprite>();
    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = PossibleBGs[Random.Range(0, PossibleBGs.Count)];
    }
}
