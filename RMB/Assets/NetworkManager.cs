using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.UI;
using UnityEditor;

public class NetworkManager : MonobitEngine.MonoBehaviour
{
    /** ウィンドウ表示フラグ. */
    private static bool bDisplayWindow = false;

    /** サーバ途中切断フラグ. */
    private static bool bDisconnect = false;

    /** サーバ接続失敗フラグ. */
    private static bool bConnectFailed = false;

    /** ルーム作成失敗フラグ. */
    private static bool bCreateRoomFailed = false;

    /** ルーム入室失敗フラグ. */
    private static bool bJoinRoomFailed = false;

    /** ルームランダム入室失敗フラグ. */
    private static bool bRandomJoinRoomFailed = false;

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
    * ルーム参加失敗コールバック.
    */
    public void OnCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("OnCreateRoomFailed : errorCode = " + codeAndMsg[0] + ", message = " + codeAndMsg[1]);
        bCreateRoomFailed = true;
        bDisplayWindow = true;
    }
    
    /**
    * ルーム入室失敗コールバック.
    */
    public void OnJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("OnJoinRoomFailed : errorCode = " + codeAndMsg[0] + ", message = " + codeAndMsg[1]);
        bJoinRoomFailed = true;
        bDisplayWindow = true;
    }

    /**
    * ルームランダム入室失敗コールバック.
    */
    public void OnMonobitRandomJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("OnMonobitRandomJoinFailed : errorCode = " + codeAndMsg[0] + ", message = " + codeAndMsg[1]);
        bRandomJoinRoomFailed = true;
        bDisplayWindow = true;
    }

    /** 手動切断. */
    public static void DisconnectflgOn()
    {
        bDisconnect = true;
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
                // シーンをリロードする
                ManageSceneLoader.SceneType sceneName = ManageSceneLoader.GetSceneType();
                ManageSceneLoader.SceneChange(sceneName);
                Debug.Log(sceneName);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        // ルーム作成失敗時の表示
        if (bCreateRoomFailed)
        {
            GUILayout.Label("ルーム作成に失敗しました。", style);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(50)))
            {
                bCreateRoomFailed = false;
                bDisplayWindow = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        // ルーム入室失敗時の表示
        if (bJoinRoomFailed)
        {
            GUILayout.Label("入室に失敗しました。", style);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(50)))
            {
                bJoinRoomFailed = false;
                bDisplayWindow = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        // ルームランダム入室失敗時の表示
        if (bRandomJoinRoomFailed)
        {
            GUILayout.Label("ランダム入室に失敗しました。", style);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(50)))
            {
                bRandomJoinRoomFailed = false;
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

    public static bool GetisConnect()
    {
        return MonobitNetwork.isConnect;
    }

    public static bool GetinRoom()
    {
        return MonobitNetwork.inRoom;
    }

    public static Room GetRoom()
    {
        return MonobitNetwork.room;
    }

    public static RoomData[] GetRoomData()
    {
        return MonobitNetwork.GetRoomData();
    }

    public static MonobitPlayer[] GetPlayerList()
    {
        return MonobitNetwork.playerList;
    }
    public static MonobitPlayer GetPlayer()
    {
        return MonobitNetwork.player;
    }

    public static string GetPlayerName()
    {
        return MonobitNetwork.playerName;
    }

    /** プレイヤーカスタムパラメータ設定. */
    public static void SetPlayerCustomParameters(Hashtable _customParams)
    {
        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(_customParams);
    }

    /** ルームカスタムパラメータ設定. */
    public static void SetRoomParameters(Hashtable _roomParams)
    {
        MonobitEngine.MonobitNetwork.room.SetCustomParameters(_roomParams);
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

    /** ランダム入室呼び出し用. */
    public static void JoinRandomRoom(Hashtable _roomParams)
    {
        MonobitNetwork.JoinRandomRoom(_roomParams, 0);
    }

    /** ルーム退室呼び出し用. */
    public static void LeaveRoom()
    {
        MonobitNetwork.LeaveRoom();
    }
}
