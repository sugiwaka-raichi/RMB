/*-------------------------------------------------
 * 名前: シーンマネージャ
 * 内容: 他スクリプトで使えるシーンマネージャ
 * 制作: 山本 正
 *-----------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class ManageSceneLoader : MonoBehaviour
{
    // 主要シーン列挙
    public enum SceneType
    {
        NetScene,       //ネットワークを保持するシーン 杉若
        TitleScene,     // タイトル
        LobbyScene,     // ロビー
        StageScene,     // ステージ
        ResultScene,    // リザルト
    }

    [SerializeField]
    SceneType startScene;           //開始シーンを設定

    static string nowScene = null;         //現在のシーン名
    static SceneType nowSceneType;

    // Start is called before the first frame update
    void Start()
    {
        // 現在アクティブになっているシーンの名前を取得
        var currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"アクティブシーン「{currentScene}」");

        //-------------------------------------------------------
        //杉若
        //開始時シーンを呼び出す
        SceneChange(startScene);
    }

    // Update is called once per frame
    void Update() { }

    // 他スクリプトへの反映
    //public static void SceneChange(SceneType _nextSceneName)
    //{
    //    SceneManager.LoadScene(_nextSceneName.ToString());
    //    Debug.Log($"{_nextSceneName}へ移動。");
    //}

    //========================================================
    // 杉若
    // ネットワークシーンを保持したまま別シーン呼び出し
    //========================================================
    public static void SceneChange(SceneType _nextSceneName)
    {
        if (nowScene != null)
        {
            UnLoadScene();
        }
        SceneManager.LoadScene(_nextSceneName.ToString(), LoadSceneMode.Additive);       //シーンを加算ロード
        Debug.Log($"{_nextSceneName}へ移動。");
        nowScene = _nextSceneName.ToString();       //ロードされているシーンを変更
        nowSceneType = _nextSceneName;              //シーンを設定
    }

    //=====================================================
    // 杉若
    // 現在シーンをアンロード
    //=====================================================
    private static void UnLoadScene()
    {
        Debug.Log($"{SceneManager.GetActiveScene().name}をアンロード。");
        SceneManager.UnloadSceneAsync(nowScene);
    }

    //=====================================================
    // 杉若
    // 現在のシーンを取得
    //=====================================================
    public static string GetActiveScene()
    {
        return nowScene;
    }

    //======================================================
    // 杉若
    // 現在のシーンを取得
    //======================================================
    public static SceneType GetSceneType()
    {
        return nowSceneType;
    }

    //======================================================
    // 杉若
    // シーンへオブジェクトを移動させる
    //======================================================
    public static void SceneMoveObject(GameObject _obj, SceneType _type)
    {
        Scene scene = SceneManager.GetSceneByName(_type.ToString());
        SceneManager.MoveGameObjectToScene(_obj, scene);
    }

    //==========================================================
    // 杉若
    // アクティブシーンを変更
    //==========================================================
    public static void SetActiveScene(SceneType _type)
    {
        Scene scene = SceneManager.GetSceneByName(_type.ToString());
        SceneManager.SetActiveScene(scene);
    }

    // 強制終了で使いたいときに
    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
    }
}

