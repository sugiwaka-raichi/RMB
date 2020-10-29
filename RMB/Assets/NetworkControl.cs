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
    public static  string roomName = "";

    /** ルーム設定. */
    public static RoomSettings roomSettings = null;

    /** ルームカスタムパラメータ. */
    Hashtable roomParams = new Hashtable();

    /** プレイヤーキャラクタ. */
    private GameObject playerObject = null;

    /** プレイヤーカスタムパラメータ. */
    public static Hashtable customParams = new Hashtable();

    /** 準備完了カウント. */
    int readyCount = 0;

    /** ゲーム中フラグ. */
    bool playingGame = false;

    /** ルーム数カウント変数. */
    public static int roomCount = 0;

    /** プレイヤー数カウント変数. */
    int playerCount = 0;

    public static bool lobyon = true; 
    /** UI使うかフラグ. */
    [SerializeField] bool useUIflg = false;

    /** UI用. */
    [SerializeField] Canvas RoomCanvas;
    [SerializeField] Text RoomNameInfoLabel;
    [SerializeField] Text PlayerInfoItem;
    [SerializeField] Text[] PlayerInfoLabel = new Text[8];
    [SerializeField] Button ReadyGameButton;

    [SerializeField] Button DisconnectButton;
    [SerializeField] Button LeaveRoomButton;
    [SerializeField] Button MoveRoomButton;

    [SerializeField]
    GameObject fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkManager.GetisConnect())
        {
            customParams["ready"] = false;
            NetworkManager.SetPlayerCustomParameters(customParams);
            NetworkManager.ConnectServer("MonsterBattle_v1.0");
        }
        roomCount = 0;
        lobyon = true;
    }

    // Update is called once per frame
    void Update()
    {
        NetworkShow();
    }

    /** ネットワークUI表示. */
    public void NetworkShow()
    {
        // MUNサーバに接続している場合
        if (NetworkManager.GetisConnect())
        {
            // ルームに入室している場合
            if (NetworkManager.GetinRoom())
            {
                RoomNameInfoLabel.gameObject.SetActive(true);
                PlayerInfoItem.gameObject.SetActive(true);
                RoomShow();
            }
            // ルームに入室していない場合
            else if (lobyon)
            {
                RoomNameInfoLabel.gameObject.SetActive(false);
                PlayerInfoItem.gameObject.SetActive(false);
                ReadyGameButton.gameObject.SetActive(false);
                for (int i = 0; i < playerCount; i++)
                {
                    PlayerInfoLabel[i].gameObject.SetActive(false);
                }
                LobyShow();
            }
        }
    }

    /** ロビー内UI表示. */
    public void LobyShow()
    {
        lobyon = false;
        RoomSettings roomSettings = new RoomSettings();
        roomSettings.maxPlayers = 8;
        roomSettings.isVisible = true;
        roomSettings.isOpen = true;

        roomCount++;
        roomName = "room";
        NetworkManager.JoinOrCreateRoom(roomName + roomCount.ToString(), roomSettings, null);
    }

    /** ルーム内UI表示. */
    public void RoomShow()
    {
        if (roomName == "")
        {
            roomName = NetworkManager.GetRoom().name;
        }

        RoomNameInfoLabel.text = NetworkManager.GetRoom().name;
        readyCount = 0;
        for(int i = 0; i < playerCount; i++)
        {
            PlayerInfoLabel[i].gameObject.SetActive(false);
        }
        playerCount = 0;
        foreach (MonobitPlayer player in NetworkManager.GetPlayerList())
        {
            string playerInfo =
                string.Format("    {0}                {1}",
                    player.ID, player.customParameters["ready"]);
            PlayerInfoLabel[playerCount].gameObject.SetActive(true);
            PlayerInfoLabel[playerCount].text = playerInfo;
            if ((bool)player.customParameters["ready"])
            {
                readyCount++;
                if (readyCount == NetworkManager.GetPlayerList().Length)
                {
                    StartGame();
                    customParams["ready"] = false;
                    customParams["HP"] = 100;
                    NetworkManager.SetPlayerCustomParameters(customParams);
                    NetworkManager.GetRoom().open = false;
                    NetworkManager.GetRoom().visible = false;
                }
            }
            playerCount++;
        }

        if (NetworkManager.GetPlayerList().Length >= 1)
        {
            ReadyGameButton.gameObject.SetActive(true);
        }
    }

    /** ボタンテンプレート. */
    bool OnGUIButton(string _buttonname, int _buttonwidth)
    {
        return GUILayout.Button(_buttonname, GUILayout.Width(_buttonwidth));
    }

    /** サーバ切断. */
    public void DisconnectServer()
    {
        SoundManager.PlaySE(SoundData.SE_LIST.SystemDecision.ToString());       //システム決定流す

        // 正常動作のため、bDisconnect を true にして、GUIウィンドウ表示をキャンセルする
        NetworkManager.DisconnectflgOn();

        // サーバから切断する
        NetworkManager.DisconnectServer();

        // シーンをリロードする
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.LobbyScene);
    }

    /** ルーム退室ボタン用. */
    public void LeaveRoomB()
    {
        SoundManager.PlaySE(SoundData.SE_LIST.SystemDecision.ToString());       //システム決定流す
        NetworkManager.LeaveRoom();
    }

    /** 準備完了. */
    public void Ready()
    {
        SoundManager.PlaySE(SoundData.SE_LIST.SystemDecision.ToString());       //システム決定流す
        customParams["ready"] = true;
        NetworkManager.SetPlayerCustomParameters(customParams);
    }

    public void MoveRoom()
    {
        SoundManager.PlaySE(SoundData.SE_LIST.SystemDecision.ToString());       //システム決定流す
        NetworkManager.MoveRoom();
    }
    
    /** ゲーム開始. */
    public void StartGame()
    {
        //ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.StageScene);
        fadeOut.SetActive(true);
        FadeOutScript.SetNextScene(ManageSceneLoader.SceneType.StageScene);
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