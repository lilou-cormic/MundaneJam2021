using PurpleCable;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _Player = null;
    public static Player Player => Current._Player;

    [SerializeField] MainMenu MainMenu = null;

    [SerializeField] GameObject PausePanel = null;

    private Rocket _Rocket = null;
    public static Rocket Rocket => Current._Rocket;

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

    public static int CurrentLevelNumber { get; set; } = 0;

    public int LevelNumber
    {
        get => CurrentLevelNumber;

        set
        {
            CurrentLevelNumber = Mathf.Min(value, 6);
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

        SceneManager.LoadScene($"Level{CurrentLevelNumber}", mode: LoadSceneMode.Additive);

        ScoreManager.ResetScore();
    }

    private void Start()
    {
        _Rocket = FindObjectOfType<Rocket>();
        TimeLeft = _Rocket.CountDownTime;
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

            string oldLevelStates = PlayerPrefs.GetString("LevelStates", "000000");
            string newLevelStates = string.Empty;

            for (int i = 0; i < 6; i++)
            {
                if (i + 1 == LevelNumber)
                    newLevelStates += "1";
                else
                    newLevelStates += oldLevelStates[i].ToString();
            }

            PlayerPrefs.SetString("LevelStates", newLevelStates);

            Rocket.TakeOff();

            yield return new WaitForSeconds(3f);

            if (LevelNumber < 6)
            {
                LevelNumber++;

                MainMenu.LoadScene("Win");
            }
            else
            {
                MainMenu.LoadScene("LevelSelect");
            }
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
