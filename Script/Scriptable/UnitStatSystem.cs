using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    public int hp;
    public int mp;
    public int atk;
    public int def;
    public int speed;

    public static Stat operator +(Stat s1, Stat s2)
    {
        Stat temp = new Stat();
        temp.hp = s1.hp + s2.hp;
        temp.mp = s1.mp + s2.mp;
        temp.atk = s1.atk + s2.atk;
        temp.def = s1.def + s2.def;
        temp.speed = s1.speed + s2.speed;
        return temp;
    }
}
//유닛 스탯시스템, 이걸 이용해서 플레이어 유닛의 업그레이드르 관여할예정
public class UnitStatSystem : ScriptableObject {
    [SerializeField]
    protected Stat stat;
    public Stat Stat
    {
        get { return stat; }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
