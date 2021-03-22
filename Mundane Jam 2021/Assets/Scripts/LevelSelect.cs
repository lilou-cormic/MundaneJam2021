using PurpleCable;
using UnityEngine;

public class LevelSelect : UIButton
{
    [SerializeField] GameObject LevelCompleteIndicator = null;

    private int levelNumber;

    private void Start()
    {
        levelNumber = int.Parse(Label);

        LevelCompleteIndicator.SetActive(PlayerPrefs.GetString("LevelStates", "000000").Substring(levelNumber - 1, 1) == "1");
    }

    public void LoadLevel()
    {
        GameManager.CurrentLevelNumber = levelNumber;

        StartCoroutine(MainMenu.GoToScene("Main"));
    }
}
