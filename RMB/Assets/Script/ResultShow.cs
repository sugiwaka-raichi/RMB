using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultShow : MonoBehaviour
{
    static GameObject ResultCanvas;

    static Text ResultText;

    // Start is called before the first frame update
    void Start()
    {
        ResultCanvas = this.gameObject;
        ResultText = GetComponentInChildren<Text>();
        ResultCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ResultActive(int playerlank, int players)
    {
        ResultCanvas.gameObject.SetActive(true);

        ResultText.text = string.Format("{0}     {1}", players, playerlank);
    }
}
