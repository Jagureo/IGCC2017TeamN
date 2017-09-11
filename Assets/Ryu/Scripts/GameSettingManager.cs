using UnityEngine;
using System.Collections;

public enum Gender {
  Male,
  Female,
}

public class GameSettingManager : MonoBehaviour {
  public static GameSettingManager Instance { get { return instance; } }
  private static GameSettingManager instance = null;

  public Gender Gender { get; set; }

  private void Awake() {
    if (!instance) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  private void Start() {
    Gender = Gender.Male;
  }

}
