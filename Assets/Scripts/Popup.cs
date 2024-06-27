using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameManager gameManager; // Référence au gestionnaire de jeu
    public HUDFade hudFade; // Référence à la classe de fondu de l'interface

    // Update est appelée une fois par frame
    void Update()
    {
        // Si la touche 'E' est enfoncée et que l'objet de jeu est actif
        if (Input.GetKeyDown(KeyCode.E) && gameObject.activeSelf)
        {
            Close(); // Ferme la popup
        }
    }

    // Méthode pour ouvrir la popup
    public void Open()
    {
        // Indique au gestionnaire de jeu que la popup est ouverte
        gameManager.SetPopupOpened(this);

        // Fait apparaître la popup avec un effet de fondu
        hudFade.FadeIn();
    }

    // Méthode pour fermer la popup
    public void Close()
    {
        // Indique au gestionnaire de jeu que la popup est fermée
        gameManager.SetPopupClosed();

        // Fait disparaître la popup avec un effet de fondu
        hudFade.FadeOut();
    }
}
