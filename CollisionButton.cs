using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButton : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)//���Ŀǰ��ײ�İ�ť
    {
        switch(gameObject.name)//�Ѱ�ť���������óɲ�ͬ���ܵ�����
        {
            case "PlaceGround"://��ǰ��ײ�İ�ť������PlaceGround
                print("PlaceGround");//�����⵱ǰ״̬�����
                ButtonControl.SetPlaceGround();//���ð�ť���õĺ�����ButtonControl�ű��
                break;
            case "SelectGround"://����ͬ��
                print("SelectGround");
                ButtonControl.SetSelectGround();
                break;
            case "SetArable":
                print("SetArable");
                ButtonControl.SetArable();
                break;
            case "SetPlant":
                ButtonControl.SetPlant();
                print("SetPlant");
                break;
        }
    }
}
