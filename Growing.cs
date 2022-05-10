using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growing : MonoBehaviour
{
    int listcount;
    List<PlaceGround.Ground> grounds;
    List<PlaceGround.Ground> plantedGrounds = new List<PlaceGround.Ground>();
    List<bool> clocked = new List<bool>();
    void Update()
    {
        grounds = GetNowGroundList();
//        plantedGrounds = GetPlantedGrounds(grounds);
        ControlGrow(grounds); //管理成熟
    }


    void ControlGrow(List<PlaceGround.Ground> grounds)
    {
        int nowindex = 1;
        listcount = GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().listcount;
        foreach(var ground in grounds)
        {

            if (clocked.Count < nowindex)
            {
                clocked.Add(false);
            }
            if (ground.State == PlaceGround.State.Planted && clocked[nowindex -1] == false)
            {
                clocked[nowindex - 1] = true;
                StartCoroutine(isGrowing(nowindex));//对每一个还没有开始的已撒种地面开始进行成熟计时
            }
            nowindex++;
        }
 
    }
    IEnumerator isGrowing(int index)
    {
        yield return new WaitForSeconds(5);
        print(index + "Has chengshou le !!!");

    }
    List<PlaceGround.Ground> GetNowGroundList()//返回地板结构体列表
    {
        return GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().groundA;
    }
}
