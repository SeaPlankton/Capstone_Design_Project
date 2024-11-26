using UnityEngine;
using System.IO;
using System.Linq;

public class ZombieStatsManager : MonoBehaviour
{
    public ZombieStats[] ZombieStatsArray;

    //���� ���� ���̺� �ľ�
    private int _wave = 1;

    private void Start()
    {
        //60�ʸ��� ���̺� ���׷��̵� �Ǵ� �̺�Ʈ�� ���� �����Լ� �ɱ�
        Managers.Instance.Game.TimeController.WaveUpgrade += ZombieUpgrade;
    }
    
    /// <summary>
    /// �ð��� �帧�� ���� ���� ������ �÷��ִ� �Լ�
    /// </summary>
    /// <param name="minutesPassed">������ �ð�</param>
    public void UpdateZombieStatsOverTime(float minutesPassed)
    {
        for (int i = 0; i < ZombieStatsArray.Length; i++)
        {
            // �ð��� ����ʿ� ���� �� ������ ������ ����
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
    public int ID;               // ���� ID
    public float rotationSpeed;  // ȸ�� �ӵ�
    public float speed;          // �̵� �ӵ�
    public float attackPower;    // ���ݷ�
    public float hp;             // ü��
    public int exp;

    // �ð��� ������ ���� �����ϴ� ���� �Ӽ�
    public float speedIncreasePerMinute;   // 1�и��� �̵� �ӵ��� �����ϴ� ��
    public float attackPowerIncreasePerMinute; // 1�и��� ���ݷ��� �����ϴ� ��
    public float hpIncreasePerMinute;      // 1�и��� ü���� �����ϴ� ��

    // ������ ����
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