using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] fishObjects; // Tableau d'objets de jeu à faire apparaître
    public GameObject[] popupObjects; // Tableau d'objets de popup
    public HUDFade interagirBtn; // Référence au bouton d'interaction
    public Transform popupContainer; // Conteneur pour les popups
    public Transform oceanTransform; // Objet océan
    public int fishDensity; // Nombre total d'objets à faire apparaître
    public float spawnDistance; // Distance d'apparition
    public float HUDTransitionSpeed; // Vitesse de transition du HUD
    public float mouseSensitivity; // Sensibilité de la souris
    public float distanceDetection; // Variable publique pour la détection de distance
    public Terrain terrain; // Objet terrain

    private readonly List<GameObject> activeFishes = new List<GameObject>(); // Pool d'objets
    private Popup popupOpened = null; // Popup actuellement ouvert
    private float terrainOffsetY; // Décalage en Y du terrain

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur
        Cursor.visible = false; // Cache le curseur

        foreach (GameObject fish in fishObjects)
        {
            InteractableClickHandler clickHandler = fish.GetComponent<InteractableClickHandler>();
            clickHandler.gameManager = this; // Associe le gestionnaire de clics à ce GameManager

            string popupName = fish.name + "_popup";

            GameObject popupObject = popupObjects.Where(popup => popup.name == popupName).First();

            if (popupObject == null)
            {
                throw new System.Exception("Objet popup non trouvé pour " + fish.name);
            }

            GameObject popupInstance = Instantiate(popupObject, popupContainer);

            popupInstance.SetActive(false); // Désactive la popup au début

            Popup popup = popupInstance.AddComponent<Popup>();
            HUDFade hudFade = popupInstance.AddComponent<HUDFade>();

            hudFade.gameManager = this;
            popup.gameManager = this;
            popup.hudFade = hudFade;

            clickHandler.popup = popup;
        }

        terrainOffsetY = terrain.transform.position.y; // Initialise le décalage Y du terrain

        SpawnFishes(fishDensity, true); // Fait apparaître les poissons
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit(); // Quitte l'application si "Escape" est pressé

        HandleFishSelect(); // Gère la sélection de poisson

        List<GameObject> fishesToRemove = new List<GameObject>(); // Liste des poissons à retirer

        foreach (var activeFish in activeFishes)
        {
            var distance = Vector3.Distance(activeFish.transform.position, Camera.main.transform.position);

            if (distance > spawnDistance)
            {
                fishesToRemove.Add(activeFish); // Ajoute le poisson à la liste des poissons à retirer
            }
        }

        foreach (var fisheToRemove in fishesToRemove)
        {
            activeFishes.Remove(fisheToRemove); // Retire le poisson de la liste des poissons actifs
            Destroy(fisheToRemove); // Détruit le poisson
        }

        SpawnFishes(fishDensity - activeFishes.Count); // Fait apparaître de nouveaux poissons si nécessaire
    }

    private void HandleFishSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance < distanceDetection && hit.collider.gameObject.CompareTag("Fish"))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                interagirBtn.FadeOut(); // Cache le bouton d'interaction

                InteractableClickHandler clickHandler = hit.collider.gameObject.GetComponent<InteractableClickHandler>();
                clickHandler.popup.Open(); // Ouvre la popup du poisson
            }
            else
            {
                interagirBtn.FadeIn(); // Affiche le bouton d'interaction
            }
        }
        else
        {
            interagirBtn.FadeOut(); // Cache le bouton d'interaction si aucun poisson n'est sélectionné
        }
    }

    private void SpawnFishes(int count, bool init = false)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fishToSpawn = fishObjects[Random.Range(0, fishObjects.Length)];
            float scaleY = fishToSpawn.transform.localScale.y;

            float spawnAngle = Random.Range(0, 2 * Mathf.PI);

            float spawnDistX = init ? Random.Range(0, spawnDistance - 0.5f) : spawnDistance - 0.5f;
            float spawnDistZ = init ? Random.Range(0, spawnDistance - 0.5f) : spawnDistance - 0.5f;

            Vector3 cameraPosition = Camera.main.transform.position;

            float spawnX = cameraPosition.x + spawnDistX * Mathf.Cos(spawnAngle);
            float spawnZ = cameraPosition.z + spawnDistZ * Mathf.Sin(spawnAngle);
            float spawnY = Random.Range(terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ)) + terrainOffsetY + scaleY / 2, oceanTransform.position.y * 0.9f);

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            float randomYRotation = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, randomYRotation, 0);
            GameObject spawnedFish = Instantiate(fishToSpawn, spawnPosition, randomRotation);

            activeFishes.Add(spawnedFish); // Ajoute le poisson à la liste des poissons actifs
        }
    }

    public void SetPopupOpened(Popup popup)
    {
        if (popupOpened != null)
        {
            popupOpened.Close(); // Ferme la popup précédemment ouverte
        }

        popupOpened = popup; // Définit la nouvelle popup ouverte
    }

    public void SetPopupClosed()
    {
        popupOpened = null; // Réinitialise la popup ouverte
    }
}
