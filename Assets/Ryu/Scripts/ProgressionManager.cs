using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class ChangeProgressionEvent : UnityEvent<string> { }

public class ProgressionManager : MonoBehaviour {
  public ChangeProgressionEvent onProgressionChanged = new ChangeProgressionEvent();
  private string currentProgression = "PlayerAwakening";

  public static ProgressionManager Instance { get { return instance; } }
  private static ProgressionManager instance = null;

  private void Awake() {
    if (!instance) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  public string CurrentProgression {
    get {
      return currentProgression;
    }
  }

  public void ChangeProgression(string progression) {
    currentProgression = progression;
    onProgressionChanged.Invoke(currentProgression);
  }

}
