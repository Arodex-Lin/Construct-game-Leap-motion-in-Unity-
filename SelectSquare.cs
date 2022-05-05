using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using HandGuesterNameSpace;

public class SelectSquare : MonoBehaviour
{
    LeapProvider LeapProvider;
    LineRenderer line;
    RaycastHit hit;
    static public bool available = false;
    public List<PlaceGround.Ground> groundList;
    public PlaceGround.Ground nowSquare;
    public Material chosenMaterial;
    public Material originMaterial;
    public int index;
    const float startWidth = 0.01f;
    const float endWidth = 0.01f;
    //HandGuesterNameSpace HandGuesterNameSpace = new HandGuesterNameSpace();

    void Start()
    {
        LeapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
        line = gameObject.GetComponent<LineRenderer>();
    }


    void Update()
    {
        if (available)
        {
            groundList = GetNowGroundList();//获取当前地板结构体列表
            Frame frame = LeapProvider.CurrentFrame;
            nowSquare = OnChosen(EmitRay(frame));//获取当前选择的地板，设为被选择的材质，发射射线
            index = FindIndex(nowSquare, groundList);
        }
        else
        {
            line.SetWidth(0,0);
        }
    }

    private int FindIndex(PlaceGround.Ground nowSquare, List<PlaceGround.Ground> groundList)
    {
        foreach(var plane in groundList)
        {
            if (nowSquare.ground != null)
            {
                if (nowSquare.ground.transform.position == plane.ground.transform.position)
                {
                    return groundList.IndexOf(plane);
                }
            }
        }
        return -1;
    }

    public Vector3 EmitRay(Frame frame)//发射射线
    {
        Vector3 startPos, endPos = new Vector3(0, 0, 0);
        line.SetWidth(startWidth, endWidth);//设置起始，结束宽度
        foreach (var hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                bool rayCast = Physics.Raycast(ray, out hit, 5000);
                startPos = new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z);
                endPos = GetFinalPos(hit, hand);
                //endPos = new Vector3(0, 0, 0);
                line.SetPosition(0, startPos);
                line.SetPosition(1, endPos);//画线
            }
        }
        return endPos;
    }

    PlaceGround.Ground OnChosen(Vector3 endPos)//将被选择的地板材质更换
    {
        PlaceGround.Ground ret = new PlaceGround.Ground();
        foreach (var ground in groundList)
        {
            if (endPos == ground.ground.transform.position)
            {
                ground.ground.gameObject.GetComponent<MeshRenderer>().material = chosenMaterial;
                ret = ground;
            }
            else
            {
                ground.ground.gameObject.GetComponent<MeshRenderer>().material = originMaterial;
            }
        }
        return ret;
    }

    Vector3 GetFinalPos(RaycastHit hit, Hand hand)//返回射线终点坐标，吸附
    {
        Vector3 endPos = hand.Direction.ToVector3().normalized * 100000;
        float minndis = 1e6f;
        Vector3 finalPos = new Vector3(0, 0, 0);
        if (hit.point == null)
        {
            return endPos;
        }
        foreach (var nowObi in groundList)
        {
            if (HandGestures.getDistance(nowObi.ground.transform.position, hit.point) < minndis)
            {
                minndis = HandGestures.getDistance(nowObi.ground.transform.position, hit.point);
                finalPos = nowObi.ground.transform.position;
            }
        }
        if (minndis < 0.25f)
        {
            return finalPos;
        }
        return hand.Direction.ToVector3().normalized * 100000;
    }


    static public void SetAviailableTrue()
    {
        available = true;
    }
    static public void SetAviailableFalse()
    {
        available = false;
    }
    List<PlaceGround.Ground> GetNowGroundList()//返回地板结构体列表
    {
        //return PlaceGround.groundA;
        //return FindObjectOfType<PlaceGround>().groundA;
        return GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().groundA;
    }
}
