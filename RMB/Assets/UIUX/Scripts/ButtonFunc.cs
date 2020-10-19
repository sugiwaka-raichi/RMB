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
    /*シーン遷移*/
    // タイトル
    public void GoTitle()
    {
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.TitleScene);
    }
    // ロビー
    public void GoLobby()
    {
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.LobbyScene);
    }
    // ステージ（ゲーム本編）
    public void GoStage()
    {
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.StageScene);
    }
    // リザルト
    public void GoResult()
    {
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.ResultScene);
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
