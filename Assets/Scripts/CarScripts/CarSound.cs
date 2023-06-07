//Écrit par Thomas Prévost, Charles Trottier et Frédéric Gaudreault

using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float minVolume;
    [SerializeField] private float maxVolume;
    [SerializeField] private float minSpeed;

    private TransmissionComponent tc;
    private AudioSource audioSource;
    private MotorComponent mc;
    private float maxCouple;
    private float ratioRev;
    private float deltaPitch;
    private float deltaVolume;

    void Start()
    {
        tc = GetComponent<TransmissionComponent>();
        audioSource = GetComponent<AudioSource>();
        mc = GetComponent<MotorComponent>();
        maxCouple = mc.maxCoupleAvant;
        deltaPitch = maxPitch - 1;
        deltaVolume = maxVolume - minVolume;
    }

    void Update()
    {
        if (Time.timeScale < 0.001f)
        {
            if (audioSource.enabled)
                audioSource.enabled = false;
        }
        else
        { 
            if (!audioSource.enabled)
                audioSource.enabled = true;
            
            ratioRev = mc.coupleMoteur / maxCouple;
            audioSource.pitch = tc.vitesse < minSpeed ? minPitch + ratioRev : minPitch + deltaPitch - deltaPitch / (1 + (float)tc.vitesse * 0.01f) + ratioRev;
            audioSource.volume = minVolume + ratioRev * deltaVolume;        
        }
    }
}
