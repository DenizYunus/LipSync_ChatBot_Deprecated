
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovementWillBeDeleted : MonoBehaviour
{
    public GameObject eyeBallLeft;
    public GameObject eyeBallRight;

    void Start()
    {
        StartCoroutine(EyeControl());
    }

    IEnumerator EyeControl()
    {
        eyeBallLeft.transform.localPosition = new Vector3(-0.07170646f, 0.07606196f, 0.03144899f);
        yield return new WaitForSeconds(2);
        eyeBallLeft.transform.localPosition = new Vector3(-0.07170646f, -0.07606196f, 0.03144899f);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(EyeControl());

    }
}
