using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    // Déclaration des variables publiques
    public Rigidbody p_Rigidbody;  // Référence au Rigidbody du joueur
    public Joystick movementJoystick;  // Référence au joystick pour le mouvement
    public float bobbingSpeed;  // Vitesse du mouvement de balancement
    public float bobbingAmount;  // Amplitude du mouvement de balancement

    // Déclaration des variables privées
    private Vector3 initialTransform;  // Position initiale de la tête
    private float timer = 0;  // Timer pour calculer le déplacement

    // Méthode appelée au début
    void Start()
    {
        // Initialiser la position initiale de la tête
        initialTransform = transform.localPosition;
    }

    // Méthode appelée à chaque frame
    void Update()
    {
        // Augmenter le timer en fonction du temps écoulé et de la vitesse du Rigidbody
        timer += Time.deltaTime * (1 + p_Rigidbody.velocity.magnitude * 0.5f);

        // Calculer l'amplitude du balancement en fonction de la vitesse du Rigidbody
        float bobbingAmplitude = bobbingAmount * (1 + p_Rigidbody.velocity.magnitude * 0.8f);

        // Calculer le déplacement en X à l'aide du bruit de Perlin
        float offsetX = (Mathf.PerlinNoise(timer * bobbingSpeed, 0) - 0.5f) * bobbingAmplitude * 0.5f;

        // Calculer le déplacement en Y à l'aide du bruit de Perlin
        float offsetY = (Mathf.PerlinNoise(0, timer * bobbingSpeed) - 0.5f) * bobbingAmplitude;

        // Appliquer le déplacement à la position locale de la tête
        transform.localPosition = new Vector3(initialTransform.x + offsetX, initialTransform.y + offsetY, initialTransform.z);
    }
}
