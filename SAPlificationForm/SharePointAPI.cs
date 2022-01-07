using System;
using RestSharp;
using Newtonsoft.Json;
using static SAPlificationForm.ListResponse;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SAPlificationForm
{
    class SharePointAPI
    {
        private string accessToken = "";
        public bool valid = false;
        public SharePointAPI()
        {
            
            string[] data = GetTennantResource();
            accessToken = GetToken(data[0], data[1]);
        }

        // Returns the TennantID as pos[0] and ResourceID as pos[1]
        private string[] GetTennantResource()
        {
            string[] data = new string[2];

            var client = new RestClient("");		//REST Client webpage
            var request = new RestRequest(Method.GET);
            request.AddHeader("", "");		// Token - found using postman to send a request to sharepoint
            request.AddHeader("", "");
            request.AddHeader("", "");
            IRestResponse response = client.Execute(request);

            try
            {
                data[0] = TrimString(response.Headers[22].Value.ToString(), "", "");
                data[1] = TrimString(response.Headers[22].Value.ToString(), "", "");
            }
            catch
            {
                MessageBox.Show(response.StatusCode.ToString());
                MessageBox.Show(response.Content);
            }

            return data;
        }
		
		// return the correct string for access purposes
        private string TrimString(string input, string indexStart, string indexEnd)
        {
            int pStart = input.LastIndexOf(indexStart) + indexStart.Length;
            int pEnd = input.IndexOf(indexEnd);

            string output = input.Substring(pStart, pEnd - pStart);
            return output;
        }
		
		// gets access token by making a sharepoint request
        private string GetToken(string tennantID, string resourceID)
        {
            var client = new RestClient("");	//REST microsoft accounts access client
            var request = new RestRequest(Method.POST);
            request.AddHeader("", ""); 	// Token - found using postman to send a request to sharepoint
            request.AddHeader("", "");
            request.AddHeader("", "");
            request.AddParameter("", "" + tennantID + "" + resourceID + "" + tennantID, ParameterType.RequestBody);	// API parameter call data
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                TokenResponse tokenReponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                valid = true;
                return tokenReponse.access_token;
            }
            else
            {
                MessageBox.Show(response.StatusCode.ToString());
                MessageBox.Show(response.Content);
                return null;
            }
        }

		// Ensure that the connection was successful
        private bool TestStatus()
        {
            var client = new RestClient("");
            var request = new RestRequest(Method.GET);
            request.AddHeader("", "");
            request.AddHeader("", "");
            request.AddHeader("");
            request.AddHeader("", "" + accessToken);
            IRestResponse response = client.Execute(request);

            return response.StatusDescription != "OK" ? false : true;
        }

		// Access the list - specified by the ID, the index is the list ID
        public SAPJobDetails AccessListData(int index)
        {
            var client = new RestClient("" + index + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("", "");
            request.AddHeader("", "");
            request.AddHeader("", "");
            request.AddHeader("", "" + accessToken);
            IRestResponse response = client.Execute(request);
            
            if (!response.Content.Contains("":{\""))
            {
                GoodRoot goodResponse = JsonConvert.DeserializeObject<GoodRoot>(response.Content);
                return new SAPJobDetails(goodResponse);               
            }
            else
            {
                ErrorRoot errorResponse = JsonConvert.DeserializeObject<ErrorRoot>(response.Content);
                return null;
            }            
        }
    }
}
