using UnityEngine;
using System.Collections;

public class TrollManage : MonoBehaviour 
{
	public Animator TrollAnimator;
	public AudioSource Troll_Audio;

	public AudioClip Attack1;
	public AudioClip Attack2;

	private float Touchtime;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			TrollAnimator.SetTrigger("Attack");
			Troll_Audio.PlayOneShot(Attack1);
			if(Time.realtimeSinceStartup - Touchtime < 2f)
			{
				TrollAnimator.SetTrigger("Attack12");
				Troll_Audio.PlayOneShot(Attack2);
			}
			Touchtime = Time.realtimeSinceStartup;
		}
	}


}
