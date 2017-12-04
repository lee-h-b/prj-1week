using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/SaveData")]
public class SaveData : UnitStatSystem{
    [SerializeField]
    private int point;//뽑기랑 업그레이드용
    [SerializeField]
    private int enemyLevel;//적의 레벨(난이도)
	// Use this for initialization
    public int Point
    {
        get { return point; }
    }
    public int EnemyLevel
    {
        get { return enemyLevel; }
    }
    public void AddPoint(int val = 1)
    {
        point += val;
    }
    public void AddLevel()
    {
        enemyLevel++;
    }

    public void SubLevel()
    {
        if (point <= 2) return;
        enemyLevel--;
        point -= 2;
    }
    //스피드는 쓸대 없을듯
    public void AddAtk()
    {
        if (point <= 0) return;
        stat.atk++;
        point--;

    }
    public void AddHp()
    {
        if (point <= 0) return;
        stat.hp += 5;
        point--;

    }
    public void AddMp()
    {
        if (point <= 0) return;
        stat.mp+=5;
        point--;

    }
    public void AddDef()
    {
        if (point <= 0) return;
        stat.def++;
        point--;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
