using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenuView : MonoBehaviour
{
    /// <summary>
    /// ����ŸƮ ��ư Ŭ���� ���� �����
    /// </summary>
    public void OnClickRestartButton()
    {
        Managers.Instance.Game.RestartGame();
    }
}
