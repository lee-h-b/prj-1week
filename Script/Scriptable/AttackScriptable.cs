using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Create/DamageAction")]//데미지를 주는액션쯤 방어,회복은 추후에 <<하게되면 태그의등장
public class AttackScriptable : OrderScriptableObj {
    enum DIAGONAL { None, Only,Both }
    enum DIRECTION { None, Both}
    public int dmg;
    //대각선이문제 십자는 뭐 양방향인지 아닌지를 넣는다고치고
    //대각선은 어떻게 구현할것? 간단하게 참이면 좀 꺾어서 하는건? 불안전할듯 데미지를 어떻게 줄지?
    //빈오브젝트(아마 이펙트?)를 복제해서 넣고 소멸시킬듯 
    //예상안
    /*
    //박스컬라이더를 내가 고른 범위내에서 찾아서 잡기
    아마 Ray를 쓰지않을까 싶음 ray를 통해 찍힌 애들을 tiles라는 범위로 포함시키는거지
    그다음에 거기위치에 내가 데미지(이펙트?)를 주는것
    발사위치는? 내가 있는 자리로함 그러면 move가 tile을 교체해주는 작업을 하게될까?
    //액션오더가 하라고함그리고 tile도 캐릭이 가지는게? 타일을 가지고 있어서 얘가 gameObj? charaScript?에서
    타일을 불러오는거지 그리고 거기서 반경 x,y 대각선이면 45도 회전해서 레이를 쏘고 그레이전체에 데미지를 주는
    이펙티브를 쏘게하는거임
    닿은타일전체에 데미지를 주라고 시키고 동시에 파티클을 발생시키라고시킴
    파티클의 복제가 안됐던걸로 기억 << 자식으로 파티클을 넘기는 방식으로 변경
    //ㄴ태그의 존재가 등장할것 태그가 없으면 데미지를 나도받음
    가로세로대각선말고 다른방식 그러니깐 전방 3칸 이런걸로 하고싶음
    무식하게 레이 8개?
    //더나아가서 저레이 8개를 플레이어가 기본 소지하는거임
    //ㄴ 그럴거면 차라리 move를 ray의존형으로 하는게 나을듯
    //저렇게하면 각진 형태밖에 안됨 5*5형태를 해도
    ||모양이힘들다이거  그래서 고민한게 레이의 위치를 수정,각도도 수정하는거임 그ray들을 얘가 가지고있고
    그ray를 어떻게 가지고 있을래? ray가아닌 transform으로?
    레이로 결정
    */
    //레이는 8개 모두있다는 가정하에 움직임 << 커맨드가 많아지면 불리할텐데 < 10~30명 이런식이 아니니 큰문제는x일듯

    [SerializeField]
    bool safeStart = false;//시작위치는 뎀지를 안줄것? 이란뜻
    Quaternion diagonal = Quaternion.AngleAxis(45.0f, Vector3.up);//45도각도 이건 안바뀔듯
    Vector3 startPos;
    [SerializeField]
    bool Moveable = false;//이동여부
    [SerializeField]
    DIAGONAL diago = DIAGONAL.None;
    [SerializeField]
    DIRECTION direc = DIRECTION.None;

