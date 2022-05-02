using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{

    public static void SetPlaceGround()
    {
        PlaceGround.SetAviailableTrue();
        SelectSquare.SetAviailableFalse();
        PloughGround.SetArabFalse();
        PloughGround.SetPlantFalse();
    }
}
