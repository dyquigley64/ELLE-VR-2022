using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MillelleManager : MonoBehaviour
{
    private int sessionID;
    [SerializeField] GameMenu menu;
    private GameMode currentGameMode = GameMode.Quiz;
    int millenilleModuleID = 124;
    public Fader blackFader;
    // Dictionary<string, int> termIDs = new Dictionary<string, int>();
    List<Term> allTerms;
    List<Term> terms;
    // Start is called before the first frame update
    void Start()
    {
        sessionID = ELLEAPI.StartSession(millenilleModuleID, false);
 
        // termIDs.Add("deordorant", 768);
        // termIDs.Add("open book", 770);
        // termIDs.Add("guitar", 769);
        // termIDs.Add("stack of books", 771);
        // termIDs.Add("laptop", 772);
        // termIDs.Add("milk", 773);
        // termIDs.Add("tomato", 774);
        // termIDs.Add("apple", 775);
        // termIDs.Add("chips", 776);
        // termIDs.Add("orange juice", 777);
        // termIDs.Add("lemonade", 778);
        // termIDs.Add("cereal", 779);
        // termIDs.Add("cheese", 780);
        // termIDs.Add("steak", 781);
        // termIDs.Add("cupcake", 782);
        // termIDs.Add("strawberry cake", 783);
        // termIDs.Add("chocolate cake", 784);
        // termIDs.Add("blueberry cake", 785);

        allTerms = ELLEAPI.GetTermsFromModule(millenilleModuleID);

                Debug.Log("===========================");

        foreach (Term term in allTerms)
        {
            Debug.Log($"Term front: {term.front} Term back: {term.back} Term ID: {term.termID}");
        }

        Debug.Log("===========================");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TrimTerms(GameObject[] oNeeded)
    {
        bool found;
        terms = new List<Term>();
        foreach (Term term in allTerms)
        {
            found = false;
            foreach (GameObject obj in oNeeded)
            {
                if(obj == null) continue;
                if(obj.GetComponent<ObjectController>().objectName == term.back) found = true;
            }
            if(found)
                terms.Add(term);
        }
    }
    // public void RecordAnswer(string word, bool correct)
    // {
    //     Term temp = new Term();
    //     temp.termID = termIDs[word];
    //     ELLEAPI.LogAnswer(sessionID, temp, correct, currentGameMode == GameMode.Endless);
    // }
    public void EndGame(int percent, GameObject[] oNeeded, GameObject[] oGathered)
    {
        int attempts = oNeeded.Length;
        int score = (int)Math.Ceiling(((float)percent * (float)attempts) / 100f);
        // HashSet<string> gatheredObjectsSet = new HashSet<string>();
        bool found;
        foreach (Term term in terms)
        {
            found = false;
            foreach (GameObject obj in oGathered)
            {
                if(obj == null) continue;
                if(obj.GetComponent<ObjectController>().objectName == term.back) found = true;
            }
            ELLEAPI.LogAnswer(sessionID, term, found, currentGameMode == GameMode.Endless);
            // gatheredObjectsSet.Add(obj.GetComponent<ObjectController>().objectName);
        }
        // Debug.Log("===========================");
        // Debug.Log(gatheredObjectsSet.Count);
        // Debug.Log("===========================");
        // foreach (GameObject obj in oNeeded)
        // {
        //     string temp = obj.GetComponent<ObjectController>().objectName;
        //     if(gatheredObjectsSet.Contains(temp))
        //     {
        //         RecordAnswer(temp, true);
        //         gatheredObjectsSet.Remove(temp);
        //     }
        // }
        // foreach (string str in gatheredObjectsSet)
        // {
        //     RecordAnswer(str, false);
        // }
        //menu.EndGame(score, attempts);
        
        ELLEAPI.EndSession(sessionID, percent);
        StartCoroutine(sceneChange());
    }

    private IEnumerator sceneChange()
    {
        blackFader.Fade(true, 1f);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Bus");
    }
}
