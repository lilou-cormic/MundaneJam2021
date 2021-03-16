using PurpleCable;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _Player = null;
    public static Player Player => Current._Player;

    [SerializeField] MainMenu MainMenu = null;

    [SerializeField] GameObject PausePanel = null;

    public static float TimeLeft { get; private set; }

    private bool _IsPaused = false;
    private bool IsPaused
    {
        get => _IsPaused;

        set
        {
            _IsPaused = value;

            Time.timeScale = (_IsPaused ? 0 : 1);

            if (PausePanel != null)
                PausePanel.gameObject.SetActive(_IsPaused);

            if (_IsPaused)
            {
                EventSystem eventSystem = FindObjectOfType<EventSystem>();

                //HACK
                eventSystem.SetSelectedGameObject(gameObject);

                eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
            }
        }
    }

    public static bool IsGamePaused => Current.IsPaused;

    private static GameManager Current { get; set; }

    public static int CurrentLevelNumber { get; private set; } = 0;

    public int LevelNumber
    {
        get => CurrentLevelNumber;

        set
        {
            CurrentLevelNumber = value;
        }
    }

    private float _Gravity = 1;
    public static float Gravity => Current._Gravity;

    private bool _gameIsEnding = false;

    private void Awake()
    {
        Current = this;

        if (CurrentLevelNumber == 0)
            LevelNumber = 1;

        TimeLeft = 120f;

        ScoreManager.ResetScore();
    }

    private void OnDestroy()
    {
        UnPause();

        Current = null;
    }

    private void Update()
    {
        if (!_gameIsEnding)
            TimeLeft -= Time.deltaTime;

        if (Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel"))
            IsPaused = !IsPaused;

        if (TimeLeft <= 0)
            Win();
    }

    public static void Win()
        => Current.StartCoroutine(Current.DoWin());

    private IEnumerator DoWin()
    {
        if (!_gameIsEnding)
        {
            _gameIsEnding = true;

            GetComponent<MusicWithIntro>()?.Stop();
            //MusicManager.PlayWinJingle();

            ScoreManager.AddPoints(LevelNumber);
            ScoreManager.SetHighScore();

            LevelNumber++;

            yield return new WaitForSeconds(2f);

            //if (CurrentLevel != null)
            MainMenu.LoadScene("Win");
            //else
            //    MainMenu.LoadScene("Congratulations");
        }
    }

    public static void GameOver()
        => Current.StartCoroutine(Current.DoGameOver());

    public IEnumerator DoGameOver()
    {
        if (!_gameIsEnding)
        {
            _gameIsEnding = true;

            GetComponent<MusicWithIntro>()?.Stop();
            //MusicManager.PlayLoseJingle();

            ScoreManager.AddPoints(LevelNumber - 1);
            ScoreManager.SetHighScore();

            //if (Settings.Difficulty == Settings.DifficultyMode.Hard)
            LevelNumber = 1;

            yield return new WaitForSeconds(0.02f);

            MainMenu.LoadScene("GameOver");
        }
    }

    public void UnPause()
    {
        IsPaused = false;
    }

    public void GoToMenu()
    {
        UnPause();

        MainMenu.GoToMenu();
    }
}
