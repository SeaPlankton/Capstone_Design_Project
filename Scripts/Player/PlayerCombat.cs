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
    //현재 경험치
    [HideInInspector]
    public int EXP;

    //경험치 통
    private int MaxEXP = 5;

    // 임시
    public HpBarView HpBarView;
    public ExpBarView ExpBarView;

    //N초 마다 피 회복하기위해 틱 체크 - _tickCheck = 1초 체크
    private int _tickCheck = 0;

    //35% 확률로 공격무시하는 유령검 액세서리 획득 여부
    private bool _isMissAccessory = false;

    //부활기 액세 있는지 체크
    private bool _checkRebornAccessory = false;
    //지금 폭탄목걸이 상태이니?
    private bool _goingDie = false;
    //메시아 전용 킬카운트
    private int _killCount = 35;
    //죽었을 때 한번만 실행되게
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

    //적 처치시 경험치 계산
    public void KillEnemy(int exp)
    {
        //폭탄 목걸이 해제
        if (_goingDie)
        {
            _killCount--;
            Managers.Instance.Game.DieUIPresenter.GetLeftMessiaCount(_killCount);
            //시간 안에 10마리 잡기에 성공했을 때
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

        //경험치 20 이상되면 레벨 업, 시간 멈추고 배너띄우기
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

    //고티어 - 불제봉 액세서리 전용
    //최대 체력 증가
    public void IncreaseMaxHP(int percent)
    {
        MaxHP += percent;
        HpBarView.SetHp(HP, MaxHP);
    }

    //체력 회복
    public void RecoverHP(int percent)
    {
        if (HP + percent > MaxHP)
            HP = MaxHP;
        else
            HP += percent;
        HpBarView.SetHp(HP, MaxHP);
    }

    //고티어 액세서리 - 부적
    //10초당 5씩 회복
    public void RecoverHP()
    {
        _tickCheck++;

        //10초 마다 피 회복
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

    //부활이나 풀피 회복일때
    public void RecoverFullHP()
    {
        HP = MaxHP;
        HpBarView.SetHp(HP, MaxHP);
    }

    //체력 하락하는 액세서리 용
    public void DropHP(int percent)
    {
        //체력이 0 이하로 떨어지면
        if (HP - percent < 1)
            HP = 1;
        else
            HP -= percent;
        HpBarView.SetHp(HP, MaxHP);
    }
    /// <summary>
    /// 플레이어에게 데미지를 가함.
    /// </summary>
    /// <param name="Damage">가할 데미지</param>
    public void Hit(int Damage)
    {
        //유령검 액세서리 획득 시 35% 확률로 공격무시
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

            // 죽음 처리 - 부활기 없으면 사망UI, 부활기 있으면 5초간 유예
            _checkRebornAccessory = Managers.Instance.Game.AccessoryController.FindRebornAccessory();

            //부활기 없으면
            if (!_checkRebornAccessory)
            {
                Managers.Instance.Game.TimeController.DiePlayer();
            }
            //부활기 있으면
            else
            {
                Managers.Instance.Game.TimeController.CheckRebornTime();
                _killCount = 35;
                _goingDie = true;
            }
        }
        HpBarView.SetHp(HP, MaxHP);
    }
    //지속적으로 HP회복하는 액세서리
    public void ContinuouslyRecoverHP()
    {
        Managers.Instance.TimeManager.CountTime(9999, 1, RecoverHP);
    }

    //고티어 액세서리 - 유령검, 35% 확률로 공격무시
    public void SetMissDamage()
    {
        _isMissAccessory = true;
    }

    //레벨업 시 경험치양 설정
    public void SetExpAmount(int lv)
    {
        //경험치양 증가는 50까지만 적용
        if (lv > 50)
            return;

        float value = 4 + (Mathf.Pow((float)lv / 10.0f, 2f) * 96);
        MaxEXP = Mathf.CeilToInt(value);
    }

    //UI를 통해 부활 했을 때, 폭탄목걸이 상태였으면 상태해제
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
