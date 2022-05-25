using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using HandGuesterNameSpace;
using UnityEngine.UI;

public class TurnHandMenu : MonoBehaviour
{
    LeapProvider LeapProvider;
    public GameObject ButtonPlaceGround , ButtonSelectGround, ButtonSetArable, ButtonSetPlant;//四个按钮的游戏对象，实际只用了三个
    Vector3 offset1 = new Vector3(0,0,0.3f);
    Vector3 offset2 = new Vector3(0.3f,0,0.3f);
    Vector3 offset3 = new Vector3(0.6f,0,0.3f);//四个按钮的偏移，即于手掌心的相对位置
    Vector3 offset4 = new Vector3(0.9f,0,0.3f);
    bool menuopen = false;//当前菜单是否处于打开状态
    void Start()
    {
         LeapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = LeapProvider.CurrentFrame;//获取当前帧
        menuopen = GetMenuOpen(frame);//获取当前菜单是否应该被打开
        OpenMenu(menuopen,frame);//打开菜单的函数
    }

    void OpenMenu(bool menuopen, Frame frame)//打开菜单
    {
        Vector3 palmPos = new Vector3 (0,0,0);//给一个初始值
        if (menuopen)//如果菜单应该被打开
        {
            foreach(var hand in frame.Hands)
            {
                
                if (hand.IsLeft)
                {
                    palmPos = UnityVectorExtension.ToVector3(hand.PalmPosition);//获取左手掌心向量
                }
            }
            ButtonPlaceGround.transform.position = palmPos + offset1;
            //ButtonSelectGround.transform.position = palmPos + offset2;
            ButtonSetArable.transform.position = palmPos + offset2;
            ButtonSetPlant.transform.position = palmPos + offset3;//将三个按钮的位置设为左手掌心加偏移值
            //ButtonPlaceGround.transform.Rotate(-80, 0, 180);
        }
        else
        {
            ButtonPlaceGround.transform.position = new Vector3(-0.1f, -2.5f, -0.5f);
            ButtonSelectGround.transform.position = new Vector3(0.1f, -2.5f, -0.5f);
            ButtonSetArable.transform.position = new Vector3(0.3f, -2.5f, -0.5f);
            ButtonSetPlant.transform.position = new Vector3(0.5f, -2.5f, -0.5f);//将按钮藏到下面
        }
    }
    

    bool GetMenuOpen(Frame frame)//如果当前帧的左手掌心朝上，返回真，即菜单应该被打开，否则返回假
    {
        foreach(var hand in frame.Hands)
        {
            if(hand.IsLeft)
            {
                if (HandGestures.isPalmNormalSameDirectionWith(hand, new Vector3(0, 1, 0)))//调用HandGestures里的函数判断掌心向量是否和一个向量方向相同
                {
                    return true;
                }
            }
        }
        return false;
    }
}
