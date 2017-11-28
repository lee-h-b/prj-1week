using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//1순위 캐릭터 셀렉트의 정보를 여기로 다시 가져옴
//코드의 수정이 불가피할거같음 가져오는 이유는 AI를 한다면 나올 확장(오브젝트추가)때문
public class GameManager : MonoBehaviour {
    [SerializeField]//멤버의 최대 카운트는 셀렉트에서 관여하도록 배열이 나을지도?
    List<CharaScript> playerMember;
    [SerializeField]
    private int maxMember = 3;//난 안바꿀꺼지만 바꿀수 있도록 해봄
    static public GameManager inst;
    // Use this for initialization
    //멤버추가 캐릭에서 스크립트가 있늕지 보고 있음 따오는식
    public void AddMember(GameObject chara)
    {
        if (playerMember.Count >= maxMember) return;
        var charaData = chara.GetComponent<CharaScript>();
        if (charaData == null) return;
        else
            playerMember.Add(charaData);
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
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        //이제 게임매니저에서 뽑아써봐야함
    }
    //Get시리즈가 너무많아지면 아예 캐릭터를 반환 시킬수 있음 지금은 UI내에 쓸 2가지만 하니 넘어감
    public Sprite GetImg(int cur)
    {
        if (playerMember.Count <= cur) return null;

        return playerMember[cur].Info.image;
    }
    public string GetName(int cur)
    {
        if (playerMember.Count <= cur) return null;

        return playerMember[cur].Info.charaName;
    }


    void Awake()
    {
        if (inst == null) inst = this;
        DontDestroyOnLoad(this);
    }
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
