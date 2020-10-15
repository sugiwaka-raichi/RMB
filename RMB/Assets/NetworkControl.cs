using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkControl : MonobitEngine.MonoBehaviour
{
    /** プレイヤー名. */
    private string playerName = "";

    /** ルーム名. */
    private string roomName = "";

    /** ルーム設定. */
    RoomSettings roomSettings = null;

    /** プレイヤーキャラクタ. */
    private GameObject playerObject = null;

    /** プレイヤーカスタムパラメータ. */
    Hashtable customParams = new Hashtable();

    /** 準備完了カウント. */
    int readyCount = 0;

    /** ゲーム中フラグ. */
    bool playingGame = false;

    /** ウィンドウ表示フラグ. */
    private bool bDisplayWindow = false;

    /** サーバ途中切断フラグ. */
    private static bool bDisconnect = false;

    /** サーバ接続失敗フラグ. */
    private bool bConnectFailed = false;

    /** ルーム数カウント変数. */
    int roomCount = 0;

    /** プレイヤー数カウント変数. */
    int playerCount = 0;

    /** UI用. */
    [SerializeField] Canvas BeforeConnectCanvas;
    [SerializeField] Canvas LobyCanvas;
    [SerializeField] Canvas RoomCanvas;
    [SerializeField] Canvas InGameCanvas;
    [SerializeField] Canvas ConnectedCanvas;
    [SerializeField] Canvas InRoomCanvas;

    [SerializeField] Text PlayerNameLavel;
    [SerializeField] InputField PlayerNameinputField;
    [SerializeField] Button ConnecoServerButton;

    [SerializeField] Text RoomnameLabe;
    [SerializeField] InputField RoomnameInputField;
    [SerializeField] Button CreateRandomRoomButton;
    [SerializeField] Button JoinRandomRoomButton;
    [SerializeField] Button[] JoinRoomButton = new Button[10];

    [SerializeField] Text RoomNameInfoLabel;
    [SerializeField] Text[] PlayerInfoLabel = new Text[10];
    [SerializeField] Button StartGameButton;

    [SerializeField] Button DisconnectButton;
    [SerializeField] Button LeaveRoomButton;

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
#if UNITY_5_3_OR_NEWER || UNITY_5_3
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#else
                Application.LoadLevel(Application.loadedLevelName);
#endif
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
        NetworkShow();
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

    /** ネットワークUI表示. */
    public void NetworkShow()
    {
        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                RoomShow();
            }
            // ルームに入室していない場合
            else
            {
                LobyShow();
            }
        }
        // MUNサーバに接続していない場合
        else
        {
            BeforeconnectServerShow();
        }
    }

    /** サーバ接続前UI表示. */
    public void BeforeconnectServerShow()
    {
        SetPlayerName((PlayerNameinputField.text == null) ? "" : PlayerNameinputField.text);
       
        customParams["ready"] = false;
        customParams["HP"] = 200;
        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(customParams);
    }

    /** ロビー内UI表示. */
    public void LobyShow()
    {
        // ルーム名の入力
        roomName = RoomnameInputField.text;

        RoomSettings roomSettings = new RoomSettings();
        roomSettings.maxPlayers = 10;
        roomSettings.isVisible = true;
        roomSettings.isOpen = true;

        for (int i = 0; i < roomCount; i++)
        {
            JoinRoomButton[i].gameObject.SetActive(false);
        }
        roomCount = 0;
        // ルーム一覧から選択式で入室する
        foreach (RoomData room in MonobitNetwork.GetRoomData())
        {
            string strRoomInfo =
                string.Format("{0}({1}/{2})",
                              room.name,
                              room.playerCount,
                              (room.maxPlayers == 0 ? "-" : room.maxPlayers.ToString()));
            
            JoinRoomButton[roomCount].gameObject.SetActive(true);
            JoinRoomButton[roomCount].GetComponentInChildren<Text>().text = "Enter Room" + strRoomInfo;
            roomCount++;
        }
    }

    /** ルーム内UI表示. */
    public void RoomShow()
    {
        if (roomName == "")
        {
            roomName = MonobitNetwork.room.name;
        }

        RoomNameInfoLabel.text = roomName;
        readyCount = 0;
        for(int i = 0; i < playerCount; i++)
        {
            PlayerInfoLabel[i].gameObject.SetActive(false);
        }
        playerCount = 0;
        Debug.Log(MonobitNetwork.player.customParameters["ready"]);
        foreach (MonobitPlayer player in MonobitNetwork.playerList)
        {
            string playerInfo =
                string.Format("{0} {1} {2}",
                    player.ID, player.name, player.customParameters["ready"]);
            PlayerInfoLabel[playerCount].gameObject.SetActive(true);
            PlayerInfoLabel[playerCount].text = playerInfo;
            Debug.Log(player.customParameters["ready"]);
            if ((bool)player.customParameters["ready"])
            {
                Debug.Log(readyCount + " " + MonobitNetwork.playerList.Length);
                readyCount++;
                if (readyCount == MonobitNetwork.playerList.Length)
                {
                    StartGame();
                }
            }

            Debug.Log("PlayerName : " + player.name + ", Ready : " + player.customParameters["ready"]);
            playerCount++;
        }

        if (MonobitNetwork.playerList.Length >= 1)
        {
            StartGameButton.gameObject.SetActive(true);
        }
    }

    /** ボタンテンプレート. */
    bool OnGUIButton(string _buttonname, int _buttonwidth)
    {
        return GUILayout.Button(_buttonname, GUILayout.Width(_buttonwidth));
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

    /** サーバ接続. */
    public void Connectserver()
    {
        ConnectServer("MonsterBattle_v1.0");

        BeforeConnectCanvas.gameObject.SetActive(false);
        LobyCanvas.gameObject.SetActive(true);
        ConnectedCanvas.gameObject.SetActive(true);
    }

    /** サーバ接続呼出し用. */
    public static void ConnectServer(string _versionname)
    {
        // デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;

        // MUNサーバに接続する
        MonobitNetwork.ConnectServer(_versionname);
    }

    /** サーバ切断. */
    public void DisconnectServer()
    {
        // 正常動作のため、bDisconnect を true にして、GUIウィンドウ表示をキャンセルする
        bDisconnect = true;

        // サーバから切断する
        MonobitNetwork.DisconnectServer();

        // シーンをリロードする
#if UNITY_5_3_OR_NEWER || UNITY_5_3
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#else
                Application.LoadLevel(Application.loadedLevelName);
#endif
 
        LobyCanvas.gameObject.SetActive(false);
        RoomCanvas.gameObject.SetActive(false);
        InGameCanvas.gameObject.SetActive(false);
        ConnectedCanvas.gameObject.SetActive(false);
        BeforeConnectCanvas.gameObject.SetActive(true);
    }

    /** ルーム作成汎用. */
    public void CreateRoom()
    {
        if(roomName != "")
        {
            if(roomSettings != null)
            {
                CreateRoom(roomName, roomSettings, null);
            }
            else
            {
                CreateRoom(roomName);
            }

            LobyCanvas.gameObject.SetActive(false);
            RoomCanvas.gameObject.SetActive(true);
            InRoomCanvas.gameObject.SetActive(true);
        }
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

    /** 入室ボタン用. */
    public void JoinRoom(int roomnum)
    {
        roomName = MonobitNetwork.GetRoomData()[roomnum].name;
        JoinRoom(roomName);
        LobyCanvas.gameObject.SetActive(false);
        RoomCanvas.gameObject.SetActive(true);
        InRoomCanvas.gameObject.SetActive(true);
    }

    /** ランダム入室ボタン用. */
    public void JoinRandomRoomB()
    {
        JoinRandomRoom();
        
        if (roomCount > 0)
        {
            LobyCanvas.gameObject.SetActive(false);
            RoomCanvas.gameObject.SetActive(true);
            InRoomCanvas.gameObject.SetActive(true);
        }

    }

    /** ランダム入室呼び出し用. */
    public static void JoinRandomRoom()
    {
        MonobitNetwork.JoinRandomRoom();
    }

    /** ルーム退室ボタン用. */
    public void LeaveRoomB()
    {
        customParams["ready"] = false;
        customParams["HP"] = 200;
        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(customParams);
        LeaveRoom();
        RoomCanvas.gameObject.SetActive(false);
        InGameCanvas.gameObject.SetActive(false);
        InRoomCanvas.gameObject.SetActive(false);
        LobyCanvas.gameObject.SetActive(true);
    }

    /** ルーム退室呼び出し用. */
    public static void LeaveRoom()
    {
        MonobitNetwork.LeaveRoom();
    }

    /** 準備完了. */
    public void Ready()
    {
        customParams["ready"] = true;
        customParams["HP"] = 200;
        MonobitEngine.MonobitNetwork.SetPlayerCustomParameters(customParams);
    }

    /** ゲーム開始. */
    public void StartGame()
    {
        RoomCanvas.gameObject.SetActive(false);
        InGameCanvas.gameObject.SetActive(true);

        // プレイヤーキャラクタが未搭乗の場合に登場させる
        if (playerObject == null)
        {
            Vector3 playerpos = Vector3.zero;
            switch (MonobitNetwork.player.ID)
            {
                case 1:
                    playerpos = new Vector3(-10.0f, 1.0f, 10.0f);
                    break;
                case 2:
                    playerpos = new Vector3(10.0f, 1.0f, -10.0f);
                    break;
                case 3:
                    playerpos = new Vector3(10.0f, 1.0f, 10.0f);
                    break;
                case 4:
                    playerpos = new Vector3(-10.0f, 1.0f, -10.0f);
                    break;
                case 5:
                    playerpos = new Vector3(-5.0f, 1.0f, 5.0f);
                    break;
                case 6:
                    playerpos = new Vector3(5.0f, 1.0f, -5.0f);
                    break;
                case 7:
                    playerpos = new Vector3(5.0f, 1.0f, 5.0f);
                    break;
                case 8:
                    playerpos = new Vector3(5.0f, 1.0f, -5.0f);
                    break;
                case 9:
                    playerpos = new Vector3(-2.0f, 1.0f, 0.0f);
                    break;
                case 10:
                    playerpos = new Vector3(2.0f, 1.0f, 0.0f);
                    break;
            }

            playerObject = MonobitNetwork.Instantiate(
                            "Player",
                            playerpos,
                            Quaternion.identity,
                            0);
        }

        GameObject spawnerobj = GameObject.Find("MonsterSpawner");
        //Spawner spawnerscript = spawnerobj.GetComponent<Spawner>();
        //spawnerscript.BeginSpawning();
    }

    /** ゲーム終了. */
    public void EndGame()
    {
        Destroy(playerObject);

        GameObject spawnerobj = GameObject.Find("MonsterSpawner");
        //Spawner spawnerscript = spawnerobj.GetComponent<Spawner>();
        //spawnerscript.EndSpawning();
    }

}