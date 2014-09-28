using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class XMLFileManager : MonoBehaviour {

	private string urlstr;
	private string logstr;
	private string logstr2;

	private WWW downdata;

	// Use this for initialization
	void Start () {
//		string url = "http://www.moses-ic.com/test/testjson.txt";
//		StartCoroutine(DownloadJsonData(url));


		#if UNITY_EDITOR   
		string filepath = Application.dataPath +"/StreamingAssets";   
		#elif UNITY_IPHONE   
		string filepath = Application.dataPath +"/Raw" + "/StatesXML.xml";   
		#elif UNITY_ANDROID   
		//string filepath = "jar:file://" + Application.dataPath + "!/assets" + "/StatesXML.xml";    
		string filepath = "/sdcard/MosesOneXML";
		#endif
//		StartCoroutine(ReadJsonData(filepath));
		//CreateXML (filepath);
	}

	void OnGUI()
	{
		GUILayout.Label(logstr); 
		GUILayout.Label(logstr2); 

//		GUILayout.Label((downdata.progress * 100).ToString()); 
//		
//
//		if(GUI.Button(new Rect(100,100,100,100),"Button"))
//		{
//			Application.LoadLevel(1);
//		}
	}

//	IEnumerator ReadJsonData(string path)
//	{
//		FileInfo fileinfo = new FileInfo(path);
//		while(!fileinfo.Exists)
//		{
//			yield return 0;
//		}
//		StreamReader sr = File.OpenText(path);
//		string filestr = sr.ReadToEnd();
//	}
//	
//	void CreateXML(string path)
//	{
//		Directory.CreateDirectory (path);
//		string filepath = path + "/StatesXML.xml";
//		if (!File.Exists (filepath)) 
//		{
//			//创建XML文档实例
//			XmlDocument xmlDoc = new XmlDocument();
//			//创建root节点，也就是最上一层节点
//			XmlElement root = xmlDoc.CreateElement("moses");
//				XmlElement elmnew = xmlDoc.CreateElement("clickxiangce");
//				elmnew.InnerText = "FALSE";
//			root.AppendChild(elmnew);
//			xmlDoc.AppendChild(root);
//			//把XML文件保存至本地
//			xmlDoc.Save(filepath);
//			//Debug.Log("Created!!!");
//			logstr = "OK";
//		}
//	}

	void UpdateXML(string path)
	{
		string filepath = path + "/StatesXML.xml";
		if (File.Exists (filepath)) 
		{
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(filepath);
			//得到transforms下的所有子节点
			XmlNodeList nodelist = xmldoc.SelectSingleNode("Moses").ChildNodes;
			foreach(XmlElement xe in nodelist)
			{
				if(xe.Name == "clickxiangce")
				{
					xe.InnerText = "TRUE";
				}
			}
			xmldoc.Save(filepath);
			logstr2 = "Writeok";
		}
	}
}
