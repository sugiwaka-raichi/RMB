using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorButton : MonoBehaviour
{
    [SerializeField]
    GameObject ErrorCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoTitle()
    {
        SoundManager.PlaySE(SoundData.SE_LIST.SystemDecision.ToString());       //システム決定流す
        ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.TitleScene);
        ErrorCanvas.gameObject.SetActive(false);
        if (ManageSceneLoader.GetSceneType() == ManageSceneLoader.SceneType.StageScene)
        {
            if (!NetworkManager.GetPlayerDeathflg())
            {
                GManager.GMInstance.SendPlayerDeath();
            }
        }
        NetworkManager.PlayerDeathflgOff();
    }
}
