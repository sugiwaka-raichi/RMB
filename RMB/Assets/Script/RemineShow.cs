using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemineShow : MonoBehaviour
{
    static Text remineText;

    static Text playersText;

    // Start is called before the first frame update
    void Start()
    {
        remineText = GameObject.Find("AliveRank").GetComponent<Text>();
        playersText = GameObject.Find("SetAliveRank").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetRemine(int _reminecount, int _playerscount)
    {
        remineText.text = _reminecount.ToString();
        playersText.text = _playerscount.ToString();
    }
}
