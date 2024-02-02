using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLetter : MonoBehaviour
{
    public char letter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovableObject")) 
        {
            Debug.Log("TriggerLetter: OnTriggerEnter: other.tag = " + other.tag);
            WordManager.Instance.LetterActivated(letter); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovableObject"))
        {
            Debug.Log("TriggerLetter: OnTriggerExit: other.tag = " + other.tag);
            WordManager.Instance.LetterDeactivated(letter); 
        }
    }
}
