using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioClip walkSound;  // Kındimise heli
    private AudioSource audioSource;  // AudioSource komponent

    public float walkSpeed = 5f;  // Kiirus, mille j‰rgi kohandada heli kiirus

    private float moveX;  // Horisontaalne liikumisandur
    private float moveZ;  // Vertikaalne liikumisandur

    // Start meetod, mis algatab AudioSource komponendi
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  // Saame AudioSource komponendi
    }

    // Update meetod, mis kontrollib, kas tegelane liigub ja m‰ngib kındimise heli
    private void Update()
    {
        // Liikumisjoonised (horisontaalne ja vertikaalne liikumine)
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // Kui tegelane liigub
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            // Kui heli ei m‰ngi, siis alusta
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkSound;  // Seadista kındimise heli
                audioSource.loop = true;  // Helil on ts¸kliline kordus
                audioSource.Play();  // Alusta heli m‰ngimist
            }

            // Arvutame liikumiskiirus ja m‰‰rame helikiirus (pitch)
            float speed = new Vector3(moveX, 0, moveZ).magnitude;  // Liikumiskiirus
            float pitchValue = 1 + (speed / walkSpeed);  // Pitch v‰‰rtus vastavalt liikumiskiirusest

            // Logi liikumiskiirus ja pitch v‰‰rtus, et kontrollida
            Debug.Log("Movement Speed: " + speed);
            Debug.Log("Pitch Value: " + pitchValue);

            // Seadista audio pitch v‰‰rtus
            audioSource.pitch = pitchValue;
        }
        else
        {
            // Kui tegelane seisab, peata kındimise heli
            audioSource.Stop();
        }
    }
}
