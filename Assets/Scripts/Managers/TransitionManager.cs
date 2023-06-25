using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

    public static TransitionManager Instance;
    [SerializeField] private List<SceneID> sceneIDMap;

    private CanvasGroup canvas;
    private float targetAlpha;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    void Start() {
        canvas = GetComponentInChildren<CanvasGroup>();
        canvas.blocksRaycasts = false;
        FadeIn();
    }

    private void Update() {
        if (canvas.alpha != targetAlpha) {
            if (canvas.alpha < targetAlpha) canvas.alpha = Mathf.Min(canvas.alpha + Time.deltaTime, targetAlpha);
            else canvas.alpha = Mathf.Max(canvas.alpha - Time.deltaTime, targetAlpha);
        }
    }

    public void LoadScene(string name) {
        var sceneID = FindSceneID(name);
        if (sceneID > 0) StartCoroutine(ILoadScene(sceneID));
    }

    IEnumerator ILoadScene(int sceneID, bool fade = false) {
        if (fade) {
            FadeOut();
            while (canvas.alpha != targetAlpha) yield return null;
        } yield return SceneManager.LoadSceneAsync(sceneID);
        if (fade) {
            FadeIn();
        }
    }

    public void FadeOut(bool darken = false) {
        targetAlpha = darken ? 0.5f : 1f;
        canvas.blocksRaycasts = true;
    }

    public void FadeIn() {
        targetAlpha = 0f;
        canvas.blocksRaycasts = false;
    }

    public int FindSceneID(string name) {
        foreach (SceneID id in sceneIDMap) {
            if (id.name == name) return id.index;
        } Debug.LogWarning("No Scene named " + name + " was found.");
        return -1;
    }

    public void Quit() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

[System.Serializable]
public class SceneID {
    public string name;
    public int index;
}