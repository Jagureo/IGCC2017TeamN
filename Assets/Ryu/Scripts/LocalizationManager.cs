using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class LocalizationManager : MonoBehaviour {
  public delegate void LanguageChangeEvent();
  public event LanguageChangeEvent onLanguageChanged = delegate { };

  public static LocalizationManager Instance { get { return instance; } }
  private static LocalizationManager instance = null;

  [SerializeField]
  public List<TextAsset> languageFiles = new List<TextAsset>();

  private Dictionary<string, Language> languages = new Dictionary<string, Language>();
  private Language currentLanguage;

  private void Awake() {
    if (!instance) {
      instance = this;
      DontDestroyOnLoad(gameObject);
      LoadXml();
      currentLanguage = languages.FirstOrDefault().Value;
    }
    else {
      Destroy(gameObject);
    }
  }

  private void LoadXml() {
    foreach (var languageFile in languageFiles) {
      var languageXMLData = XDocument.Parse(languageFile.text, LoadOptions.None);
      var language = new Language();
      language.id = languageXMLData.Element("Language").Attribute("id").Value;

      foreach (var textx in languageXMLData.Element("Language").Elements("Text")) {
        language.text.Add(textx.Attribute("id").Value, textx.Value);
      }

      languages.Add(language.id, language);
    }
  }

  public string GetText(string key) {
    if (!currentLanguage.text.ContainsKey(key)) {
      return currentLanguage.id + "." + key;
    }
    return currentLanguage.text[key];
  }

  public void ChangeLanguage(string id) {
    if (!languages.ContainsKey(id)) return;
    currentLanguage = languages[id];
    onLanguageChanged();
  }
}

[System.Serializable]
public class Language {
  public string id;
  public Dictionary<string, string> text = new Dictionary<string, string>();
}
