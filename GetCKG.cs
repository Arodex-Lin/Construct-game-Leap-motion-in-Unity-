using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCKG : MonoBehaviour
{
    public static float GetFloatToScale(GameObject Object)
    {
        for (float i = 0.001f; i <= 1; i += 0.001f)
        {

            Object.transform.localScale = new Vector3(i, Object.transform.localScale.y, i);
            if (Mathf.Abs(Object.GetComponent<BoxCollider>().size.x * Object.transform.localScale.x - 0.5f) < 0.01f)
            {
                return i;
            }
        }
        return 0;

    } 
}
