using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Create/MoveOrder")]
//명령오브젝트 데미지,이동은 여기서 파생 될듯싶고
//여기서는 위치를 해줄 중심에서 최대 어떻게 갈지 나올 n(방향)과 최대갈수있는m
//ㄴ그냥 x축최대 x와 y축값 y로하자 이렇게하면 대각선 = x^2 +y^2가 될테니
//이동은X축가고 y축가고하는식이될듯
//공격은? X축과 Y축 그리고 추가로 대각축(z)도 있겠지


    //이동은 해당 n  위치에 닿은 목표로할거임 그럼 기존 위치에서 빼와야할건데
    //command를 Pos에서하기로 바꿀듯 빼오면 최소한 부활(?)은 쉬울테니
public class OrderScriptableObj : ScriptableObject {
    public int x, y;

    public string orderName;
    public int cost;//코스트 대쉬같은거 할때 코스트가 들게 할지도모름
    public int masterID;//마스터 전용키? 안쓸거같음

    //public int gridSize;//그리드의 크기 이거의 *X,Y를 할거니 필수고 바뀔 가능성 있으니 public 모든스킬이 들고 있는건 좀 넌센스

    //카드쓰는 주인이라 마스터 구체적인건 이걸 담을 얘가함
    //마스터 내에서 이동의 애니메이션도 재생할것
    //엔비는 그리드사이즈,그리드 최대크기 등등을 담당할 환경 디폴트로 하고싶었으나 실패 번거롭겠지만 모든스킬소유
    public Envi envi;    //스킬매니저(가칭)이 수정해줄거임


    //마스터를 이용해서 액션을 행함
    public virtual void Action(GameObject master)
    {
        Move(master);
    }
    //이동을 마우스를 이용해서 하도록 하는건 어떤지?
    public void Move(GameObject master)
    {
        //여기서 애니메이션 관리?

        bool up, right;
        if (x < 0) up = false;
        else up = true;
        if (y < 0) right = false;
        else right = true;

        //혹시모르니 새로만듬
        Vector3 period = new Vector3(master.transform.position.x,0,master.transform.position.z);
        Vector3 start = master.transform.position;
        for(int i = 0; i < Mathf.Abs(x); i++)
        {
            //x축으로 끝까지감
            if (right && period.x + envi.gridSize > envi.maxX)
                period.x += envi.gridSize;
            //x축으로 맨 왼쪽까지감
            else if(right == false && period.x -envi.gridSize < -envi.maxX)
                period.x -= envi.gridSize;
        }
        for(int i = 0; i < Mathf.Abs(y); i++)
        {
            if (up && period.z + envi.gridSize > envi.maxY)
                period.z += envi.gridSize;
            else if (up == false && period.z - envi.gridSize < -envi.maxY)
                period.z -= envi.gridSize;
        }
        //TODO : 도착했는데 겹치는일이 생긴다면 다른곳으로 이동해야할것
        master.transform.position = Vector3.Lerp(start,period, 20f);//20은 수정가능
    }
}
