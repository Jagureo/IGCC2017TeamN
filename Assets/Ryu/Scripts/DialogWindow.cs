using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour {
  private ScrollRect scrollRect;
  private Image image;

  [SerializeField]
  private GameObject dialogNode;
  [SerializeField]
  private Sprite brokenSprite;

  private void Awake() {
    scrollRect = GetComponent<ScrollRect>();
    image = GetComponent<Image>();
  }

  public void BreakWindow(string str) {
    if (str != "DoorOpens") return;
    image.sprite = brokenSprite;
  }

  public void AddDialog(string str) {
    var node = Instantiate(dialogNode);
    var translator = node.GetComponentInChildren<TextTranslator>();
    var nodeTransform = node.GetComponent<RectTransform>();

    translator.Key = str;
    nodeTransform.SetParent(scrollRect.content.transform, false);
    Canvas.ForceUpdateCanvases();
    scrollRect.verticalNormalizedPosition = 0.0f;
    Canvas.ForceUpdateCanvases();
  }
}
