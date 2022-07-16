using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject blackOverlay;

    private void Awake()
    {
        StartCoroutine(LoadGameOperation());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadGameOperation()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (blackOverlay.GetComponent<CanvasGroup>().alpha > 0)
        {
            blackOverlay.GetComponent<CanvasGroup>().alpha -= .1f;
            yield return null;
        }
        blackOverlay.SetActive(false);
    }

    [ContextMenu("Unload main menu")]
    public void StartGame()
    {
        FindObjectOfType<PlayerManager>().PlayGame();
        SceneManager.UnloadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
