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
    public GameObject ButtonPlaceGround , ButtonSelectGround, ButtonSetArable, ButtonSetPlant;//�ĸ���ť����Ϸ����ʵ��ֻ��������
    Vector3 offset1 = new Vector3(0,0,0.3f);
    Vector3 offset2 = new Vector3(0.3f,0,0.3f);
    Vector3 offset3 = new Vector3(0.6f,0,0.3f);//�ĸ���ť��ƫ�ƣ����������ĵ����λ��
    Vector3 offset4 = new Vector3(0.9f,0,0.3f);
    bool menuopen = false;//��ǰ�˵��Ƿ��ڴ�״̬
    void Start()
    {
         LeapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = LeapProvider.CurrentFrame;//��ȡ��ǰ֡
        menuopen = GetMenuOpen(frame);//��ȡ��ǰ�˵��Ƿ�Ӧ�ñ���
        OpenMenu(menuopen,frame);//�򿪲˵��ĺ���
    }

    void OpenMenu(bool menuopen, Frame frame)//�򿪲˵�
    {
        Vector3 palmPos = new Vector3 (0,0,0);//��һ����ʼֵ
        if (menuopen)//����˵�Ӧ�ñ���
        {
            foreach(var hand in frame.Hands)
            {
                
                if (hand.IsLeft)
                {
                    palmPos = UnityVectorExtension.ToVector3(hand.PalmPosition);//��ȡ������������
                }
            }
            ButtonPlaceGround.transform.position = palmPos + offset1;
            //ButtonSelectGround.transform.position = palmPos + offset2;
            ButtonSetArable.transform.position = palmPos + offset2;
            ButtonSetPlant.transform.position = palmPos + offset3;//��������ť��λ����Ϊ�������ļ�ƫ��ֵ
            //ButtonPlaceGround.transform.Rotate(-80, 0, 180);
        }
        else
        {
            ButtonPlaceGround.transform.position = new Vector3(-0.1f, -2.5f, -0.5f);
            ButtonSelectGround.transform.position = new Vector3(0.1f, -2.5f, -0.5f);
            ButtonSetArable.transform.position = new Vector3(0.3f, -2.5f, -0.5f);
            ButtonSetPlant.transform.position = new Vector3(0.5f, -2.5f, -0.5f);//����ť�ص�����
        }
    }
    

    bool GetMenuOpen(Frame frame)//�����ǰ֡���������ĳ��ϣ������棬���˵�Ӧ�ñ��򿪣����򷵻ؼ�
    {
        foreach(var hand in frame.Hands)
        {
            if(hand.IsLeft)
            {
                if (HandGestures.isPalmNormalSameDirectionWith(hand, new Vector3(0, 1, 0)))//����HandGestures��ĺ����ж����������Ƿ��һ������������ͬ
                {
                    return true;
                }
            }
        }
        return false;
    }
}
