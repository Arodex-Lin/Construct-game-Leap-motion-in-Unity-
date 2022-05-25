using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using HandGuesterNameSpace;

public class PloughGround : MonoBehaviour
{
    LeapProvider LeapProvider;
    //PlaceGround.Ground NowSquare;
    int nowindex;
    List<PlaceGround.Ground> grounds;
    //public PlaceGround.Ground Ground;
    bool Once = true;
    static public bool arab = false;
    static public bool plant = false;
    // Start is called before the first frame update
    void Start()
    {
        LeapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame
    void Update()
    {
        if (arab)
        {
            IfGroundIs(PlaceGround.State.Nothing,PlaceGround.State.Arable);//地面如果为Nothing，设置为Arable
        }
        if (plant)
        {
            IfGroundIs(PlaceGround.State.Arable,PlaceGround.State.Planted);//同理
        }
    }

    void IfGroundIs(PlaceGround.State ifstate,PlaceGround.State wantstate)//执行犁地或撒种等对地面的操作，从ifstate设置到wantstate
    {
        Frame frame = LeapProvider.CurrentFrame;
        grounds = GetNowGroundList();//获取地面结构体列表
        nowindex = GameObject.Find("ScriptHanger").GetComponent<SelectSquare>().index;
        if (nowindex >= 0)
        {
            PlaceGround.Ground NowSquare = grounds[nowindex];
            if (NowSquare.ground != null)
            {
                if (pdRight(frame))
                {
                    if (NowSquare.getState() == ifstate)
                    {
                        grounds[nowindex] = new PlaceGround.Ground(NowSquare.ground, wantstate);
                    }
                }
            }
        }
        
    }
    bool pdRight(Frame frame)//检测手是否向右的函数
    {
        foreach (var hand in frame.Hands)
        {
            if (HandGestures.isMoveRight(hand) && Once)
            {
                Once = false;
                StartCoroutine(moveOnce());
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    List<PlaceGround.Ground> GetNowGroundList()//返回地板结构体列表
    {
        return GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().groundA;
    }
    static public void SetArabTrue()//以下四个函数都是通过按钮改变对应的布尔值
    {
        arab = true;
    }
    static public void SetArabFalse()
    {
        arab = false;
    }
    static public void SetPlantTrue()
    {
        plant = true;
    }
    static public void SetPlantFalse()
    {
        plant = false;
    }
    public IEnumerator moveOnce()
    {
        yield return new WaitForSeconds(1f);
        Once = true;
    }//延时1秒
}
