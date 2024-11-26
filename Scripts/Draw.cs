using UnityEngine;

public class Draw : MonoBehaviour
{
    //아이템 먹었는지 확인
    [HideInInspector]
    private bool[] _checkAquiredItem = new bool[37];

    private void Start()
    {
        for (int i = 0; i < _checkAquiredItem.Length; i++)
        {
            _checkAquiredItem[i] = false;
        }
        _checkAquiredItem[10] = true;
    }

    //무기, 일반 악세서리 뽑는 함수
    public Item DrawNormalItem()
    {
        Item item = new Item();

        while (true)
        {
            int index = Random.Range(0, 26);
            item = GetItem(index);

            //중복가능이면
            if (item.Reduplicatable)
            {
                break;
            }
            //중복 불가능이지만
            else
            {
                //획득하지 않았으면
                if (!_checkAquiredItem[item.ItemNumber])
                {
                    //선행 아이템 필요하면
                    if (item.Requirement)
                    {
                        //선행아이템 조건 만족하면
                        if (CheckRequireItem(item))
                            break;
                        //선행아이템 조건 만족못하면 다음 while문으로
                        else
                        {
                            continue;
                        }
                    }
                    break;
                }
            }
        }
        return item;
    }

    //무기, 일반 + 고티어 악세서리 뽑는 함수
    public Item DrawRareItem()
    {
        Item item = new Item();

        while (true)
        {
            int index = Random.Range(26, 34);
            item = GetItem(index);

            //획득하지 않았으면
            if (!_checkAquiredItem[item.ItemNumber])
            {
                break;
            }
        }

        _checkAquiredItem[item.ItemNumber] = true;
        return item;
    }

    //회복 아이템 뽑는 함수
    //70% 25% 5%
    public Item DrawHealingPack()
    {
        Item item = new Item();

        int index = Random.Range(1, 101);

        if (index <= 5)
            item = GetItem(36);             //피 100회복
        else if (index > 5 && index <= 25)
            item = GetItem(35);             //피 70회복
        else if (index > 25)
            item = GetItem(34);             //피 30회복

        return item;
    }

    //아이템 획득시 획득 체크
    public void SelectItem(int index)
    {
        _checkAquiredItem[index] = true;

        //buckshot과 플레체 액세서리 설정
        if (index == 10)
            _checkAquiredItem[9] = false;
        else if (index == 9)
            _checkAquiredItem[10] = false;
    }

    //무기 아이템 최대레벨 달성 시 획득 불가능하게 중복 가능 False로 바꾸기
    public void LockMaxLevelItem(int index)
    {
        Managers.Instance.DataManager.ItemList.ItemList[index].Reduplicatable = false;
    }

    //무기 아이템 최대레벨 달성이였지만 버렸을 경우 다시 배너에 뜰 수 있게 중복 가능 True로 바꾸기
    public void UnLockMaxLevelItem(int index)
    {
        Managers.Instance.DataManager.ItemList.ItemList[index].Reduplicatable = true;
    }

    //아이템 가져오기
    private Item GetItem(int index)
    {
        Item item = new Item();

        item = Managers.Instance.DataManager.ItemList.ItemList[index];

        return item;
    }

    //선행아이템 조건 확인
    private bool CheckRequireItem(Item item)
    {
        bool ispossible = false;

        if (_checkAquiredItem[item.ItemNumber - 1] == true)
            ispossible = true;

        return ispossible;
    }
}
