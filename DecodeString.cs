using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class DecodeString : MonoBehaviour
{
	private string Str = "40%%22bao%%22xihuan22@qq.com%%2218758327029%%22n%%22null%%22null%%22登录%%222014-09-25 10:37:48.0%%22DoiuAH4TX7bZLDHWfq2nBF%%22普通%%22一次最大上传10m%%2210%%221024%%220%%22error";
	private string[] Pamraters;
	private Dictionary<string, string> UserData;
	private int Length;
	
	void Start () 
	{
		Pamraters = Regex.Split(Str, "%%22", RegexOptions.IgnoreCase);
		Length = Pamraters.Length / 2;

		UserData = new Dictionary<string, string>();

		for(int i = 0 ; i < Length; i += 2)
		{
			//"Targetname",  "VideoURL"
			UserData.Add(Pamraters[i], Pamraters[i+1]);
		}

		foreach (KeyValuePair<string, string> post_arg in UserData)  
		{    	
			Debug.Log(post_arg.Key + "   " + post_arg.Value);  
		}  
	}

}
