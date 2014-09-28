// ------------------------------------------------------------------------------
// Copy right. 2014-2014  Moses Bao
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;

public class HandlemycloudTarget
{
	///<summary>
	///声明代理
	///</summary>
	public delegate bool mycloudRecoTargetEventHandle(object sender, mycloudRecoTargetEventArgs e);

	///<summary>
	///定义监视对象需要的数据的类 - object
	///</summary>
	public class mycloudRecoTargetEventArgs: EventArgs
	{
		public string _TargetID;
		public string _Metadata;
		
		public mycloudRecoTargetEventArgs(string targetid, string metadata)
		{
			this._TargetID = targetid;
			this._Metadata = metadata;
		}
	}

	#region TargetIDModify	
	/// <summary>
	/// 声明ModifycloudRecoTarget事件对象
	/// </summary>
	public event mycloudRecoTargetEventHandle ModifycloudRecoTargetEvent;

	/// <summary>
	/// 事件触发
	/// </summary>
	/// <param name="e">E.</param>
	public bool myOnModifyTarget(mycloudRecoTargetEventArgs e)
	{
		Debug.Log("******OnModifyTarget!!!******");
		if(ModifycloudRecoTargetEvent != null)
		{
			Debug.Log("******send!!!!!!******");
			return ModifycloudRecoTargetEvent(this, e);
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Handles the modify.
	/// </summary>
	/// <returns><c>true</c>, if modify was handled, <c>false</c> otherwise.</returns>
	/// <param name="targetid">Targetid.</param>
	/// <param name="metadata">Metadata.</param>
	public bool HandleModify(string targetid, string metadata)
	{
		mycloudRecoTargetEventArgs e = new mycloudRecoTargetEventArgs(targetid, metadata);

		return myOnModifyTarget(e);
	}
	#endregion


	#region HandleDownload
	/// <summary>
	/// Occurs when download video event.
	/// </summary>
	public event mycloudRecoTargetEventHandle DownloadVideoEvent;

	/// <summary>
	/// 事件触发
	/// </summary>
	/// <param name="e">E.</param>
	public bool OnDownloadVideo(mycloudRecoTargetEventArgs e)
	{
		if(DownloadVideoEvent != null)
		{
			return DownloadVideoEvent(this, e);
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Downloadmies the video.
	/// </summary>
	/// <returns><c>true</c>, if video was downloadmyed, <c>false</c> otherwise.</returns>
	/// <param name="targetid">Targetid.</param>
	/// <param name="metadata">Metadata.</param>
	public bool DownloadmyVideo(string targetid, string metadata)
	{
		mycloudRecoTargetEventArgs e = new mycloudRecoTargetEventArgs(targetid, metadata);

		return OnDownloadVideo(e);
	}
	#endregion
}


