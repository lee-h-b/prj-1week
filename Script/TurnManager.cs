using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//턴을 관리할 매니저
/*구상
 * 1. 현재 멤버들중 가장 빠른애를 찾음, 멤버내에서 speed의 비교를 할것 만약 둘다 빠르면 앞번호로
 * 2. 적의 가장 빠른애, 나의 가장빠른애의 커맨드를 실행 < 단 지금 명령을 넣은게 p1뿐ㅇ니 p1의 커맨드만 함
 * //ㄴ2의 커맨드는 아마 queue로 2개를 만들어서 할거임 
 * 애니메이션의 길이 +1f~2f정도멈춰서 턴이 바로바로 안돌도록 할듯
 */

public class TurnManager : MonoBehaviour {
    static public TurnManager inst;
    [SerializeField]
    float animCycle;//애니메이션사이클 다음 턴으로 가는데 중요(?)한 역활

    public Transform orderScreen;//명령받는 스크린 싸이클 다돌면 행함
    public bool playing = false;//한번 만들어봄 쓰지않을까?
    //현재 멤버수
    Queue<CharaScript> p1Priority = new Queue<CharaScript>();
    [SerializeField]
    Queue<CharaScript> p2Priority = new Queue<CharaScript>();
    //queue내의 chara순서대로 실행함 int로 ㅎ려했는데
    //members내에서 몇번째 값인지를 넣는 방식이 좀 어려운듯

    IEnumerator Cycle()
    {
        CharaScript member1 = new CharaScript();
        CharaScript member2 = new CharaScript();
        while (p1Priority.Count > 0 || p2Priority.Count > 0 )
        {
            //queue내의 멤버를 빼옴
            
            if(p1Priority.Count != 0)
             member1 = p1Priority.Dequeue();
            if(p2Priority.Count != 0)
             member2 = p2Priority.Dequeue();
            //3인이유는 명령이 증감하지않을거고 테이블에서 3개를 무조건 받았기때문
            for (int j = 0; j < 3; j++)
            {
                Debug.Log("도는애 : " + member1);
                Debug.Log(member1.OrderEmpty());
                 
                //명령실행 << 이것과 똑같은게 member2로도 들어갈것
                if (member1 != null && member1.Dead == false )
                {
                    if(member1.OrderEmpty() == false )
                    member1.ActionOrder();

                    if (member1.Dead == true) member1 = null;
                }
                if(member2 != null && member2.Dead == false)//체력이 0이 아닌경우에만
                {
                    //죽은애를 불러보니 문제가생김
                    if(member2.OrderEmpty() == false )
                    member2.ActionOrder();
                    if (member2.Dead == true) member2 = null;

                }
                int check = PartyManager.inst.CheckMemberAnnihilation();
                if (check != 0)
                {
                    Debug.Log("체커"+ check);
                    playing = false;//플레잉을 거짓으로
                    GameObject.Find("UI").GetComponent<UIDraw>().Win(check);
                    //게임 끝났다고 포인트 레벨 정리해줌
                    if (check == 1)
                    {
                        GameManager.inst.PlayerInfo.AddPoint(2);
                        GameManager.inst.PlayerInfo.AddLevel();
                    }
                    else GameManager.inst.PlayerInfo.AddPoint();//져도 1포인트줌
                    yield break;//턴끝났고 생존자 나옴
                }

                yield return new WaitForSeconds(2f);//무조건 2초쉼
            }

        }
        //저거다 벗어나면
        //자기자신을 멈춰도 유효하지않을까
        Debug.Log("사이클을 나갔다");
        playing = false;//플레잉을 거짓으로
        orderScreen.gameObject.SetActive(true);
        //끝나면 mp좀 회복
        for(int i = 0; i < Mathf.Max(PartyManager.inst.Member2.Count, PartyManager.inst.Member1.Count); i++)
        {
            if (PartyManager.inst.Member1.Count > i && PartyManager.inst.Member1[i].Dead == false)
                PartyManager.inst.Member1[i].PayMp (-20);//메소드 추가하기 귀찮음
            if (PartyManager.inst.Member2.Count > i && PartyManager.inst.Member2[i].Dead == false)
                PartyManager.inst.Member2[i].PayMp(-20);//메소드 추가하기 귀찮음
        }
        yield break;
        //버그 : 다음턴에 가장빠른애가 1player가 되버림 <<플레이어자체가바뀌는듯 소트문제임
    }
    //빠른정렬이 아니라 queue에 넣기 쉽게 정리하는거
    int SpeedSort(CharaScript m1, CharaScript m2)
    {
        if (m1.Status.speed > m2.Status.speed)
            return -1;
        else if (m1.Status.speed < m2.Status.speed)
            return 1;
        else return 0;
    }
    void PushQueues()
    {
        //p2는 아직 이동,공격이먼저
        //가장 빠른애로 바뀌어서 수정함 이거의문제 깊은복사,얉은복사 문제였음 깊은복사로 바꿔서해결
        var p1 = PartyManager.inst.Copy(1);
        var p2 = PartyManager.inst.Copy(2);
        p1.Sort(SpeedSort);
        p2.Sort(SpeedSort);
        for (int i = 0; i < Mathf.Max(p1.Count, p2.Count); i++)
        {

            Debug.Log("사이즈 : " + i + " " +p1.Count);
            if (p1.Count > i && p1[i].Dead == false)
                p1Priority.Enqueue(p1[i]);
            if (p2.Count > i && p2[i].Dead == false)
                p2Priority.Enqueue(p2[i]);
            Debug.Log(p1Priority.Peek());
        }
    }
    //턴을 실행하기
    public void ActionTurn()
    {
        playing = true;
        PushQueues();//큐부터 활성화
        //2개의 PartyMember를 돌리기
        StartCoroutine(Cycle() );

    }
    void Awake()
    {
        if (inst == null) inst = this;
    }
    // Use this for initialization
    void Start () {
        if(orderScreen == null)
        orderScreen = GameObject.Find("UI").transform.Find("PickCommand");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
