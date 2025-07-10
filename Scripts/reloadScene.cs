using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reloadScene : MonoBehaviour
{
    public void reload()
    {
        GameObject scene = GameObject.FindGameObjectWithTag("sceneHolder");

        Destroy(scene);
        int y = SceneManager.sceneCount;
        SceneManager.LoadScene("gameStuff", LoadSceneMode.Additive);
        
        SceneManager.UnloadSceneAsync(y-1);

    }

}
