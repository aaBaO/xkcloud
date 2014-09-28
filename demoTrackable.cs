/*==============================================================================
Copyright (c) 2014-2014 Moses Bao
==============================================================================*/

using UnityEngine;
using System.Collections;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class demoTrackable : MonoBehaviour,
ITrackableEventHandler
{
	public GameObject Troll;
	public GameObject Video;
	public GameObject Girl;
	public AudioSource Music;
	public Animator nobodyGirl;
	public UIButton ButtonBack;
	#region PRIVATE_MEMBER_VARIABLES
	
	private TrackableBehaviour mTrackableBehaviour;

	private bool mHasBeenFound = false;
	private bool mLostTracking;
	private float mSecondsSinceLost;

	private int ObjID;
	private float lastChangedtime;
	private bool isFound;
	#endregion // PRIVATE_MEMBER_VARIABLES
	
	#region UNTIY_MONOBEHAVIOUR_METHODS
	
	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
		OnTrackingLost ();
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		EventDelegate.Add (ButtonBack.onClick, ClickBack);
	}

	void Update()
	{
		// Pause the video if tracking is lost for more than two seconds
		if (mHasBeenFound && mLostTracking)
		{
			if (mSecondsSinceLost > 0.5f)
			{
				VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour>();
				if (video != null &&
				    video.CurrentState == VideoPlayerHelper.MediaState.PLAYING)
				{
					video.VideoPlayer.Pause();
				}
				
				mLostTracking = false;
			}
			
			mSecondsSinceLost += Time.deltaTime;
		}
		
		VideoPlaybackBehaviour video2 = GetComponentInChildren<VideoPlaybackBehaviour> ();
		if (video2 != null && video2.CurrentState == VideoPlayerHelper.MediaState.REACHED_END) 
		{
			video2.VideoPlayer.Play(false, 0);	
			
		}
		
		if (Input.touchCount == 2 && lastChangedtime > 1.0f && isFound) 
		{
			ObjID += 1;
			if(ObjID > 2)
			{
				ObjID = 0;
			}
			switch(ObjID)
			{
				case 0:
					{
						showVideo(true);
						showGirl(false);
						showTroll(false);
						break;
					}
				case 1:
					{
						showVideo(false);
						showGirl(true);
						showTroll(false);
						break;
					}
				case 2:
					{
						showVideo(false);
						showGirl(false);
						showTroll(true);
						break;
					}
			}
			lastChangedtime = 0;
		}
		lastChangedtime += Time.deltaTime;

	}

	#endregion // UNTIY_MONOBEHAVIOUR_METHODS
	
	#region PUBLIC_METHODS
	
	/// <summary>
	/// Implementation of the ITrackableEventHandler function called when the
	/// tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			OnTrackingFound();
		}
		else
		{
			OnTrackingLost();
		}
	}
	
	#endregion // PUBLIC_METHODS
	
	
	
	#region PRIVATE_METHODS
	
	
	private void OnTrackingFound()
	{
		switch(ObjID)
		{
			case 0:
			{
				showVideo(true);
				showGirl(false);
				showTroll(false);
				break;
			}
			case 1:
			{
				showVideo(false);
				showGirl(true);
				showTroll(false);
				break;
			}
			case 2:
			{
				showVideo(false);
				showGirl(false);
				showTroll(true);
				break;
			}
		}
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

		mHasBeenFound = true;
		mLostTracking = false;
		isFound = true;
	}
	
	
	private void OnTrackingLost()
	{
		showVideo(false);
		showGirl(false);
		showTroll(false);

		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

		mLostTracking = true;
		isFound = false;
		mSecondsSinceLost = 0;
	}

	private void showVideo(bool tf)
	{
		RenderandCollider (Video, tf);
		
		if (tf) 
		{
			VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour> ();
			if (video != null && video.AutoPlay) 
			{
				if (video.VideoPlayer.IsPlayableOnTexture ()) 
				{
					VideoPlayerHelper.MediaState state = video.VideoPlayer.GetStatus ();
					if (state == VideoPlayerHelper.MediaState.PAUSED ||
					    state == VideoPlayerHelper.MediaState.READY ||
					    state == VideoPlayerHelper.MediaState.STOPPED) 
					{
						// Pause other videos before playing this one
						PauseOtherVideos (video);
						
						// Play this video on texture where it left off
						video.VideoPlayer.Play (false, video.VideoPlayer.GetCurrentPosition ());
					} 
					else if (state == VideoPlayerHelper.MediaState.REACHED_END) 
					{
						// Pause other videos before playing this one
						PauseOtherVideos (video);
						
						// Play this video from the beginning
						video.VideoPlayer.Play (false, 0);
					}
				}
			}
		}
		else
		{
			VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour> ();
			PauseOtherVideos(video);
			if (video != null &&
			    video.CurrentState == VideoPlayerHelper.MediaState.PLAYING)
			{
				video.VideoPlayer.Pause();
			}
			
		}
	}

	private void showGirl(bool tf)
	{
		if(tf)
		{
			nobodyGirl.SetBool ("dance", true);
			Music.Play();
		}
		else
		{
			nobodyGirl.SetBool ("dance", false);
			Music.Stop();
		}
		RenderandCollider (Girl, tf);
	}

	private void showTroll(bool tf)
	{
		RenderandCollider (Troll, tf);
		Troll.GetComponent<TrollManage> ().enabled = tf;
	}

	private void RenderandCollider(GameObject obj,bool tf)
	{
		Renderer[] rendererComponentsp = obj.GetComponents<Renderer>();
		Collider[] colliderComponentsp = obj.GetComponents<Collider>();
		if(rendererComponentsp.Length > 0 )
		{
			// Enable rendering:
			foreach (Renderer component in rendererComponentsp)
			{
				//Debug.Log("p:" + component.name);
				component.enabled = tf;
			}
		}
		if(colliderComponentsp.Length > 0 )
		{
			// Enable colliders:
			foreach (Collider component in colliderComponentsp)
			{
				component.enabled = tf;
			}
		}
		
		foreach (Transform child in obj.transform) 
		{
			if(child.gameObject.GetComponent<Renderer>())
			{
				//Debug.Log("render:" + child.gameObject.GetComponent<Renderer>().name);
				child.gameObject.GetComponent<Renderer>().enabled  = tf;
			}
			
			if(child.gameObject.GetComponent<Collider>())
			{
				//Debug.Log("collider:" + child.gameObject.GetComponent<Collider>().name);
				child.gameObject.GetComponent<Collider>().enabled = tf;
			}
			RenderandCollider(child.gameObject, tf);
		}
	}

	// Pause all videos except this one
	private void PauseOtherVideos(VideoPlaybackBehaviour currentVideo)
	{
		VideoPlaybackBehaviour[] videos = (VideoPlaybackBehaviour[])
			FindObjectsOfType(typeof(VideoPlaybackBehaviour));
		
		foreach (VideoPlaybackBehaviour video in videos)
		{
			if (video != currentVideo)
			{
				if (video.CurrentState == VideoPlayerHelper.MediaState.PLAYING)
				{
					video.VideoPlayer.Pause();
				}
			}
		}
	}

	private void ClickBack()
	{
		Application.LoadLevel("Index");
	}
	#endregion // PRIVATE_METHODS
}
