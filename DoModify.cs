// ------------------------------------------------------------------------------
// Copy Right  2014-2014 Moses BaO
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class DoModify: MonoBehaviour
{
	private static string GetStr = "386cf4492eea4691b0bded2bc7f45260%%22478653e358694cff88fa7d058e71318c%%22795c1fdc7a084f76be776a56b847d385";
	//private static string GetStr;
	private static string[] Pamraters;
	private static Dictionary<string, string> UserData;
	private static int Length;

	///声明一个委托
	public delegate void HandleMetadataDelegate(string metadata);

	/// <summary>
	/// Handles the metadata.
	/// 将函数当作参数来传递
	/// 类似与一个代码模版
	/// </summary>
	/// <param name="metadata">Metadata.</param>
	/// <param name="DoHandle">Do handle.</param>
	public static void HandleMetadata(string metadata, HandleMetadataDelegate DoHandle)
	{
		DoHandle(metadata);
	}

	/// <summary>
	/// Save the video path.
	/// </summary>
	/// <param name="metadata">Metadata.</param>
	private static void SaveVideoPath(string metadata)
	{
		MosesData.Videopath = metadata;
	}

	/// <summary>
	/// Save the assetbundle path.
	/// </summary>
	/// <param name="metadata">Metadata.</param>
	private static void SaveAssetbundlePath(string metadata)
	{
		MosesData.AssetbundlePath = metadata;
	}

	#region  
	/// <summary>
	/// 将识别出的目标与数据匹配，确认是否为准确目标
	/// </summary>
	/// <returns><c>true</c>, if modifymycloud reco target was right, <c>false</c> otherwise.</returns>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	public static bool DoModifymycloudRecoTargetEvent (System.Object sender, HandlemycloudTarget.mycloudRecoTargetEventArgs e)
	{
//		if(e._TargetID == "Xiaochi2")
//		{
//			MosesData.Xiaochi = false;
//			return true;
//		}
//		else
//		{
//			return false;
//		}
		//DoSplit(GetStr);
		
		if(UserData.ContainsKey(e._TargetID))
		{
			HandleMetadata(e._Metadata, SaveVideoPath);
			return true;
		}
		else
		{
			return false;
		}
	}
	#endregion

	private static void DoSplit(string str)
	{
		Pamraters = Regex.Split(str, "%%22", RegexOptions.IgnoreCase);
		Length = Pamraters.Length;
		
		UserData = new Dictionary<string, string>();
		
		for(int i = 0 ; i < Length; i ++)
		{
			//"TargetID",  "VideoOnlineorCache"
			UserData.Add(Pamraters[i], "Online");
		}
		
		foreach (KeyValuePair<string, string> pairvalue in UserData)  
		{    	
			Debug.Log("TargetID:" + pairvalue.Key + "--Value:" + pairvalue.Value);  
		}  

	}

	private void messgae(string str)
	{
		GetStr = str;
		Debug.LogError("******GETSTR:" + str);
		DoSplit(GetStr);
	}
}


