using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    public static GameLogic Instance;

    public class CurrentScore {
        public int highestScore = 0;
        public int wavesSurvived = 0;
        public int satisfiedClients = 0;
        public int unsatisfiedClients = 0;
        public int tiredClients = 0;
        public int foodsShot = 0;
    } private CurrentScore currentScore;

    [SerializeField] private float endTimerAmount = 5;
    private float endTimer;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        endTimer = endTimerAmount;
    }

    // Update is called once per frame
    void Update() {
        if (Score.Instance != null && Score.Instance.SCORE < 0) {
            endTimer -= Time.deltaTime;
            if (endTimer <= 0) GameOver();
        }
    }

    private void IncreaseWavesSurvived() {
        currentScore.wavesSurvived++;
    }

    private void IncreasedSatisfiedClients() {
        currentScore.satisfiedClients++;
    }

    private void IncreaseUnsatisfiedClients() {
        currentScore.unsatisfiedClients++;
    }

    private void IncreaseTiredClients() {
        currentScore.tiredClients++;
    }

    private void IncreaseFoodsShots() {
        currentScore.foodsShot++;
    }

    private void GameOver() {
        endTimer = endTimerAmount;
        TransitionManager.Instance.LoadScene("GameOver");
    }
}
