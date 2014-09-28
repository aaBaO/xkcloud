using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class GetPostWWW : MonoBehaviour 
{
	private string HostName = "http://192.168.31.192:8080";
	private string ServiceURLPath = "/xihuan22dcloud/services/Buserservice/serviceYanzhengyonghu";
	private string[] Parameters = new string[] {"usernameOrEmailOrPhone", "passWord"};

	// Use this for initialization
	void Start () 
	{
		GetYanzhengyonghu ();
		PostYanzhengyonghu ();
		//StartCoroutine (postWebservice ());
	}

	public void GetYanzhengyonghu()
	{
		//将参数集合封装到Dictionary集合方便传值  
		Dictionary<string, string> dic = new Dictionary<string, string>();  
		
		//参数  
		dic.Add(Parameters[0], "竺章强");  
		dic.Add(Parameters[1], "123456");

		StartCoroutine (getWebservice (HostName + ServiceURLPath, dic));
		
	}

	public void PostYanzhengyonghu()
	{
		//将参数集合封装到Dictionary集合方便传值  
		Dictionary<string, string> dic = new Dictionary<string, string>();  
		
		//参数  
		dic.Add(Parameters[0], "竺章强");  
		dic.Add(Parameters[1], "123456");

		StartCoroutine (postWebservice (HostName + ServiceURLPath, dic));
	}

	IEnumerator getWebservice(string url, Dictionary<string,string> get)
	{
		//string url = "http://192.168.31.192:8080/xihuan22dcloud/services/Buserservice/serviceYanzhengyonghu?usernameOrEmailOrPhone=18758327079&passWord=123456"; 
		//string url = "http://192.168.31.192:8080/xihuan22dcloud/services/Buserservice/serviceYanzhengyonghu";

		string ParametersStr;  
		bool first;  
		if (get.Count > 0)  
		{  
			first = true;  
			ParametersStr = "?";  
			//从集合中取出所有参数，设置表单参数（AddField()).  
			foreach (KeyValuePair<string, string> post_arg in get)  
			{  
				if (first)  
					first = false;  
				else  
					ParametersStr += "&";  
				
				ParametersStr += post_arg.Key + "=" + post_arg.Value;  
			}  
		}  
		else  
		{  
			ParametersStr = "";  
		}  

		WWW www = new WWW (url + ParametersStr);
		
		yield return www;
		if (www.error != null)  
		{  
			//GET请求失败  
			Debug.Log("Error: " + www.error.ToString());
		}  
		else  
		{  
			//GET请求成功  
			Debug.Log (www.text);
		}
	}
	
	IEnumerator postWebservice(string url, Dictionary<string,string> post)
	{
		//		Hashtable headers = new Hashtable();    // 键值对存储
		//		headers.Add("Contend-Type", "application/x-www-form-urlencoded");   // 1.http头
		
//		string data = "usernameOrEmailOrPhone=竺章强&passWord=123456";  // 参数
//
//		byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);   // 1.数据。将参数转为字节
//		
//		// 3.网络传输器（URL，数据，http头)
//		WWW www = new WWW("http://192.168.31.192:8080/xihuan22dcloud/services/Buserservice/serviceYanzhengyonghu", bs);
//		
//		// yield 将需要return的数据全都放到迭代器内才执行
//		yield return www;
//		
//		Debug.Log( www.text);
//		
//		// 如果发生url错误等意外
//		if (www.error != null) {
//			Debug.Log( www.error.ToString());
//			
//			yield return null;
//		}

		//表单   
		WWWForm form = new WWWForm();  
		//从集合中取出所有参数，设置表单参数（AddField()).  
		foreach (KeyValuePair<string, string> post_arg in post)  
		{  
			form.AddField(post_arg.Key, post_arg.Value);  
		}  
		//表单传值，就是post   
		WWW www = new WWW(url, form);  
		
		yield return www;  
		
		if (www.error != null)  
		{  
			//POST请求失败  
			Debug.Log("Error: " + www.error.ToString());
		}  
		else  
		{  
			//POST请求成功  
			Debug.Log (www.text);
		}
	}
}
