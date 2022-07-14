using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
        
public class ScenePortal : MonoBehaviour
{
    public Animator animator;
    public string targetScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player") {
                StartCoroutine(LoadScene(targetScene));
            }
            animator.SetTrigger("start");

            
        }



    IEnumerator LoadScene(string s)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(s);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
