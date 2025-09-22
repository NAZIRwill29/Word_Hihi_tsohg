using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;

public class InternetChecker : MonoBehaviour
{
    void Start()
    {
        if (CheckInternetConnection())
            Debug.Log("has internet connection");
        else
            Debug.Log("no internet connection");
    }

    public bool CheckInternetConnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Check if we can reach a website
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
