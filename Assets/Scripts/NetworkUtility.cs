using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetworkUtility : MonoBehaviour {


    public static string IPAddress;
    // Util method for sending post request
    public static WWW SendPostRequest(string json, string apiRoute)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] pData = Encoding.ASCII.GetBytes(json.ToCharArray());

        return new WWW("http://" + IPAddress + ":3000" + apiRoute, pData, headers);
    }
}
