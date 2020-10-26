/*-------------------------------------------------
 * 名前: ボタンメソッド
 * 内容: ボタンで使うであろう機能の集約スクリプト
 * 制作: 山本 正
 *-----------------------------------------------*/ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class ButtonFunc : MonoBehaviour
{
    [SerializeField]
    GameObject fadeOut;

    /*シーン遷移*/
    // タイトル
    public void GoTitle()
    {
        fadeOut.SetActive(true);
        FadeOutScript.SetNextScene(ManageSceneLoader.SceneType.TitleScene);
        //ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.TitleScene);
    }
    // ロビー
    public void GoLobby()
    {
        if (NetworkManager.GetinRoom())
        {
            NetworkManager.LeaveRoom();
        }
        
        fadeOut.SetActive(true);
        FadeOutScript.SetNextScene(ManageSceneLoader.SceneType.LobbyScene);
        //ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.LobbyScene);
    }
    // ステージ（ゲーム本編）
    public void GoStage()
    {

        fadeOut.SetActive(true);
        FadeOutScript.SetNextScene(ManageSceneLoader.SceneType.StageScene);
        //ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.StageScene);
    }
    
    // ゲーム終了（基本的にタイトルに存在するQUITボタンに配置）
    public void OnGameQuit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
                        UnityEngine.Application.Quit();
        #endif
    }
}
