using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnabledScript : MonoBehaviour
{
    public bool tutorialEnabled;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        tutorialEnabled = true;
    }

    public void toggleTutorial()
    {
        tutorialEnabled = !tutorialEnabled;
    }
}
