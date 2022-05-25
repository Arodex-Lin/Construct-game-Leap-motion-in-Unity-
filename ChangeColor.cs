using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
   static  public Material matureMaterial;
    void Update()
    {
       foreach(var ground in GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().groundA)
        {
            if(ground.State == PlaceGround.State.Mature)
            {
                print(122);
                ground.ground.gameObject.GetComponent<MeshRenderer>().material = matureMaterial;
            }
        }
    }
}
