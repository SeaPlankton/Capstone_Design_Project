using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCombat : MonoBehaviour
{
    [HideInInspector]
    public Player Player;

    public int MaxHP;
    public int HP;
    [HideInInspector]
    public int LV;
    //���� ����ġ
    [HideInInspector]
    public int EXP;

    //����ġ ��
    private int MaxEXP = 5;

    // �ӽ�
    public HpBarView HpBarView;
    public ExpBarView ExpBarView;

    //N�� ���� �� ȸ���ϱ����� ƽ üũ - _tickCheck = 1�� üũ
    private int _tickCheck = 0;

    //35% Ȯ���� ���ݹ����ϴ� ���ɰ� �׼����� ȹ�� ����
    private bool _isMissAccessory = false;

    //��Ȱ�� �׼� �ִ��� üũ
    private bool _checkRebornAccessory = false;
    //���� ��ź����� �����̴�?
    private bool _goingDie = false;
    //�޽þ� ���� ųī��Ʈ
    private int _killCount = 35;
    //�׾��� �� �ѹ��� ����ǰ�
    private bool _isDie = false;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    private void Start()
    {
        Managers.Instance.Game.TimeController.PlayerRetry += RebornThroughUI;
        HP = MaxHP;
        LV = 1;
        EXP = 0;
        HpBarView.SetHp(HP, MaxHP);
        ExpBarView.SetExp(EXP, MaxEXP);
        _isDie = false;
    }

    //�� óġ�� ����ġ ���
    public void KillEnemy(int exp)
    {
        //��ź ����� ����
        if (_goingDie)
        {
            _killCount--;
            Managers.Instance.Game.DieUIPresenter.GetLeftMessiaCount(_killCount);
            //�ð� �ȿ� 10���� ��⿡ �������� ��
            if (_killCount <= 0)
            {
                Managers.Instance.Game.TimeController.RebornSuccess();
                _goingDie = false;
                HP = MaxHP;
                HpBarView.SetHp(HP, MaxHP);
                _killCount = 0;
            }
        }
        EXP += exp;
        ExpBarView.SetExp(EXP, MaxEXP);

        //����ġ 20 �̻�Ǹ� ���� ��, �ð� ���߰� ��ʶ���
        if (EXP >= MaxEXP)
        {
            EXP = EXP - MaxEXP;
            LV += 1;
            SetExpAmount(LV);
            if(LV > 3)
            {
                Managers.Instance.Game.TimeController.SpawnSpecialZombie();
            }           
            Managers.Instance.Game.BannerPresenter.LevelUp();
            ExpBarView.SetExp(EXP, MaxEXP);
        }
    }

    //��Ƽ�� - ������ �׼����� ����
    //�ִ� ü�� ����
    public void IncreaseMaxHP(int percent)
    {
        MaxHP += percent;
        HpBarView.SetHp(HP, MaxHP);
    }

    //ü�� ȸ��
    public void RecoverHP(int percent)
    {
        if (HP + percent > MaxHP)
            HP = MaxHP;
        else
            HP += percent;
        HpBarView.SetHp(HP, MaxHP);
    }

    //��Ƽ�� �׼����� - ����
    //10�ʴ� 5�� ȸ��
    public void RecoverHP()
    {
        _tickCheck++;

        //10�� ���� �� ȸ��
        if (_tickCheck == 10)
        {
            if (HP + 5 > MaxHP)
                HP = MaxHP;
            else
                HP += 5;

            _tickCheck = 0;
        }
        HpBarView.SetHp(HP, MaxHP);
    }

    //��Ȱ�̳� Ǯ�� ȸ���϶�
    public void RecoverFullHP()
    {
        HP = MaxHP;
        HpBarView.SetHp(HP, MaxHP);
    }

    //ü�� �϶��ϴ� �׼����� ��
    public void DropHP(int percent)
    {
        //ü���� 0 ���Ϸ� ��������
        if (HP - percent < 1)
            HP = 1;
        else
            HP -= percent;
        HpBarView.SetHp(HP, MaxHP);
    }
    /// <summary>
    /// �÷��̾�� �������� ����.
    /// </summary>
    /// <param name="Damage">���� ������</param>
    public void Hit(int Damage)
    {
        //���ɰ� �׼����� ȹ�� �� 35% Ȯ���� ���ݹ���
        if (_isMissAccessory)
        {
            int percent = Random.Range(0, 101);

            if (percent < 75)
            {
                return;
            }
        }

        HP -= Damage;
        if (HP < 0 && !_isDie)
        {
            HP = 0;
            _isDie = true;

            // ���� ó�� - ��Ȱ�� ������ ���UI, ��Ȱ�� ������ 5�ʰ� ����
            _checkRebornAccessory = Managers.Instance.Game.AccessoryController.FindRebornAccessory();

            //��Ȱ�� ������
            if (!_checkRebornAccessory)
            {
                Managers.Instance.Game.TimeController.DiePlayer();
            }
            //��Ȱ�� ������
            else
            {
                Managers.Instance.Game.TimeController.CheckRebornTime();
                _killCount = 35;
                _goingDie = true;
            }
        }
        HpBarView.SetHp(HP, MaxHP);
    }
    //���������� HPȸ���ϴ� �׼�����
    public void ContinuouslyRecoverHP()
    {
        Managers.Instance.TimeManager.CountTime(9999, 1, RecoverHP);
    }

    //��Ƽ�� �׼����� - ���ɰ�, 35% Ȯ���� ���ݹ���
    public void SetMissDamage()
    {
        _isMissAccessory = true;
    }

    //������ �� ����ġ�� ����
    public void SetExpAmount(int lv)
    {
        //����ġ�� ������ 50������ ����
        if (lv > 50)
            return;

        float value = 4 + (Mathf.Pow((float)lv / 10.0f, 2f) * 96);
        MaxEXP = Mathf.CeilToInt(value);
    }

    //UI�� ���� ��Ȱ ���� ��, ��ź����� ���¿����� ��������
    private void RebornThroughUI()
    {
        _goingDie = false;
        _isDie = false;
        HP = MaxHP;
        HpBarView.SetHp(HP, MaxHP);
    }

    public void SetDieFalse()
    {
        _isDie = false;
    }
}