    GameObject HittedObj;//여기서 맞았던 오브젝을 치울수도있음
    public void test(GameObject master)
    {
        Vector3 startPos = new Vector3(master.transform.position.x, 0f, master.transform.position.z);
        Vector3 var = master.transform.right * (120);

        Ray up = new Ray(startPos, master.transform.forward * 20f );
        Ray down = new Ray(startPos, -master.transform.forward *100f);
        Ray Rside = new Ray(startPos, var);
        Ray Lside = new Ray(startPos, -master.transform.right * 200f);

        Ray RUdiagonal = new Ray(startPos, diagonal * master.transform.forward);
        Ray RDdiagonal = new Ray(startPos, diagonal * master.transform.right);
        Ray LUdiagonal = new Ray(startPos, diagonal * -master.transform.right);
        Ray LDdiagonal = new Ray(startPos, diagonal * -master.transform.forward);

        Debug.DrawRay(up.origin, up.direction *50, Color.black);//위
        Debug.DrawRay(down.origin, down.direction * 50, Color.gray);//아래
        Debug.DrawRay(Rside.origin, Rside.direction * 50, Color.red);//오른
        Debug.DrawRay(Lside.origin, Lside.direction * 50, Color.blue);//왼

        Debug.DrawRay(LDdiagonal.origin, LDdiagonal.direction * 50f, Color.cyan);// 좌하단
        Debug.DrawRay(RDdiagonal.origin, RDdiagonal.direction * 50f, Color.green);//우하단
        Debug.DrawRay(RUdiagonal.origin, RUdiagonal.direction * 30f, Color.yellow);//우상단
        Debug.DrawRay(LUdiagonal.origin, LUdiagonal.direction * 30f, Color.magenta);//좌상단

    }
    //상황에따라서는 여기 ray분류에서 역방향을 따로 구해줘야한다
    public override void Action(GameObject master)
    {
        //마나체크
        if (cost >= master.GetComponent<CharaScript>().Status.mp) return;

        master.GetComponent<CharaScript>().PayMp(cost);
           
        var xSync = master.transform.position.x;
        if (master.tag == "P1") xSync += 5;//x의 차이에 따라 대각선의 값이 약간 바뀜 따라서 태그에따라
        else if (master.tag == "P2") xSync -= 5;//값의차이를 동기화? 시켜줌
        Vector3 startPos = new Vector3(xSync, 0, master.transform.position.z);//기존 1에서 0ㅇ,로바꿈 닿은게 타일이면 파티클터지게
        //높이에 영향을 주는 y는 필요없음
        //레이에 담아서 반복문을 통해 레이를 쏘게하면 지장이 없을까?
        //ray[0],ray[0] *50f 이렇게 할테고 이미 쏠 방향은 정해뒀기에 지장은 없다고봄 즉 여긴 방향의 설정임
        //오히려 개개인 이름달아주면 반복이 어려움 
        List<Ray> rays = new List<Ray>();//얘가 하게될 ray들
        List<int> dist = new List<int>();//ray가 뻗게할 길이를 얘가 가짐 비효율적인데 dist가 초기화되니까..
        //만약 X가 0보다 크거나 양방향일경우
        if ((x != 0) && (diago == DIAGONAL.None || diago == DIAGONAL.Both))
        {
            rays.Add(new Ray(startPos, master.transform.forward));
            dist.Add(x * envi.gridSize );//ray의 길이를 설정 //전에는 보정으로 +10이던가 +gridsize했는데 할필요X
        }
        //반대방향은 나중에 direc == DIRECTION.BOTH면함 -던 +던 한방향을하고 BOTH의 경우 반대방향도 포함하는식임
        //8개 -> 4개로 줄어드는 이득
        if ((y != 0) && (diago == DIAGONAL.None || diago == DIAGONAL.Both))
        {
            rays.Add(new Ray(startPos, master.transform.right));//오른쪽방향레이추가
            dist.Add(y * envi.gridSize  );
        }
        //대각선은 우측으로 45도 돌린거임
        if ((x != 0) && (diago == DIAGONAL.Only || diago == DIAGONAL.Both))//만약 대각선이고 x가 있다면
        {
            rays.Add(new Ray(startPos, diagonal * master.transform.forward));
            dist.Add(x * envi.gridSize + envi.gridSize );
        }
        if ((y != 0) && (diago == DIAGONAL.Only || diago == DIAGONAL.Both))
        {
            rays.Add(new Ray(startPos, diagonal * master.transform.right));
            dist.Add(y * envi.gridSize + envi.gridSize );
        }//대각선은 공식을 달리 해야할까?

        //만약 내가 있던곳도 데미지를 줄거면 << 데미지 계산
        if (safeStart == false)
        {
            var overlap = Physics.OverlapBox(master.transform.position, new Vector3(envi.gridSize / 4, envi.gridSize / 2, envi.gridSize / 4));
            for(int i = 0; i < overlap.Length; i++)
            {
                if(overlap[i].gameObject.tag != master.tag && overlap[i].gameObject.GetComponent<CharaScript>() )
                {
                    overlap[i].gameObject.GetComponent<CharaScript>().GetDamage(dmg + master.GetComponent<CharaScript>().Status.atk);
                }                    
            }
        }
        for(int i = 0; i < rays.Count; i++)
        {

            RaycastHit[] forwardHits;//앞만되고 추가가 안되서 아래만듬
            RaycastHit[] backHits = null;//뒤에맞춘애들ㅎ
            forwardHits = Physics.RaycastAll(rays[i],dist[i]);
            if (direc == DIRECTION.Both)
            {
                var r_Ray = rays[i];//dist가 음수로 안되길래 이렇게했음
                r_Ray.direction = -r_Ray.direction ;
                backHits = Physics.RaycastAll(r_Ray, dist[i]);//거리를 반대로 함으로써 반대편을 얻음을꾀한다
                for(int j = 0; j < backHits.Length; j++)
                {
                    if (backHits[j].transform.GetComponent<TileScript>() != null && master.GetComponent<CharaScript>().particle != null)
                    {
                        var temp = Instantiate(master.GetComponent<CharaScript>().particle, backHits[j].transform);
                        if (master.tag == "P1")
                        {
                            var sc = temp.GetComponent<ParticleSystem>().main;
                            sc.startColor = Color.blue;
                        }
                        Destroy(temp.gameObject, 2f);
                    }
                    if (backHits[j].transform.GetComponent<CharaScript>() != null && backHits[j].transform.tag != master.tag)
                    {
                        backHits[j].transform.GetComponent<CharaScript>().GetDamage(dmg + master.GetComponent<CharaScript>().Status.atk);
                    }
                }
            }
            for(int j = 0; j < forwardHits.Length; j++)
            {

                if (forwardHits[j].transform.GetComponent<TileScript>() != null && master.GetComponent<CharaScript>().particle != null)
                {
                    var temp = Instantiate(master.GetComponent<CharaScript>().particle, forwardHits[j].transform);
                    if (master.tag == "P1")
                    {
                        var sc = temp.GetComponent<ParticleSystem>().main;
                        sc.startColor = Color.blue;
                    }

                        Destroy(temp.gameObject, 2f);

                }
                if (forwardHits[j].transform.GetComponent<CharaScript>() != null && forwardHits[j].transform.tag != master.tag )
                {
                    forwardHits[j].transform.GetComponent<CharaScript>().GetDamage(dmg + master.GetComponent<CharaScript>().Status.atk);
                }
                    
//                    Debug.Log(backHits[j].transform.gameObject.name + " " + backHits[j].transform.parent);
 //                   Debug.Log(backHits.Length);
                    //                    Debug.Log("Bhits : i" + backHits[j].transform.name);
//                    Debug.DrawLine(rays[i].origin, backHits[j].point, Color.green, dist[j]);
                    
            }
        }
        if (Moveable == true)
            base.Action(master);


    }

    // Update is called once per frame
    void Update () {
		
	}
}
