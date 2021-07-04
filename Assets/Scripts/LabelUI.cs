using UnityEngine;
using UnityEngine.UI;

//Debug UI, to be removed 
public class LabelUI
{
    private Text aDistanceTxt;
    private Text bDistanceTxt;

    //Setup Canvas
    public LabelUI() 
    {
        GameObject uibase = new GameObject();
        uibase.name = "UICanvas";
        Canvas uiCanvas = uibase.AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uibase.AddComponent<CanvasScaler>();
        uibase.AddComponent<GraphicRaycaster>();
        GameObject aTxt = new GameObject();
        aTxt.transform.parent = uibase.transform;
        aTxt.name = "LabelADistance";
        GameObject bTxt = new GameObject();
        bTxt.transform.parent = uibase.transform;
        bTxt.name = "LabelBDistance";
        CreateText(aTxt, bTxt);
    }
    
    //Creates the two updating text
    private void CreateText(GameObject aTxt, GameObject bTxt) 
    {
        aDistanceTxt = aTxt.AddComponent<Text>();
        aDistanceTxt.text = "A: ";
        aDistanceTxt.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        aDistanceTxt.fontSize = 64;
        aDistanceTxt.alignment = TextAnchor.UpperLeft;
        RectTransform aRectTransform = aTxt.GetComponent<RectTransform>();
        aRectTransform.anchoredPosition = new Vector3(550, -100, 0);
        aRectTransform.anchorMax = Vector2.up;
        aRectTransform.anchorMin = Vector2.zero;
        aRectTransform.sizeDelta = new Vector2(1000, 100);
        bDistanceTxt = bTxt.AddComponent<Text>();
        bDistanceTxt.text = "B: ";
        bDistanceTxt.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        bDistanceTxt.fontSize = 64;
        bDistanceTxt.alignment = TextAnchor.UpperLeft;
        RectTransform bRectTransform = bTxt.GetComponent<RectTransform>();
        bRectTransform.anchoredPosition = new Vector3(550, -200, 0);
        bRectTransform.anchorMax = Vector2.up;
        bRectTransform.anchorMin = Vector2.zero;
        bRectTransform.sizeDelta = new Vector2(1000, 100);
    }

    public void UpdateUI(float distA, float distB)
    {
        aDistanceTxt.text = "A: " + distA/100 + " cm";
        bDistanceTxt.text = "B: " + distB/100 + " cm";
    }
}