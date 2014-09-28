using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DownloadVideo :MonoBehaviour
{
	private static bool Begin;
	private static bool Done;


	public static bool DoDownloadmyVideoFromURL(System.Object sender, HandlemycloudTarget.mycloudRecoTargetEventArgs e)
	{
		Begin = true;
		return true;
	}

	private void Start()
	{
		StartCoroutine(HandleDownload());
	}

	private static IEnumerator HandleDownload()
	{
		while(!Begin)
		{
			yield return 0;
		}

	}
}
