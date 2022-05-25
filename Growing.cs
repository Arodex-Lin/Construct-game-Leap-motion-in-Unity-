using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�˽ű�������������ӵĳ��죬�õķ����ܲ��ף�������ʱ���ù�Ҳ���ÿ�
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
        ControlGrow(grounds); //�������
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
                StartCoroutine(isGrowing(nowindex));//��ÿһ����û�п�ʼ�������ֵ��濪ʼ���г����ʱ
            }
            nowindex++;
        }
 
    }
    IEnumerator isGrowing(int index)
    {
        yield return new WaitForSeconds(5);
        print(index + "Has chengshou le !!!");

    }
    List<PlaceGround.Ground> GetNowGroundList()//���صذ�ṹ���б�
    {
        return GameObject.Find("ScriptHanger").GetComponent<PlaceGround>().groundA;
    }
}
