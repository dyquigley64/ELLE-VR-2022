using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseMenu : MonoBehaviour
{
    private GameObject menu, resumeButton;

    public HellesKitchenManager hellesKitchenManager;

    //public Fader fader;
    public static bool paused, canPause;

    void Start()
    {
        menu = transform.GetChild(0).gameObject;
        resumeButton = menu.transform.GetChild(1).gameObject;
    }
    void Update()
    {
        
        if (VRInput.rightStickClickDown)
        {
            if (!paused && canPause)
            {
            
                hellesKitchenManager.LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
                hellesKitchenManager.XRRig.transform.position = new Vector3(7.81f, 0, -1.399f);
                hellesKitchenManager.XRRig.transform.rotation = hellesKitchenManager.initialPlayerRotation;


                Pause();
            } 
            else Resume();
        }
        //if(paused)
        //{
           // Debug.Log("CURRENT EVENT THING: "+ EventSystem.current.currentSelectedGameObject);
            //EventSystem.current.SetSelectedGameObject(resumeButton);
       // }

        if (paused && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    
    public void Pause()
    {
        //fader.Fade(true, 1, true);
        Time.timeScale = 0;
        paused = true;
        menu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        //fader.Fade(false);
        hellesKitchenManager.LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        paused = false;
        menu.SetActive(false);
    }

    public void Restart ()
    {
        Time.timeScale = 1;
        hellesKitchenManager.LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        hellesKitchenManager.LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        paused = false;
        SceneManager.LoadScene("Hubworld");
    }
}
