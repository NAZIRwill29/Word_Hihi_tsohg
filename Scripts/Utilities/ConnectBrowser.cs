using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectBrowser : MonoBehaviour
{
    //req delete user acc url
    public void ReqDelUseAccUrl(string playerId)
    {
        OpenUrl("mailto:wordnazservice@gmail.com?subject=Request%20Delete%20User%20Account&body=player%20id%20%3A%20"
         + playerId + "%0Atime%3A%20T" + System.DateTime.UtcNow.ToString("MM/dd/yyyy"));
    }

    //email support url
    public void EmailSupportUrl(string emailName)
    {
        OpenUrl("mailto:" + emailName + "?subject=Email%20Support");
    }

    //feedback url
    public void EamilReviewUrl(string emailName)
    {
        OpenUrl("mailto:" + emailName + "?subject=App%20Review");
    }

    //open url
    public void OpenUrl(string urlName)
    {
        try
        {
            //open url in browser
            Application.OpenURL(urlName);
        }
        catch (System.Exception e)
        {
            Debug.Log("error OpenUrl " + e);
        }

    }
}
