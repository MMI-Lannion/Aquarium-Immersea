using UnityEngine;

// Classe qui gère les clics pour fermer une fenêtre popup
public class ClosePopupClickHandler : MonoBehaviour
{
    // Référence à l'objet Popup
    private Popup popup;

    // Méthode appelée au démarrage
    void Start()
    {
        // Obtient le composant Popup du parent de ce GameObject
        popup = GetComponentInParent<Popup>();
    }

    // Méthode pour fermer la popup
    public void Close()
    {
        // Vérifie si l'objet Popup n'est pas nul
        if (popup != null)
        {
            // Appelle la méthode Close sur l'objet Popup
            popup.Close();
        }
    }
}
