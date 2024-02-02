using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;
    public int player = 0;
    private HashSet<char> activatedLetters = new HashSet<char>();
    public string targetWord = "ABC"; // Le mot cible

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LetterActivated(char letter)
    {
        activatedLetters.Add(letter);
        CheckWordCompletion();
    }

    public void LetterDeactivated(char letter)
    {
        activatedLetters.Remove(letter);
    }

    private void CheckWordCompletion()
    {
        foreach (char letter in targetWord)
        {
            Debug.Log("CheckWordCompletion: letter = " + letter);
            if (!activatedLetters.Contains(letter))
            {
                return;
            }
        }
        Debug.Log("Mot formé correctement !");
        GameManager.Instance.validateWord(player);
    }
}
