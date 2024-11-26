using UnityEngine;
using System.IO;
using System.Linq;

public class ZombieStatsManager : MonoBehaviour
{
    public ZombieStats[] ZombieStatsArray;

    //현재 게임 웨이브 파악
    private int _wave = 1;

    private void Start()
    {
        //60초마다 웨이브 업그레이드 되는 이벤트에 좀비 스탯함수 걸기
        Managers.Instance.Game.TimeController.WaveUpgrade += ZombieUpgrade;
    }
    
    /// <summary>
    /// 시간이 흐름에 따라 좀비 스탯을 올려주는 함수
    /// </summary>
    /// <param name="minutesPassed">지나간 시간</param>
    public void UpdateZombieStatsOverTime(float minutesPassed)
    {
        for (int i = 0; i < ZombieStatsArray.Length; i++)
        {
            // 시간이 경과됨에 따라 각 좀비의 스탯을 증가
            ZombieStatsArray[i].speed += ZombieStatsArray[i].speedIncreasePerMinute * minutesPassed;
            ZombieStatsArray[i].attackPower += ZombieStatsArray[i].attackPowerIncreasePerMinute * minutesPassed;
            ZombieStatsArray[i].hp += ZombieStatsArray[i].hpIncreasePerMinute * minutesPassed;
        }
    }

    public void ZombieUpgrade()
    {
        _wave++;

        switch (_wave)
        {
            case 2:
                ZombieStatsArray[0].hp = 12;
                ZombieStatsArray[0].attackPower = 2;

                ZombieStatsArray[1].hp = 10f;
                ZombieStatsArray[1].attackPower = 5;

                ZombieStatsArray[2].hp = 84f;
                ZombieStatsArray[2].attackPower = 20;
                break;
            case 3:
                ZombieStatsArray[0].hp = 33;
                ZombieStatsArray[0].attackPower = 5;

                ZombieStatsArray[1].hp = 16f;
                ZombieStatsArray[1].attackPower = 6;

                ZombieStatsArray[2].hp = 172f;
                ZombieStatsArray[2].attackPower = 30;
                break;
            case 4:
                ZombieStatsArray[0].hp = 68;
                ZombieStatsArray[0].attackPower = 8;

                ZombieStatsArray[1].hp = 32f;
                ZombieStatsArray[1].attackPower = 7;

                ZombieStatsArray[2].hp = 292f;
                ZombieStatsArray[2].attackPower = 40;
                break;
            case 5:
                ZombieStatsArray[0].hp = 135;
                ZombieStatsArray[0].attackPower = 13;

                ZombieStatsArray[1].hp = 56f;
                ZombieStatsArray[1].attackPower = 8;

                ZombieStatsArray[2].hp = 444f;
                ZombieStatsArray[2].attackPower = 45;
                break;
            case 6:
                ZombieStatsArray[0].hp = 234;
                ZombieStatsArray[0].attackPower = 18;

                ZombieStatsArray[1].hp = 190f;
                ZombieStatsArray[1].attackPower = 9;

                ZombieStatsArray[2].hp = 628f;
                ZombieStatsArray[2].attackPower = 60;
                break;
            case 7:
                ZombieStatsArray[0].hp = 371;
                ZombieStatsArray[0].attackPower = 25;

                ZombieStatsArray[1].hp = 236f;
                ZombieStatsArray[1].attackPower = 10;

                ZombieStatsArray[2].hp = 964f;
                ZombieStatsArray[2].attackPower = 70;
                break;
            case 8:
                ZombieStatsArray[0].hp = 536;
                ZombieStatsArray[0].attackPower = 32;

                ZombieStatsArray[1].hp = 396f;
                ZombieStatsArray[1].attackPower = 21;

                ZombieStatsArray[2].hp = 1227f;
                ZombieStatsArray[2].attackPower = 80;
                break;
            case 9:
                ZombieStatsArray[0].hp = 756;
                ZombieStatsArray[0].attackPower = 40;

                ZombieStatsArray[1].hp = 572f;
                ZombieStatsArray[1].attackPower = 22;

                ZombieStatsArray[2].hp = 1852f;
                ZombieStatsArray[2].attackPower = 90;
                break;
            case 10:
                ZombieStatsArray[0].hp = 1030;
                ZombieStatsArray[0].attackPower = 49;

                ZombieStatsArray[1].hp = 777f;
                ZombieStatsArray[1].attackPower = 33;

                ZombieStatsArray[2].hp = 2590f;
                ZombieStatsArray[2].attackPower = 95;
                break;
        }
    }
}


[System.Serializable]
public struct ZombieStats
{
    public int ID;               // 좀비 ID
    public float rotationSpeed;  // 회전 속도
    public float speed;          // 이동 속도
    public float attackPower;    // 공격력
    public float hp;             // 체력
    public int exp;

    // 시간이 지남에 따라 증가하는 스탯 속성
    public float speedIncreasePerMinute;   // 1분마다 이동 속도가 증가하는 값
    public float attackPowerIncreasePerMinute; // 1분마다 공격력이 증가하는 값
    public float hpIncreasePerMinute;      // 1분마다 체력이 증가하는 값

    // 생성자 정의
    public ZombieStats(int ID, float rotationSpeed, float speed, float attackPower, float hp, int exp,
                       float speedIncreasePerMinute, float attackPowerIncreasePerMinute, float hpIncreasePerMinute)
    {
        this.ID = ID;
        this.rotationSpeed = rotationSpeed;
        this.speed = speed;
        this.attackPower = attackPower;
        this.hp = hp;
        this.exp = exp;

        this.speedIncreasePerMinute = speedIncreasePerMinute;
        this.attackPowerIncreasePerMinute = attackPowerIncreasePerMinute;
        this.hpIncreasePerMinute = hpIncreasePerMinute;
    }
}