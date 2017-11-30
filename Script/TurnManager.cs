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
 //버그1 이동의불량
 //버그2 로봇이 이동x
public class TurnManager : MonoBehaviour {
    static public TurnManager inst;
    [SerializeField]
    float animCycle;//애니메이션사이클 다음 턴으로 가는데 중요(?)한 역활

    //    Queue<OrderScript> p1Queue, p2Queue;//명령스크립트 이걸 order에게서 받은뒤 실행할것
    //명령을 받아서 하면 캐릭터에 있는 명령은 뭐가됨?
    Queue<CharaScript> p1Priority = new Queue<CharaScript>();
    Queue<CharaScript> p2Priority = new Queue<CharaScript>();
    //queue내의 chara순서대로 실행함 int로 ㅎ려했는데
    //members내에서 몇번째 값인지를 넣는 방식이 좀 어려운듯

    //사이클 턴돌림
    //칼이 이상한곳으로 이동함
    //사이클도 1번만돔
    IEnumerator Cycle()
    {
        while(p1Priority.Count > 0 || p2Priority.Count > 0 )
        {
            //queue내의 멤버를 빼옴
            CharaScript member1 = new CharaScript();
            CharaScript member2 = new CharaScript();
            if(p1Priority.Count != 0)
             member1 = p1Priority.Dequeue();
            if(p2Priority.Count != 0)
             member2 = p1Priority.Dequeue();

            //3인이유는 명령이 증감하지않을거고 테이블에서 3개를 무조건 받았기때문
            for (int j = 0; j < 3; j++)
            {
                //명령실행 << 이것과 똑같은게 member2로도 들어갈것
                if (member1 != null)
                {
                    member1.ActionOrder();
                    yield return new WaitForSeconds(2f);//무조건 2초쉼
                }
            }

        }
        //저거다 벗어나면
        //자기자신을 멈춰도 유효하지않을까
        Debug.Log("사이클을 나갔다");
        yield break;
    }
    //빠른정렬이 아니라 queue에 넣기 쉽게 정리하는거
    int SpeedSort(CharaScript m1, CharaScript m2)
    {
        if (m1.Info.speed > m2.Info.speed)
            return -1;
        else if (m1.Info.speed < m2.Info.speed)
            return 1;
        else return 0;
    }
    void PushQueues()
    {
        //p2는 아직 이동,공격이먼저
        var p1 = PartyManager.inst.Member1;
        var p2 = PartyManager.inst.Member2;
        p1.Sort(SpeedSort);
//        p2.Sort(SpeedSort);
        for (int i = 0; i < Mathf.Max(p1.Count, p2.Count); i++)
        {
            Debug.Log("사이즈 : " + i + " " +p1.Count);
            if (p1.Count > i)
                p1Priority.Enqueue(p1[i]);
//            if (p2.Count > i)
//                p2Priority.Enqueue(p2[i]);
            Debug.Log(p1Priority.Peek());
        }

    }
    //턴을 실행하기
    public void ActionTurn()
    {
        PushQueues();//큐부터 활성화
        //2개의 PartyMember를 돌리기
        //CyCle로 이곳이 멈추도록?
        StartCoroutine(Cycle() );

    }
    void Awake()
    {
        if (inst == null) inst = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
