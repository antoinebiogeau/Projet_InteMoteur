using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public int player = 0;
    [Header("Kelyan UI")]
    [SerializeField] private TMP_Text compteurRougeText;
    [SerializeField] private TMP_Text compteurBleuText;
    private int compteurRouge = 0;
    private int compteurBleu = 0;
    [SerializeField] private GameObject uiComptageBallons;
    [SerializeField] private GameObject UIKelyan;
    [SerializeField] private GameObject UIAlix;
    [SerializeField] private GameObject UIAimie;
    [SerializeField] private GameObject UIAntoine;
    private GameObject currentUI;
    [Header("UI Antoine")]
    [SerializeField] private GameObject camera1;
    [SerializeField] private GameObject camera2;
    
    void Start()
    {
        currentUI = UIKelyan;
        uiComptageBallons.SetActive(false);
    }   
    public void AjouterBallonRouge()
    {
        compteurRouge++;
        MettreAJourUI();
    }

    public void AjouterBallonBleu()
    {
        compteurBleu++;
        MettreAJourUI();
    }
    public void EnleverBallonRouge()
    {
        if (compteurRouge > 0)
        {
            compteurRouge--;
            MettreAJourUI();
        }
    }
    public void EnleverBallonBleu()
    {
        if (compteurBleu > 0)
        {
            compteurBleu--;
            MettreAJourUI();
        }
    }

    public void Quitter()
    {
        uiComptageBallons.SetActive(false);
    }

    private void MettreAJourUI()
    {
        compteurRougeText.text = "" + compteurRouge.ToString();
        compteurBleuText.text = "" + compteurBleu.ToString();
    }
    public void ValidateBloons()
    {
        if (GameManager.Instance.ValidateBloons(player, compteurRouge, compteurBleu))
        {
            ChangeIU();
        }
    }

    public void ChangeIU()
    {
        currentUI.SetActive(false);
        if (currentUI == UIKelyan)
        {
            currentUI = UIAlix;
        }
        else if (currentUI == UIAlix)
        {
            currentUI = UIAntoine;
        }
        else if (currentUI == UIAntoine)
        {
            currentUI = UIAimie;
        }
        currentUI.SetActive(true);
    }
    public void ToggleCamera()
    {
        GameManager.Instance.SwitchCamAnt(player);
    }
    public void GameOver()
    {
        
    }
}
