using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using HandGuesterNameSpace;
[SerializeField]
[Serializable]
public class PlaceGround : MonoBehaviour
{
    LeapProvider provider;
    
    public GameObject[] ground; //׼�������õ����Ԥ����

    public int listcount;
    public List<Ground> groundA;
    //public static Ground[] groundA;
    public bool available = true;
    public bool test = false;
    RaycastHit hit;
    public LayerMask LayerMask; //���������
    GameObject nowToPlace;

    bool afterPlace = true; //�������������õ�ǰѡ�е��������
    bool Once= true; //�ж����Ƶļ��


    //public HandGuesterNameSpace.HandGestures k;
    void Start()
    {
        groundA = new List<Ground>();
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        afterPlace = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        listcount = groundA.Count;
        if (test)
        {
            test = false;
            foreach(var one in groundA)
            {
                print(one.getState());
            }
            print("The End");
        }
        if (available)
        {
            Frame frame = provider.CurrentFrame;
            if (afterPlace)
            {
                afterPlace = false;
                nowToPlace = Instantiate(ground[UnityEngine.Random.Range(0, ground.Length)], new Vector3(0, 0, 0), Quaternion.identity);
                nowToPlace.transform.Rotate(new Vector3(0, 90 * UnityEngine.Random.Range(0, 5), 0));//�����ת
            }//ÿ�����ѡ��ذ���ʽ
            castOnGround(frame);//����ͶӰ����
        }
    }

    

    public enum State//�ذ�״̬ö��
    {
        Nothing,//ɶҲû��
        Placed,//����ŵ��н���
        Arable,//�ɸ��ֵ�
        Planted,//����ֲ���
        Mature
    }

    //public class Ground
    //{
    //    public GameObject ground;
    //    public State State;
    //    public Ground(GameObject gameObject,State state)
    //    {
    //        ground = gameObject;
    //        State = state;
    //    }
    //}
    public struct Ground//�ذ�ṹ��
    {
        public GameObject ground;
        public State State;
        public int fuck;

        public void ffi() => fuck = 5;
        public void SetArable() => State = State.Arable;

        public Ground(GameObject ground, State state, int i)
        {
            this.ground = ground;
            State = state;
            fuck = i;
        }

        public void setNothing() => State = State.Nothing;
        public State getState() => State;
    }

    void castOnGround(Frame frame)//�ڵ���ͶӰԤ�ƺõĵ���
    {
        foreach (var hand in frame.Hands)
        {
            if(hand.IsRight&&!afterPlace)//���ֲ��Ҵ��ڷ���״̬��ʵʱ������ͶӰ����
            {
                int nowState = getNowState(nowToPlace);
                Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                //Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), new Vector3(0,0,0));
                bool rayCast = Physics.Raycast(ray, out hit, 5000, LayerMask);
                Vector3 groundPlace = hit.point;
                if (hit.point != null)
                {
                    switch (nowState)
                    {
                        case 1:
                            foreach (var nowObi in groundA)
                            {
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0.5f, 0, 0), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0.5f, 0, 0);
                                        break;
                                    }
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(-0.5f, 0, 0), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(-0.5f, 0, 0);
                                        break;
                                    }
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0, 0, 0.5f), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0, 0, 0.5f);
                                        break;
                                    }
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0, 0, -0.5f), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0, 0, -0.5f);
                                        break;
                                    }
                            }
                            break;
                        case 2:
                            groundPlace = hit.point;
                            break;
                        default:
                            break;
                    }////ʺɽ���룬�����˸������������ø���,�������ܺ��Ĵ���
                }
                nowToPlace.transform.position = groundPlace;
                if (pdRight(frame)&&nowState!=0)//������ȷ��״̬����������λ�ã��ӽ����飬���ò���ֵ������ѡ�񱻷��õ��棩
                {
                    afterPlace = true;
                    //Ground ground = new Ground();
                    Ground ground;
                    ground.ground = nowToPlace;
                    ground.fuck = 1;
                    //ground.setNothing();
                    //ground.setArable();
                    ground.State = State.Nothing;
                    groundA.Add(ground);
                }
            }
           
        }
    }

    int getNowState(GameObject gameObject)//һ�����ص�ǰ����״̬�ĺ���
    {
        float minn = 1e6f;
        foreach(var nowObj in groundA)
        {
            if (gameObject.transform.position.x > nowObj.ground.transform.position.x - 0.25f && gameObject.transform.position.x < nowObj.ground.transform.position.x + 0.25f && gameObject.transform.position.z > nowObj.ground.transform.position.z - 0.25f && gameObject.transform.position.z < nowObj.ground.transform.position.z + 0.25f)
            {
                return 0;
            }
            minn = Mathf.Min(minn, getDistance(gameObject.transform.position, nowObj.ground.transform.position));//ÿ�θ�����Сֵ
        }
        if (minn < 1f)
        {
            return 1;
        }//��ǰ�������ĵ�������������ĵ����С��1������1
        return 2;//���򷵻�2
    }
    
    float getDistance(Vector3 transform1,Vector3 transform2)//����ŷʽ����
    {
        return Mathf.Sqrt(Mathf.Pow((transform1.x - transform2.x), 2) + Mathf.Pow((transform1.z - transform2.z), 2));
    }


    List<Vector3> returnAnchorPos(GameObject gameObject)//���ص�����Χ�ĸ�ê������ĺ���
    {
        List<Vector3> toBeReturn = new List<Vector3>();
        toBeReturn.Add(gameObject.transform.position + new Vector3(0.5f, 0, 0));
        toBeReturn.Add(gameObject.transform.position + new Vector3(-0.5f, 0, 0));
        toBeReturn.Add(gameObject.transform.position + new Vector3(0, 0, 0.5f));
        toBeReturn.Add(gameObject.transform.position + new Vector3(0, 0, -0.5f));
        return toBeReturn;

    }//�������Ż�ʺɽswithѭ��

    bool pdRight(Frame frame)//������Ƿ����ҵĺ���
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

    public IEnumerator moveOnce()
    {
        yield return new WaitForSeconds(1f);
        Once = true;
    }//��ʱ1��

    public static implicit operator PlaceGround(Ground v)
    {
        throw new NotImplementedException();
    }
}
