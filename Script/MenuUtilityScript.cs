using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum DATAENUM { HP,MP,ATK,DEF,LEVEL}//외부에서 쓸수 있게될까? 하는 마음에
public class MenuUtilityScript : MonoBehaviour {
    
    public Text saveDataInfo;//옆에 text
    public Transform charaSelect;//캐릭 선택창
    public Transform playerUpgrade;//업그레이드
    public Transform casino;//돈넣고 돈먹기
    public Transform downPanel;
    // Use this for initialization
    void Start () {
        if (downPanel == null) downPanel = transform.parent.Find("DownPanel");
        if (saveDataInfo == null) saveDataInfo = transform.Find("InfoText").GetComponent<Text>();

        if (charaSelect == null) charaSelect = downPanel.Find("CharacterSelector");
        if (playerUpgrade == null) playerUpgrade = downPanel.Find("Upgrade");
        if (casino == null) casino = downPanel.Find("Gacha");
        WritePlayerData();
	}
    public void TurnScreen(string objName)
    {//해당 이름과 같으면 활성,아니면 비활성
        for(int i = 0; i < downPanel.childCount; i++)
        {
            if (downPanel.GetChild(i).name == objName)
                downPanel.GetChild(i).gameObject.SetActive(true);
            else
                downPanel.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void WritePlayerData()
    {
        var data = GameManager.inst.PlayerInfo;
        saveDataInfo.text = "추가체력 : " + data.Stat.hp.ToString() + "\n";
        saveDataInfo.text += "추가마나 : " + data.Stat.mp.ToString() + "\n";
        saveDataInfo.text += "추가공격력 : " + data.Stat.atk.ToString() + "\n";
        saveDataInfo.text += "추가방어력 : " + data.Stat.def.ToString() + "\n";
        saveDataInfo.text += "현재 적 레벨 : " + data.EnemyLevel.ToString() + "\n";
        saveDataInfo.text += "현재 보유 포인트 : " + data.Point.ToString() + "\n";
    }
    //위의 WritePlayerData를 업데이트에서 쓰기엔 비효율이라서만듬
    public void Upgrade(int code)
    {
        if (code == 0) GameManager.inst.PlayerInfo.AddHp();
        if (code == 1) GameManager.inst.PlayerInfo.AddMp();
        if (code == 2) GameManager.inst.PlayerInfo.AddAtk();
        if (code == 3) GameManager.inst.PlayerInfo.AddDef();
        if (code == 4) GameManager.inst.PlayerInfo.SubLevel();
        WritePlayerData();
    }
    public void Quit()
    {
        Application.Quit();
    }

}
