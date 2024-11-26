using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenuView : MonoBehaviour
{
    /// <summary>
    /// 리스타트 버튼 클릭시 게임 재시작
    /// </summary>
    public void OnClickRestartButton()
    {
        Managers.Instance.Game.RestartGame();
    }
}
