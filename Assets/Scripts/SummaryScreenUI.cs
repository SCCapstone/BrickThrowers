using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class SummaryScreenUI : MonoBehaviour
{

    public GameObject summaryScreen;
    
    [Header("Summary")]
    [SerializeField] TextMeshProUGUI artifact;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI missed;
    [SerializeField] TextMeshProUGUI exp;
    [SerializeField] TextMeshProUGUI coins;

    private LevelManager lm = LevelManager.Instance;

    Player player;
    
    public void SetSummary(Player player){
        
        artifact.text = ""+player.artifactsGot;

    }

    public void SetSummary()
    {
        artifact.text = lm.Collected.ToString();
        score.text = lm.Score.ToString();
        missed.text = (lm.Artifacts.Length - lm.Collected).ToString();
        exp.text = lm.CalculateExp().ToString();
        coins.text = "$" + lm.CalculateCurr().ToString();
    }

    public void gameOver(Player player)
    {
        if (player.oxygenLevel <= 0)
        {
            summaryScreen.SetActive(true);
            SetSummary();
        }
    }

    public void ButtonClicked()
    {
        Debug.Log("Button Clicked");
    }

    public void ReturntoMainMenu()
    {

        SceneManager.LoadScene("Main Menu");

    }

}
