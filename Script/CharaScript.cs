using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//ui그리기 캐릭터 위에 게이지 보여주기
/// <summary>
/// 캐릭터 스크립트, 몬스터가 없으니 상속없이 이것 자체만으로 해결함
/// ai는 여기서 3종류를 떼서 쓰는식으로 할것이고
/// 스크립터는 여기서 임의의 값(ID만으로 충분할듯)만 떼감
/// 우선 구조체로 chara를 만들고 ID,이름, 체력, 마력,공격력,방어력,스피드만 빼고 설명은 따로 만듬 < 너무많으면 복잡해
/// 또한 추후의 능력치강화를 위해서라도 chara는 따로 만들고 하는게 좋겠음 
/// 아마 캐릭트랜스폼에다가 능력치란 이름으로 능력치 = chara능력치 (미리 준비되있거나,삭제금지건 배틀매니저? 거기에서 미리 세트해둘것) + 업그레이드능력치 이런식으로 대입받아서 쓸거로 추정됨
/// 
/// 여기서 주요 업무 DB따오기, DB의 ID에 맞는 아이템 뱉어주기 << ID는 또 딕셔너리로 가면 ID는 구성에서 빠질듯
/// 현재 있는 캐릭터의 순서에 맞게 트랜스폼 or 메쉬반환하기
/// ///chara 전부 scriptableobject를 활용하게 될듯
/// 
/// 스킬가차는 솔직히 모르겠음 스킬해금 시스템인데 좋은 스킬이 생각나야지
/// 스킬은 애니메이션을함과 동시에 다르게 파티클을 소환할 가능성이 큼
/// 스킬 방출 방식은? 수학적으로 임의의 위치에 소환식? 아니면 5*5크기의 빈 게임오브젝트를 만드는식? 이것도 고민
/// 
/// 그리고 저 스킬. 스킬의 직업에 따라 방출되는 스킬이 달라야함 스킬은 스킬스크립에서 설명
/// 아마떠오른건 스크립터블 오브젝에 100%의존함 그러면 txt로 db만드지 않아도 되지않을까?
/// </summary>
///
/* 보기 더럽지만 기존에 쓰던건 일단 주석처리
[System.Serializable]
public struct Chara
{
    public string name;
    public int hp;
    public int mp;
    public int atk;//주는 데미지에 추가
    public int def;//
    public int speed;//속도 높을수록 우선순위에서 먼저임
    /*이동속도도 2가지인데 1. a->b->a->b->a->b->a2->a3하던가 
     * a에서 가장빠른애 , b에서 가장빠른애 동시에 -> 그다음 ->그다음 -> 2번째카드.. 이런식일지
     * 후자가 게임에 맞겠고 전자가 만들기 쉽겠음
   
}*/
public class CharaScript : MonoBehaviour {
    /*    [SerializeField]
        Dictionary<int, Chara> charaDB;
        [SerializeField]
        TextAsset charaFile;
        string charaPath;//위의 위치정도
        */
    [SerializeField]
    //필요하면 이걸 Status라는 이름으로 넘길수도 있겠다
    //배틀할때 이걸 그대로 쓸거임

    private CharaScriptable info;//나의 정보
    private Stat stat;
    //const 할까 했는데 초기화를 안해서안됨
    private int maxHp;
    private int maxMp;

