// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class LetterCase : MonoBehaviour
// {
//     public char letter; // Lettre que cette case repr�sente
//
//     void OnTriggerEnter(Collider other)
//     {
//         if (other.GetComponent<DragAndDropPiece>() != null)
//         {
//             lvlManager.Instance.AddLetter(letter);
//         }
//     }
//
//     void OnTriggerExit(Collider other)
//     {
//         if (other.GetComponent<DragAndDropPiece>() != null)
//         {
//             lvlManager.Instance.RemoveLetter(letter);
//         }
//     }
// }
