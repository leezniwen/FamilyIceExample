using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace family_icecream.Contents
{
	public class APIFunc
	{
		public string GetDataTest(string getName)
		{
			
			return "Get Its";
		}

		public  List<FamilyIce> GetData()
		{
			
			string url = "http://localhost:5000/Family/GetIceStore/all";
			var request = (HttpWebRequest)WebRequest.Create(url);
			var response = (HttpWebResponse)request.GetResponse();
			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
			if(response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				List<FamilyIce> result = JsonSerializer.Deserialize<List<FamilyIce>>(responseString);

                return result;
			}
			return null;
		}

		public List<string> GetAllStoreName()
		{
			List<string> str_result = new List<string> { };
            string url = "http://localhost:5000/Family/StoreNameList";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
				var option = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
                FamilyStoreDB result = JsonSerializer.Deserialize<FamilyStoreDB>(responseString , option);
				for (int i = 0; i < result.stores.Count; i++)
				{
					str_result.Add(result.stores[i].NAME);
				}
				return str_result;
            }
			return null;
		}

		public string SendStoreName(List<FamilyIce> Data)
		{
			string url = "http://localhost:5000/Family/AddIceFlaver";
			string jsonStr = JsonSerializer.Serialize(Data);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/json";

			using(var streamwriter = new StreamWriter(request.GetRequestStream()))
			{
				streamwriter.Write(jsonStr);
			}

			var response = (HttpWebResponse)request.GetResponse();
			string result = new StreamReader(response.GetResponseStream()).ReadToEnd();


            return result;
		}

	}
}

