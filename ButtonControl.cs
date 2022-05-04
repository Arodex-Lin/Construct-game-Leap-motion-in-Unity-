using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{

    public static void SetPlaceGround()
    {
        print(1);
        PlaceGround.SetAviailableTrue();
        SelectSquare.SetAviailableFalse();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }
    public static void SetSelectGround()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }
    public static void SetArable()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabTrue();
        PloughGround.SetPlantFalse();
    }
    public static void SetPlant()
    {
        PlaceGround.SetAviailableFalse();
        SelectSquare.SetAviailableTrue();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantTrue();
    }
}
