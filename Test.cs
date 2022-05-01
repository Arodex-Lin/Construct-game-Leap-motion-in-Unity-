using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PlaceGround placeGround;
    List<PlaceGround.Ground> grounds;
    // Start is called before the first frame update
    //public bool fuck = false;
     
    void Start()
    {
        //grounds = FindObjectOfType<PlaceGround>().groundA;
        grounds = GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().groundA;
        var lisst = placeGround.groundA;
        //lisst[0].ffi();
        //lisst.Add(new PlaceGround.Ground(null, PlaceGround.State.Arable, 10));
        grounds[0].ffi();
        //grounds[0].fuck = 10;
        grounds[0] = new PlaceGround.Ground(grounds[0].ground, grounds[0].State, 10);
    }

    // Update is called once per frame
    void Update()
    {
        //var groundList = FindObjectOfType<PlaceGround>().groundA;
        //if (fuck)
        //{
        //    fuck = true;
        //    //groundList[0].State = PlaceGround.State.Arable;
        //}
    }
}
