using UnityEngine;

public class AmthanhZombie : MonoBehaviour
{
    public AudioSource zombieAudio;
    public Transform player2;
    public float activationDistance = 10f;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player2.position);

        if (distance <= activationDistance)
        {
            if (!zombieAudio.isPlaying)
            {
                zombieAudio.Play();
            }
        }
        else
        {
            if (zombieAudio.isPlaying)
            {
                zombieAudio.Stop();
            }
        }
    }
}
