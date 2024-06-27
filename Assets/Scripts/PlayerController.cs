using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Sources audio pour le son sous l'eau et au-dessus de l'eau
    public AudioSource underwaterAudioSource;
    public AudioSource aboveWaterAudioSource;

    // Fréquences de coupure audio pour les transitions sous l'eau et au-dessus de l'eau
    public float underwaterAudioCutoffFrequency = 20000f;
    public float aboveWaterAudioCutoffFrequency = 100f;
    public float audioTransitionHeight = 0.5f; // Hauteur de transition audio

    // Références aux autres composants et objets dans la scène
    public CameraController cameraController;
    public Rigidbody p_Rigidbody;
    public float airControlFactor;
    public Transform cameraTransform;
    public Joystick movementJoystick;
    public float decelerationSpeed;
    public float gravityMultiplier;
    public Transform oceanTransform;

    // Filtres audio pour les transitions sous l'eau et au-dessus de l'eau
    private HighPass underwaterHighPassFilter;
    private LowPass aboveWaterLowPassFilter;

    // Initialisation au démarrage
    void Start()
    {
        // Récupération des composants de filtres audio
        underwaterHighPassFilter = underwaterAudioSource.GetComponent<HighPass>();
        aboveWaterLowPassFilter = aboveWaterAudioSource.GetComponent<LowPass>();
    }

    // Mise à jour de la physique à intervalles fixes
    void FixedUpdate()
    {
        MoveInAir(); // Appel de la fonction pour gérer le mouvement en l'air
    }

    // Gère la transition audio en fonction de la position du joueur par rapport à l'eau
    private void handleAudioTransition()
    {
        float y = transform.position.y + transform.localScale.y * 0.5f; // Position du joueur en y
        float max = oceanTransform.position.y; // Niveau maximum de transition
        float min = oceanTransform.position.y - audioTransitionHeight; // Niveau minimum de transition
        float t = Mathf.Clamp((y - min) / (max - min), 0f, 1f); // Calcul du facteur de transition

        // Transition du son sous l'eau
        underwaterHighPassFilter.cutoff = Mathf.Lerp(10f, underwaterAudioCutoffFrequency, Mathf.Pow(t, 4f));
        // Transition du son au-dessus de l'eau
        aboveWaterLowPassFilter.cutoff = Mathf.Lerp(aboveWaterAudioCutoffFrequency, 20000f, Mathf.Pow(t, 4f));

        // Activation ou désactivation des filtres en fonction de la fréquence de coupure
        underwaterHighPassFilter.dryWet = underwaterHighPassFilter.cutoff > 10f ? 1 : 0;
        aboveWaterLowPassFilter.dryWet = aboveWaterLowPassFilter.cutoff < 20000f ? 1 : 0;
    }

    // Gère le mouvement du joueur dans les airs
    void MoveInAir()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float absJoyX = Mathf.Abs(movementJoystick.Horizontal);
        float absJoyY = Mathf.Abs(movementJoystick.Vertical);

        // Ignorer la souris si le joystick est utilisé
        cameraController.SetIgnoreMouse(absJoyX != 0 || absJoyY != 0);

        // Utiliser l'entrée du joystick s'il y en a, sinon utiliser l'entrée du clavier
        float horizontal = absJoyX != 0 ? movementJoystick.Horizontal : horizontalInput;
        float vertical = absJoyY != 0 ? movementJoystick.Vertical : verticalInput;

        // Calcul de la direction du mouvement
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = cameraTransform.rotation * direction;

        // Application du contrôle en l'air
        p_Rigidbody.AddForce(direction * airControlFactor, ForceMode.Acceleration);

        // Application de la décélération
        if (p_Rigidbody.velocity.magnitude > 0.1f)
        {
            p_Rigidbody.AddForce(decelerationSpeed * Time.fixedDeltaTime * -p_Rigidbody.velocity, ForceMode.Force);
        }

        // Bloquer le joueur sur l'axe y s'il est au-dessus du niveau de l'océan
        if (transform.position.y + 0.5f > oceanTransform.position.y)
        {
            transform.position = new Vector3(transform.position.x, oceanTransform.position.y - 0.5f, transform.position.z);
        }

        // Application de la gravité
        ApplyGravity();

        // Gestion de la transition audio
        handleAudioTransition();
    }

    // Applique la gravité lorsque le joystick est inactif
    void ApplyGravity()
    {
        // Appliquer la gravité seulement s'il n'y a pas d'entrée
        if (movementJoystick.Horizontal == 0 && movementJoystick.Vertical == 0)
        {
            p_Rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }
}
