using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField]
    private List<OrderScriptableObj> orders;//내가 받은 명령들 
    private int maxOrder = 3;

    //세분해서 겟하게 하면 좋긴 할텐데 말이지
    public CharaScriptable Info
    {
        get
        {
            return info;
        }
    }
    
    public void AddOrder(OrderScriptableObj order)
    {
        for(int i = 0; i < maxOrder; i++)
        {
            if(orders[i] == null)
            {
                orders.Add(order);
                break;
            }
        }
    }
    //클릭하면 행할거임
    public void DelOrder(int cur)
    {
        if (cur >= maxOrder) return;
        orders[cur] = null;
    }
	// Use this for initialization
	void Start () {
        /*
        if (charaPath == null) charaPath = "CharaDB";
        charaFile = Resources.Load(charaPath) as TextAsset;
        */
	}
    
	
	// Update is called once per frame
	void Update () {

    }
}
