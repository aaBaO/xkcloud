/*==============================================================================
Copyright (c) 2014-2014 Moses BaO
==============================================================================*/

using UnityEngine;
using System.Collections;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class XKCloudTrackable : MonoBehaviour,
								ITrackableEventHandler
{
	#region PUBCLIC_MEMBER_VARIABLES
	public GameObject Video;
	public Animator Dragon;
	public AudioSource Music;
	public GameObject Model;
	#endregion

	#region PRIVATE_MEMBER_VARIABLES
	
	private TrackableBehaviour mTrackableBehaviour;

	private bool mHasBeenFound = false;
	private bool mLostTracking;
	private float mSecondsSinceLost;

	private bool PlayFullScreen;
	private bool Modelenbled;
	
	#endregion // PRIVATE_MEMBER_VARIABLES
	
	
	
	#region UNTIY_MONOBEHAVIOUR_METHODS
	void Awake()
	{
		//StartCoroutine (LoadDragonFly ());
	}

	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
		CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
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

//		if(Input.GetMouseButtonDown(0) && Modelenbled )
//		{
//			Dragon.SetTrigger("fly");
//		}
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
		// Stop finder since we have now a result, finder will be restarted again when we lose track of the result
		ImageTracker imageTracker = TrackerManager.Instance.GetTracker<ImageTracker>();
		if(imageTracker != null)
		{
			imageTracker.TargetFinder.Stop();
		}

//		if (MosesData.Xiaochi) 
//		{
//			showModel(true);
//			showVideo(false);
//		}
//		else
//		{
//			Debug.Log("xiaochi 22");
//		}
			
		Debug.LogError ("Trackable " + mTrackableBehaviour.TrackableName + " found");
//		else 
//		{
#if UNITY_IOS
		if (MosesData.Videopath.Contains ("://")) 
		{
			PlayFullScreen = true;
		} 
		else 
		{
			PlayFullScreen = false;
		}
		showVideo (true);
		
#elif UNITY_ANDROID
		MosesData.Found = true;
		PlayFullScreen = false;
		showVideo (true);
#endif
//			showModel(false);
//		}
	}
	
	
	private void OnTrackingLost()
	{
		// Start finder again if we lost the current trackable
		ImageTracker imageTracker = TrackerManager.Instance.GetTracker<ImageTracker>();
		if(imageTracker != null)
		{
			imageTracker.TargetFinder.ClearTrackables(false);
			imageTracker.TargetFinder.StartRecognition();
		}
		
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

		MosesData.Found = false;
		showVideo(false);
		//showModel(false);
	}

	private void showVideo(bool tf)
	{
		if(!PlayFullScreen)
			RenderandCollider (Video, tf);
		
		if (tf) 
		{
			VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour> ();
			video.m_path = MosesData.Videopath;
			video.mIsInited = false;
			video.mIsPrepared = false;
			StartCoroutine("FindandPlayvideo");
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

	private void showModel(bool tf)
	{
		if(Model != null)
		{
			RenderandCollider (Model, tf);
			//RenderandCollider (Zimu,tf);
			Modelenbled = tf;
			if(tf)
			{
				Music.Play();
			}
			else
			{
				Music.Stop();
			}
		}
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

	IEnumerator FindandPlayvideo()
	{
		VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour> ();

		while(video.VideoPlayer.GetStatus() == VideoPlayerHelper.MediaState.NOT_READY 
		     || video.VideoPlayer.GetStatus() == VideoPlayerHelper.MediaState.ERROR 
		     || video.mIsInited == false || video.mIsPrepared == false)
		{
//			Debug.LogError("video.mIsInited:" + video.mIsInited + ".video.mIsPrepared :" + video.mIsPrepared );
			yield return 0;
		}

		if (video != null && video.AutoPlay) 
		{
			#if UNITY_IOS
			bool isplayfullscreen = video.VideoPlayer.IsPlayableFullscreen();
			#elif UNITY_ANDROID
			bool isplayfullscreen = false;
			#endif
			VideoPlayerHelper.MediaState state = video.VideoPlayer.GetStatus ();

			if (state == VideoPlayerHelper.MediaState.PAUSED ||
			    state == VideoPlayerHelper.MediaState.READY ||
			    state == VideoPlayerHelper.MediaState.STOPPED) 
			{
				// Pause other videos before playing this one
				PauseOtherVideos (video);
				
				// Play this video on texture where it left off
				video.VideoPlayer.Play (isplayfullscreen, video.VideoPlayer.GetCurrentPosition ());
				
			} 
			else if (state == VideoPlayerHelper.MediaState.REACHED_END) 
			{
				// Pause other videos before playing this one
				PauseOtherVideos (video);
				
				// Play this video from the beginning
				video.VideoPlayer.Play (isplayfullscreen, 0);

			}
		}
	}

	IEnumerator LoadDragonFly()
	{
		WWW bundle = WWW.LoadFromCacheOrDownload("http://www.moses-ic.com/AssetBundles/ios_dragonfly.assetbundle", 1);
		
		yield return  bundle;

		GameObject dragonfly = Instantiate(bundle.assetBundle.mainAsset) as GameObject;
		dragonfly.transform.parent = this.gameObject.transform;
		dragonfly.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		dragonfly.transform.localRotation = Quaternion.Euler(0, 180, 0);
		yield return dragonfly;

		Dragon = dragonfly.GetComponent<Animator> ();
		yield return Dragon;

		Music = dragonfly.GetComponent<AudioSource> ();
		yield return Music;

		Model = dragonfly;
		yield return Model;

		showModel(false);

		//Debug.Log("LOAD SUCCESS!");
		bundle.assetBundle.Unload(false);
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

	#endregion // PRIVATE_METHODS
}
