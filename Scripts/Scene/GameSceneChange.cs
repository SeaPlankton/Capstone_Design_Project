using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneChange : MonoBehaviour
{
    public void OnClickRestartBtn()
    {
        Managers.Instance.Game.RestartGame();
    }
    public void OnClickTitleBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
