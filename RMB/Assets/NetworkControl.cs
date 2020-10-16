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

    /** ルーム数カウント変数. */
    int roomCount = 0;

    /** プレイヤー数カウント変数. */
    int playerCount = 0;

    /** UI使うかフラグ. */
    [SerializeField] bool useUIflg = false;

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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (useUIflg)
        {
            NetworkShow();
        }
    }

    // OnGUI is called for rendering and handring GUI events
    void OnGUI()
    {
        if (!useUIflg)
        {
            NetworkShowOnGUI();
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

    void NetworkShowOnGUI()
    {
        // デフォルトのボタンと被らないように、段下げを行う。
        GUILayout.Space(24);

        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ボタン入力でサーバから切断＆シーンリセット
            if (GUILayout.Button("Disconnect", GUILayout.Width(150)))
            {
                // 正常動作のため、bDisconnect を true にして、GUIウィンドウ表示をキャンセルする
                NetworkManager.DisconnectflgOn();

                // サーバから切断する
                DisconnectServer();

                // シーンをリロードする
                ManageSceneLoader.SceneType sceneName = ManageSceneLoader.GetSceneType();
                ManageSceneLoader.SceneChange(sceneName);
                Debug.Log(sceneName);
            }

            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {

                // ボタン入力でルームから退室
                if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
                {
                    customParams["ready"] = false;
                    customParams["HP"] = 200;
                    playingGame = false;
                    NetworkManager.SetPlayerCustomParameters(customParams);
                    NetworkManager.LeaveRoom();
                }

                if (!playingGame)
                {
                    GUILayout.Label(MonobitNetwork.room.name);
                    readyCount = 0;
                    foreach (MonobitPlayer player in MonobitNetwork.playerList)
                    {
                        string playerInfo =
                            string.Format("{0} {1} {2}",
                                player.ID, player.name, player.customParameters["ready"]);
                        GUILayout.Label(playerInfo);

                        if ((bool)player.customParameters["ready"])
                        {
                            readyCount++;

                            if (readyCount == MonobitNetwork.playerList.Length)
                            {
                                StartGame();
                                customParams["ready"] = false;
                                NetworkManager.SetPlayerCustomParameters(customParams);
                            }
                        }

                        Debug.Log("PlayerName : " + player.name + ", Ready : " + player.customParameters["ready"]);
                    }

                    if (MonobitNetwork.playerList.Length >= 1)
                    {
                        if (GUILayout.Button("StartGame"))
                        {
                            customParams["ready"] = true;
                            customParams["HP"] = 200;
                            NetworkManager.SetPlayerCustomParameters(customParams);
                        }
                    }
                }
            }

            // ルームに入室していない場合
            if (!MonobitNetwork.inRoom)
            {
                GUILayout.BeginHorizontal();

                // ルーム名の入力
                GUILayout.Label("RoomName : ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));

                RoomSettings roomSettings = new RoomSettings();
                roomSettings.maxPlayers = 10;
                roomSettings.isVisible = true;
                roomSettings.isOpen = true;

                // ボタン入力でルーム作成
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    NetworkManager.CreateRoom(roomName, roomSettings, null);
                }

                GUILayout.EndHorizontal();

                // 現在存在するルームからランダムに入室する
                if (GUILayout.Button("Join Random Room", GUILayout.Width(200)))
                {
                    NetworkManager.JoinRandomRoom();
                }

                // ルーム一覧から選択式で入室する
                foreach (RoomData room in MonobitNetwork.GetRoomData())
                {
                    string strRoomInfo =
                        string.Format("{0}({1}/{2})",
                                      room.name,
                                      room.playerCount,
                                      (room.maxPlayers == 0 ? "-" : room.maxPlayers.ToString()));

                    if (GUILayout.Button("Enter Room : " + strRoomInfo))
                    {
                        NetworkManager.JoinRoom(room.name);
                    }
                }
            }
        }
        else
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("PlayerName :");
            NetworkManager.SetPlayerName(GUILayout.TextField((MonobitNetwork.playerName == null) ? "" : MonobitNetwork.playerName,
                                               GUILayout.Width(200)));
            GUILayout.EndHorizontal();
            customParams["ready"] = false;
            customParams["HP"] = 200;
            NetworkManager.SetPlayerCustomParameters(customParams);

            if (GUILayout.Button("Connect Server", GUILayout.Width(200)))
            {
                NetworkManager.ConnectServer("MonsterBattle_v1.0");
            }
        }
    }

    /** サーバ接続前UI表示. */
    public void BeforeconnectServerShow()
    {
        NetworkManager.SetPlayerName((PlayerNameinputField.text == null) ? "" : PlayerNameinputField.text);
       
        customParams["ready"] = false;
        customParams["HP"] = 200;
        NetworkManager.SetPlayerCustomParameters(customParams);
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

    /** サーバ接続. */
    public void Connectserver()
    {
        NetworkManager.ConnectServer("MonsterBattle_v1.0");

        BeforeConnectCanvas.gameObject.SetActive(false);
        LobyCanvas.gameObject.SetActive(true);
        ConnectedCanvas.gameObject.SetActive(true);
    }


    /** サーバ切断. */
    public void DisconnectServer()
    {
        // 正常動作のため、bDisconnect を true にして、GUIウィンドウ表示をキャンセルする
        NetworkManager.DisconnectflgOn();

        // サーバから切断する
        NetworkManager.DisconnectServer();

        // シーンをリロードする
        ManageSceneLoader.SceneType sceneName = ManageSceneLoader.GetSceneType();
        ManageSceneLoader.SceneChange(sceneName);
        Debug.Log(sceneName);

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
                NetworkManager.CreateRoom(roomName, roomSettings, null);
            }
            else
            {
                NetworkManager.CreateRoom(roomName);
            }

            LobyCanvas.gameObject.SetActive(false);
            RoomCanvas.gameObject.SetActive(true);
            InRoomCanvas.gameObject.SetActive(true);
        }
    }

    /** 入室ボタン用. */
    public void JoinRoom(int roomnum)
    {
        roomName = MonobitNetwork.GetRoomData()[roomnum].name;
        NetworkManager.JoinRoom(roomName);
        LobyCanvas.gameObject.SetActive(false);
        RoomCanvas.gameObject.SetActive(true);
        InRoomCanvas.gameObject.SetActive(true);
    }

    /** ランダム入室ボタン用. */
    public void JoinRandomRoomB()
    {
        NetworkManager.JoinRandomRoom();
        
        if (roomCount > 0)
        {
            LobyCanvas.gameObject.SetActive(false);
            RoomCanvas.gameObject.SetActive(true);
            InRoomCanvas.gameObject.SetActive(true);
        }

    }

    /** ルーム退室ボタン用. */
    public void LeaveRoomB()
    {
        customParams["ready"] = false;
        customParams["HP"] = 200;
        NetworkManager.SetPlayerCustomParameters(customParams);
        NetworkManager.LeaveRoom();
        RoomCanvas.gameObject.SetActive(false);
        InGameCanvas.gameObject.SetActive(false);
        InRoomCanvas.gameObject.SetActive(false);
        LobyCanvas.gameObject.SetActive(true);
    }

    /** 準備完了. */
    public void Ready()
    {
        customParams["ready"] = true;
        customParams["HP"] = 200;
        NetworkManager.SetPlayerCustomParameters(customParams);
    }
    
    /** ゲーム開始. */
    public void StartGame()
    {
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.StageScene);
        Debug.Log("change getter");
        playingGame = true;
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