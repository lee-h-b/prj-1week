using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//1순위 캐릭터 셀렉트의 정보를 여기로 다시 가져옴
//코드의 수정이 불가피할거같음 가져오는 이유는 AI를 한다면 나올 확장(오브젝트추가)때문
public class GameManager : MonoBehaviour {
        [SerializeField]//멤버의 최대 카운트는 셀렉트에서 관여하도록 배열이 나을지도?
    //  List<CharaScript> playerMember;//게임오브젝트로? 그냥 여기 정보를 토대로 캐릭터에서 찾ㅇ서 꺼내라함
    List<int> playerMember;//캐릭터 스크립트를 잘 안쓰는거같아서 int로 변경
    [SerializeField]
    private int maxMember = 3;//난 안바꿀꺼지만 바꿀수 있도록 해봄
    public List<GameObject> characters;//캐릭터 정보들 public인 이유는 접근의 용이성
    public List<GameObject> orders;//명령들 스킬테이블로 주기전에 보관하는 역활

    public Envi mapEnvi;//그라운드를 인스턴스 시키는것보단 이게 낫다고 판단 그라운드가 대입시켜줌

    static public GameManager inst;
    
    // Use this for initialization
    //멤버추가 캐릭에서 스크립트가 있늕지 보고 있음 따오는식
    public List<int> PlayerMember
    {
        get { return playerMember; }
    }
    public void AddMember(int cur)
    {
        if (playerMember.Count >= maxMember ) return;
        var charaData = characters[cur];
        if (charaData == null) return;
        else
            playerMember.Add(characters[cur].GetComponent<CharaScript>().Info.ID );
    }
    //캐릭을 빼옴!
    public void DelMember()
    {
        if (playerMember.Count <= 0) return;
//        Debug.Log(playerMember.Count - 1);
        playerMember.RemoveAt(playerMember.Count - 1);
    }
    public void LoadScene(string sceneName)
    {
        if (playerMember.Count <= 0) return;//어짜피 플레이어가 있어야 뭔가 움직이고 할테니깐

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        //이제 게임매니저에서 뽑아써봐야함
    }
    public void GetAllChara()
    {
        //        string[] inFiles2 = Directory.GetFiles(Application.dataPath + "/Resources/Chara");
        //LoadAll로 결정 이유는 짧아서 그리고 빌드할때도 이게 나을지도?

        var loadData = Resources.LoadAll("Chara");
        for (int i = 0; i < loadData.Length; i++)
        {
            characters.Add(loadData[i] as GameObject);
//            SummonImg(characters[i].GetComponent<CharaScript>());
        }
    }
    public void GetAllOrder()
    {
        var loads = Resources.LoadAll("Order");
        for(int i = 0; i <loads.Length; i++)
        {
            orders.Add(loads[i] as GameObject);
        }
    }
    //Get시리즈가 너무많아지면 아예 캐릭터를 반환 시킬수 있음 지금은 UI내에 쓸 2가지만 하니 넘어감
    public Sprite GetImg(int cur)
    {
        if (playerMember.Count <= cur) return null;

        //아이디가 0= 없음이니 cur위치에서 -1위치
        return characters[ playerMember[cur]-1 ].GetComponent<CharaScript>().Info.image;
    }
    public string GetName(int cur)
    {
        if (playerMember.Count <= cur) return null;

        return characters[playerMember[cur] - 1].GetComponent<CharaScript>().Info.charaName;
    }

    void Awake()
    {
        if (inst == null) inst = this;
        DontDestroyOnLoad(this);
        GetAllOrder();
        GetAllChara();
    }
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