    [SerializeField]
    private List<OrderScriptableObj> orders;//내가 받은 명령들 
    private int maxOrder = 3;
    private bool dead = false;
    public bool DeadSwitch = false;
    public Transform particle;//없으면 안함
    //세분해서 겟하게 하면 좋긴 할텐데 말이지
    public CharaScriptable Info
    {
        get
        {
            return info;
        }
    }
    public Stat Status
    {
        get { return stat; }
    }
    //ai에게 주기위해서
    public List<OrderScriptableObj> Orders
    {
        get { return orders; }
    }
    public bool Dead
    {
        get { return dead; }
    }
    public int MaxHp
    {
        get { return maxHp; }
    }
    public int MaxMp { get { return maxMp; } }
    public void AddOrder(OrderScriptableObj order)
    {
        for(int i = 0; i < maxOrder; i++)
        {
            if(orders[i] == null)
            {
                orders[i] = order;
                
                break;
            }
        }
    }
    //클릭하면 행할거임
    public void PayMp(int val)
    {        
        stat.mp -= val;
        if (stat.mp > maxMp) stat.mp = maxMp;
        else if (stat.mp < 0) stat.mp = 0;
    }
    public void DelOrder(int cur)
    {
        if (cur >= maxOrder) return;
        orders[cur] = null;
    }
    public bool OrderEmpty()
    {
        if (orders[0] == null) return true;
        return false;
    }
    public void ActionOrder()//cur를 안받고 그냥함 맨앞에꺼니깐 이걸 지우고 밀어넣고할듯
    {
        orders[0].Action(this.gameObject);
        orders.RemoveAt(0);//이걸로될지? < 안됐음 사이즈를 아예줄여서 2번째부터 오류남
        orders.Add( null );//그래서 사용하고 다시 빈공간을 추가함으로써 순환시킴
    }
    //예상한문제가나옴 체력을 공유함 info때문으로 추정 이값을 복제해서 사용할 필요가있는데
    //이게임은 그래도 다른게임(능력치 100%쓰는(?))게임은 문제
    //간단하게 스탯복제하는수밖에 더있나?
    public void GetDamage(int dmg)
    {
        if (dmg < stat.def) dmg = stat.def;//뎀지를 +-0로 만듬
        stat.hp -= ( dmg - stat.def);
//        Debug.Log("맞은애 체력 " + stat.hp);
        //체력이 0이면 dead니깐 death를 구동하고 이거 비활성화로 할듯
        //체력이 0이면? 명령을 다지우고..번거롭게 하지않기위해 death라는걸 true로 만들고
        if(stat.hp <= 0)
        {
            orders = null;
            gameObject.SetActive(false);//존재를 숨김
            dead = true;
        }
            
    }
    //인게임 내에서 체력,마나를 보여주는거
    /*
    public void GaugeInGame()
    {
        Vector3 uiPos = transform.position;
        uiPos.y += 4f;
        uiPos.
    }
    */
    //세이브데이터를 직접적으로 +하지 않고 그냥 숫자로 업그레이드 하기에 퓨어를 붙여봄
    public void PurePowerUp(int value)
    {
        stat.atk += (int)(value * 0.75);
        stat.def += (int)(value * 0.5);
        stat.hp += (value * 5);
        stat.mp += (value * 5);
    }
    void Start () {
        /*
        if (charaPath == null) charaPath = "CharaDB";
        charaFile = Resources.Load(charaPath) as TextAsset;
        */
        //게임 매니저 내의 업그레이드를 합침 여기서하면 플레이어 능력치를 ai도 따간다 어디에 하든 똑같게될듯?
        //이게 게임 오브젝트니깐 이걸 복제하는 그순간 즉 Ground or 파티매니저에서 하자
        //has-a를 사용해서 스크립트에다가 추가하는식이 좋을듯
        if (tag == "P1")
            stat = info.Stat + GameManager.inst.PlayerInfo.Stat;
        else if (tag == "P2")
        {//여기서 적일경우도 추가됨
            stat = info.Stat;
            PurePowerUp(GameManager.inst.PlayerInfo.EnemyLevel);
        }
        else stat = info.Stat;
        
        maxHp = stat.hp;
        maxMp = stat.mp;
	}
    
	
	// Update is called once per frame
	void Update () {
        //임시, 죽으면 명령을 그만두고 다음애의 명령을 해야하는 구상
        if(DeadSwitch == true)
        {
            dead = true;
            gameObject.SetActive(false);//존재를 숨김
        }
    }
}
