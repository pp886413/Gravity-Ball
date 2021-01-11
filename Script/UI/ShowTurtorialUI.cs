using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTurtorialUI : MonoBehaviour
{
    public GameObject TurtorialUI;
    public GameObject Canvas;

    public static int Index = 0;
    public int MaxIndex = 0;

    private string[] TurtorialText = { "按下 <sprite=3>移動", "按下<sprite=4>跳躍", "按下 <sprite=2> 拖曳物品"};
    
    private void Awake()
    {
        MaxIndex = TurtorialText.Length;

        InvokeRepeating("ShowNextTurtorialUI", 1.5f, 6.0f);
    }
    private void ShowNextTurtorialUI()
    {
        GameObject ExTurtorialUI = Instantiate(TurtorialUI);
        ExTurtorialUI.transform.SetParent(Canvas.transform);

        ExTurtorialUI.GetComponentInChildren<TextMeshProUGUI>().text = TurtorialText[Index];
        Index += 1;
        
        if (Index == MaxIndex)
        {
            CancelInvoke("ShowNextTurtorialUI");
            Index = 0;
            Destroy(this.gameObject);
        }
    }
}
