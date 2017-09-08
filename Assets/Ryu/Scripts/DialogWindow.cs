﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour {
  private ScrollRect scrollRect;

  [SerializeField]
  private GameObject dialogNode;

  private void Awake() {
    scrollRect = GetComponent<ScrollRect>();
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
