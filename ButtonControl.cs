using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{//����������ť�ֱ𴥷������������������Ϊ�Ǿ�̬�������ڴ�����ť�Ľű�(CollisionButton)��ֱ�ӵ�����

    public static void SetPlaceGround()
    {
        PlaceGround.toDisappear = false;
        PlaceGround.SetAviailableTrue();
        SelectSquare.SetAviailableFalse();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }//���õ�ǰ״̬Ϊ�����õ��桱�������õ��淽����Ϊ�棬������Ϊ��
    public static void SetSelectGround()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }//��ʱû�õ�
    public static void SetArable()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabTrue();
        PloughGround.SetPlantFalse();
    }//ͬ�������õ��淽����Ϊ�٣�������Ϊ�棬������Ϊ��
    public static void SetPlant()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantTrue();
    }//ͬ��
}
