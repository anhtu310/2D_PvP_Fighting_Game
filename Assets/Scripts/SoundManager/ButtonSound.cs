using UnityEngine;

public class ButtonSound : MonoBehaviour
{
	public AudioClip clickSound;
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayClickSound()
	{
		audioSource.PlayOneShot(clickSound);
	}
}
