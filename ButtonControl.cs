using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{//触碰三个按钮分别触发下面的三个函数，因为是静态，所以在触发按钮的脚本(CollisionButton)里直接调用了

    public static void SetPlaceGround()
    {
        PlaceGround.toDisappear = false;
        PlaceGround.SetAviailableTrue();
        SelectSquare.SetAviailableFalse();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }//设置当前状态为“放置地面”，将放置地面方法设为真，其他设为假
    public static void SetSelectGround()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }//暂时没用到
    public static void SetArable()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabTrue();
        PloughGround.SetPlantFalse();
    }//同理，将放置地面方法设为假，耕种设为真，撒种设为假
    public static void SetPlant()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantTrue();
    }//同理
}
