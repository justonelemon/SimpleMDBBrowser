using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MoviePanel : MonoBehaviour
{

    [SerializeField]
    Text title;
    [SerializeField]
    Image imagePoster;

    public void SetTitle(string _title)
    {
        title.text = _title;
    }

    public void SetImagePoster(string _textureURL)
    {
        StartCoroutine(DownloadMoviePoster(_textureURL));
    }

    IEnumerator DownloadMoviePoster(string _textureURL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_textureURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            imagePoster.sprite = Sprite.Create(downloadedTexture as Texture2D, new Rect(0, 0, downloadedTexture.width, downloadedTexture.height), new Vector2(0, 0));
        }
    }
}