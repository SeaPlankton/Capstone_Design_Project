using UnityEngine;

public class Draw : MonoBehaviour
{
    //������ �Ծ����� Ȯ��
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

    //����, �Ϲ� �Ǽ����� �̴� �Լ�
    public Item DrawNormalItem()
    {
        Item item = new Item();

        while (true)
        {
            int index = Random.Range(0, 26);
            item = GetItem(index);

            //�ߺ������̸�
            if (item.Reduplicatable)
            {
                break;
            }
            //�ߺ� �Ұ���������
            else
            {
                //ȹ������ �ʾ�����
                if (!_checkAquiredItem[item.ItemNumber])
                {
                    //���� ������ �ʿ��ϸ�
                    if (item.Requirement)
                    {
                        //��������� ���� �����ϸ�
                        if (CheckRequireItem(item))
                            break;
                        //��������� ���� �������ϸ� ���� while������
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

    //����, �Ϲ� + ��Ƽ�� �Ǽ����� �̴� �Լ�
    public Item DrawRareItem()
    {
        Item item = new Item();

        while (true)
        {
            int index = Random.Range(26, 34);
            item = GetItem(index);

            //ȹ������ �ʾ�����
            if (!_checkAquiredItem[item.ItemNumber])
            {
                break;
            }
        }

        _checkAquiredItem[item.ItemNumber] = true;
        return item;
    }

    //ȸ�� ������ �̴� �Լ�
    //70% 25% 5%
    public Item DrawHealingPack()
    {
        Item item = new Item();

        int index = Random.Range(1, 101);

        if (index <= 5)
            item = GetItem(36);             //�� 100ȸ��
        else if (index > 5 && index <= 25)
            item = GetItem(35);             //�� 70ȸ��
        else if (index > 25)
            item = GetItem(34);             //�� 30ȸ��

        return item;
    }

    //������ ȹ��� ȹ�� üũ
    public void SelectItem(int index)
    {
        _checkAquiredItem[index] = true;

        //buckshot�� �÷�ü �׼����� ����
        if (index == 10)
            _checkAquiredItem[9] = false;
        else if (index == 9)
            _checkAquiredItem[10] = false;
    }

    //���� ������ �ִ뷹�� �޼� �� ȹ�� �Ұ����ϰ� �ߺ� ���� False�� �ٲٱ�
    public void LockMaxLevelItem(int index)
    {
        Managers.Instance.DataManager.ItemList.ItemList[index].Reduplicatable = false;
    }

    //���� ������ �ִ뷹�� �޼��̿����� ������ ��� �ٽ� ��ʿ� �� �� �ְ� �ߺ� ���� True�� �ٲٱ�
    public void UnLockMaxLevelItem(int index)
    {
        Managers.Instance.DataManager.ItemList.ItemList[index].Reduplicatable = true;
    }

    //������ ��������
    private Item GetItem(int index)
    {
        Item item = new Item();

        item = Managers.Instance.DataManager.ItemList.ItemList[index];

        return item;
    }

    //��������� ���� Ȯ��
    private bool CheckRequireItem(Item item)
    {
        bool ispossible = false;

        if (_checkAquiredItem[item.ItemNumber - 1] == true)
            ispossible = true;

        return ispossible;
    }
}
