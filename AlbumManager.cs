using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class AlbumManager : MonoBehaviour 
{
	public GameObject BackButton;
	
	private string logstr;
	private string logstr2;
	private UIButton backButton;

	// Use this for initialization
	void Start () 
	{
		backButton = BackButton.GetComponent<UIButton> ();

		#if UNITY_EDITOR   
		string filepath = Application.dataPath +"/StreamingAssets";   
		#elif UNITY_IPHONE   
		string filepath = Application.dataPath +"/Raw" + "/StatesXML.xml";   
		#elif UNITY_ANDROID   
		//string filepath = "jar:file://" + Application.dataPath + "!/assets" + "/StatesXML.xml";    
		string filepath = "/sdcard/mosesone";
		#endif
		//		StartCoroutine(ReadJsonData(filepath));
		//CreateXML (filepath);

		EventDelegate.Add(backButton.onClick,delegate() 
		{
			#if UNITY_ANDROID
			UpdateXML(filepath);
			#endif
		});
	}

	void CreateXML(string path)
	{
		Directory.CreateDirectory (path);
		string filepath = path + "/StatesXML.xml";
		if (!File.Exists (filepath)) 
		{
			//创建XML文档实例
			XmlDocument xmlDoc = new XmlDocument();
			//创建root节点，也就是最上一层节点
			XmlElement root = xmlDoc.CreateElement("moses");
			XmlElement elmnew = xmlDoc.CreateElement("clickxiangce");
			elmnew.InnerText = "FALSE";
			root.AppendChild(elmnew);
			xmlDoc.AppendChild(root);
			//把XML文件保存至本地
			xmlDoc.Save(filepath);
			//Debug.Log("Created!!!");
			logstr = "OK";
		}
	}
	
	void UpdateXML(string path)
	{
		string filepath = path + "/moses.xml";
		if (File.Exists (filepath)) 
		{
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(filepath);
			//得到transforms下的所有子节点
			XmlNodeList nodelist = xmldoc.SelectSingleNode("moses").ChildNodes;
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
