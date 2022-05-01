using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCKG : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject toBeGet;
    void Start()
    {
        print(1);
    }

    // Update is called once per frame
    void Update()
    {
        for(float i =0.001f;i<=1;i+=0.001f)
        {
            
            gameObject.transform.localScale = new Vector3(i,gameObject.transform.localScale.y,i);
            if(Mathf.Abs(gameObject.GetComponent<BoxCollider>().size.x * gameObject.transform.localScale.x-0.5f)<0.01f)
            {
                print(gameObject.GetComponent<BoxCollider>().size.x * gameObject.transform.localScale.x);
                print(i);
            }
        }
        
    }
}
