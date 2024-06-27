using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDFade : MonoBehaviour
{
    // Référence au GameManager
    public GameManager gameManager;

    // Alpha cible pour le fondu
    private int targetAlpha = 0;
    // Alpha actuel pour le fondu
    private float alpha = 0.0f;
    // Tableau des images enfants du HUD
    private UnityEngine.UI.Image[] childImages;

    // Initialisation
    void Start()
    {
        // Récupère toutes les images enfants
        childImages = GetComponentsInChildren<UnityEngine.UI.Image>();
    }

    // La fonction Update est appelée une fois par frame
    void Update()
    {
        // Sauvegarde de l'alpha précédent
        float previousAlpha = alpha;
        // Calcule le nouvel alpha en se rapprochant de l'alpha cible
        alpha = Mathf.MoveTowards(alpha, targetAlpha, gameManager.HUDTransitionSpeed * Time.deltaTime);

        // Applique le nouvel alpha à toutes les images enfants
        foreach (UnityEngine.UI.Image childImage in childImages)
        {
            Color color = childImage.color;
            color.a = alpha;
            childImage.color = color;
        }

        // Vérifie si le fondu est terminé
        if (targetAlpha == 0 && Mathf.Abs(previousAlpha - targetAlpha) > Mathf.Epsilon && Mathf.Abs(alpha - targetAlpha) <= Mathf.Epsilon)
        {
            // Appelle la fonction de fin de fondu
            OnFadeOutEnd();
        }
    }

    // Fonction pour faire apparaître le HUD
    public void FadeIn()
    {
        gameObject.SetActive(true);
        targetAlpha = 1;
    }

    // Fonction pour faire disparaître le HUD
    public void FadeOut()
    {
        targetAlpha = 0;
    }

    // Fonction appelée à la fin du fondu de disparition
    public void OnFadeOutEnd()
    {
        gameObject.SetActive(false);
    }
}
