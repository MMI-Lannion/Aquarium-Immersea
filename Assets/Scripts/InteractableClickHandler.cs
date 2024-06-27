using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableClickHandler : MonoBehaviour
{
    // Référence au Popup à afficher lorsqu'un objet est cliqué
    public Popup popup;
    // Référence au gestionnaire de jeu pour accéder aux paramètres du jeu
    public GameManager gameManager;

    // Cette méthode est appelée à chaque fois qu'un clic de souris est détecté sur l'objet
    void OnMouseDown()
    {
        // Vérifie si la distance entre la caméra principale et l'objet est inférieure à la distance de détection définie dans le gestionnaire de jeu
        if (Vector3.Distance(Camera.main.transform.position, transform.position) < gameManager.distanceDetection)
        {
            // Ouvre le popup
            popup.Open();
        }
    }
}
