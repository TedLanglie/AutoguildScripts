using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] float _Speed;
    // Start is called before the first frame update

    private IEnumerator lerpToNewValue(float targetY)
    {
        float time = 0;
        float lerpValue = transform.position.y;
        while (Mathf.Abs(transform.position.y - targetY) > .001)
        {
            lerpValue = Mathf.Lerp(transform.position.y, targetY, time * _Speed);
            transform.position = new Vector3(0, lerpValue, -10);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0, targetY, -10);
    }

    private IEnumerator lerpToNewXValue(float targetX)
    {
        float time = 0;
        float lerpValue = transform.position.x;
        while (Mathf.Abs(transform.position.x - targetX) > .001)
        {
            lerpValue = Mathf.Lerp(transform.position.x, targetX, time * _Speed);
            transform.position = new Vector3(lerpValue, 0, -10);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(targetX, 0, -10);
    }

    public void toggleLerpCamYAxis(float yCoord)
    {
        StartCoroutine(lerpToNewValue(yCoord));
    }

    public void toggleLerpCamXAxis(float xCoord)
    {
        StartCoroutine(lerpToNewXValue(xCoord));
    }
}
