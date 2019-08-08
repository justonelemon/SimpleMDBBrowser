using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public struct SearchResults
{
    [System.Serializable]
    public struct MovieContents
    {
        public string Title;
        public string Year;
        public string imdbID;
        public string Type;
        public string Poster;
    }

    public List<MovieContents> Search;
}

public class MovieSearchManager : MonoBehaviour
{
    [SerializeField]
    GameObject emptyMoviePanel;
    [SerializeField]
    Transform contentPanel;

    List<MoviePanel> moviePanels;

    InputField _inputField;

    //http://www.omdbapi.com/?apikey=6367da97&s=sun

    private void Awake()
    {
        moviePanels = new List<MoviePanel>();
    }

    private void Start()
    {
        _inputField = GetComponent<InputField>();
    }

    public void DoSearch()
    {
        DoSearch("http://www.omdbapi.com/?apikey=6367da97&s=" + _inputField.text);
    }

    public void DoSearch(string _searchURL)
    {
        StartCoroutine(DownloadJsonSearchResult(_searchURL));
    }

    IEnumerator DownloadJsonSearchResult(string _searchURL)
    {
        UnityWebRequest www = UnityWebRequest.Get(_searchURL);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            ClearMoviePanels();
            CreateMoviePanels(JsonUtility.FromJson<SearchResults>(www.downloadHandler.text));
        }
    }

    void CreateMoviePanels(SearchResults _movieList)
    {
        foreach(SearchResults.MovieContents _movieContent in _movieList.Search)
        {
            GameObject _newMovieListing = Instantiate(emptyMoviePanel, contentPanel);
            MoviePanel _newMoviePanel = _newMovieListing.GetComponent<MoviePanel>();

            _newMoviePanel.SetTitle(_movieContent.Title);
            _newMoviePanel.SetImagePoster(_movieContent.Poster);

            moviePanels.Add(_newMoviePanel);

        }
    }

    void ClearMoviePanels()
    {
        while (moviePanels.Count > 0)
        {
            Destroy(moviePanels[0]);
        }

        moviePanels.Clear();
    }
}
