using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.UI;

public class NetworkManager : MonobitEngine.MonoBehaviour
{

    /** ウィンドウ表示フラグ. */
    private bool bDisplayWindow = false;

    /** サーバ途中切断フラグ. */
    private static bool bDisconnect = false;

    /** サーバ接続失敗フラグ. */
    private bool bConnectFailed = false;

    /**
     * 途中切断コールバック.
     */
    public void OnDisconnectedFromServer()
    {
        Debug.Log("OnDisconnectedFromServer");
        if (bDisconnect == false)
        {
            bDisconnect = true;
            bDisplayWindow = true;
        }
        else
        {
            bDisconnect = false;
        }
    }

    /**
     * サーバ接続失敗コールバック.
     */
    public void OnConnectToServerFailed(object parameters)
    {
        Debug.Log("OnConnectToServerFailed : StatusCode = " + parameters);
        bConnectFailed = true;
        bDisplayWindow = true;
    }

    /**
     * ウィンドウ表示用メソッド.
     */
    void WindowControl(int windowId)
    {
        // GUIスタイル設定
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUIStyleState stylestate = new GUIStyleState();
        stylestate.textColor = Color.white;
        style.normal = stylestate;

        // 途中切断時の表示
        if (bDisconnect)
        {
            GUILayout.Label("途中切断しました。\n再接続しますか？", style);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("はい", GUILayout.Width(50)))
            {
                // もう一度接続処理を実行する
                MonobitNetwork.ConnectServer("MonsterBattle_v1.0");
                bDisconnect = false;
                bDisplayWindow = false;
            }
            if (GUILayout.Button("いいえ", GUILayout.Width(50)))
            {
                // シーンをリロードし、初期化する
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        // 接続失敗時の表示
        if (bConnectFailed)
        {
            GUILayout.Label("接続に失敗しました。\n再接続しますか？", style);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("はい", GUILayout.Width(50)))
            {
                // もう一度接続処理を実行する
                MonobitNetwork.ConnectServer("MonsterBattle_v1.0");
                bConnectFailed = false;
                bDisplayWindow = false;
            }
            if (GUILayout.Button("いいえ", GUILayout.Width(50)))
            {
                // オフラインモードで起動する
                MonobitNetwork.offline = true;
                bConnectFailed = false;
                bDisplayWindow = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnGUI is called for rendering and handring GUI events
    void OnGUI()
    {
        // サーバ接続状況に応じて、ウィンドウを表示する
        if (bDisplayWindow)
        {
            GUILayout.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 40, 200, 80), WindowControl, "Caution");
        }

    }

    /** ホストかどうか取得. */
    public static bool GetisHost()
    {
        return MonobitNetwork.isHost;
    }

    /** プレイヤー名入力. */
    public static void SetPlayerName(string _playername)
    {
        MonobitNetwork.playerName = _playername;
    }

    /** サーバ接続呼出し用. */
    public static void ConnectServer(string _versionname)
    {
        // デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;

        // MUNサーバに接続する
        MonobitNetwork.ConnectServer(_versionname);
    }

    /** サーバ切断呼出し用. */
    public static void DisconnectServer()
    {
        MonobitNetwork.DisconnectServer();
    }

    /** ルーム作成名前入力. */
    public static void CreateRoom(string _roomname)
    {
        MonobitNetwork.CreateRoom(_roomname);
    }

    /** ルーム作成設定入力. */
    public static void CreateRoom(string _roomname, RoomSettings _roomSettings, LobbyInfo _lobbyInfo)
    {
        MonobitNetwork.CreateRoom(_roomname, _roomSettings, _lobbyInfo);
    }

    /** 入室呼び出し用. */
    public static void JoinRoom(string _roomname)
    {
        MonobitNetwork.JoinRoom(_roomname);
    }

    /** ランダム入室呼び出し用. */
    public static void JoinRandomRoom()
    {
        MonobitNetwork.JoinRandomRoom();
    }

    /** ルーム退室呼び出し用. */
    public static void LeaveRoom()
    {
        MonobitNetwork.LeaveRoom();
    }
}
