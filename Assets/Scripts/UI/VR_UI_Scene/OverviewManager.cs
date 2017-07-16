﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// INSERT CLASS NAME. 
/// This is the long description separated from the short description with a .
/// </summary>
public class OverviewManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    #endregion

    #region PRIVATE_MEMBER_VARIABLES
    /// <summary>
    /// graph displaying a heartbeat, linked to the screen
    /// </summary>
    private GraphObject heart;
    /// <summary>
    /// screen of this mode
    /// </summary>
    [SerializeField]
    private Canvas screen;

    /// <summary>
    /// link to tabs on screen
    /// </summary>
    [SerializeField]
    private GameObject[] tabs;
    /// <summary>
    /// for continuously testing graphrenderer
    /// </summary>
    private bool testing = false;
    /* For test purposes to adjust heartbeat
     *  #region heartbeat values
     [SerializeField]
     private float a= 0.2f;
     [SerializeField]
     private float d= 1.4f;
     [SerializeField]
     private float h= 3;
     [SerializeField]
     private float s= 0.05f;
     [SerializeField]
     private float w= 0.02f;
     #endregion*/
    #endregion

    #region UNITY_MONOBEHAVIOUR_METHODS
    /// <summary>
    /// Called once by Unity during startup
    /// </summary>
    void Awake()
    {

        GameObject obj = new GameObject();
        obj.name = "heartbeat";
        obj.transform.parent = tabs[1].transform.Find("Panel"); //linked to screen
        tabs[0].GetComponentInChildren<Button>().onClick.Invoke(); //set tab 0 as default display
        /*Working well for gameobject to fit parent*/
        AspectRatioFitter test = obj.AddComponent<AspectRatioFitter>();
        test.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        test.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        test.transform.localPosition = Vector3.zero;

        //add graph
        obj.AddComponent<GraphObject>();
        heart = obj.GetComponent<GraphObject>();
        heart.Initialize(null, 3);
        heart.SetDefaultValue(2);
        heart.SetNumberOfPoints(200);
        heart.SetNoAdjustment();
        heart.SetManualAdjust(new Vector2(0, 4));
        heart.SetGraphColour(Color.green);
        heart.Play();
    }

    /// <summary>
    /// do stuff as soon as enabled again
    /// </summary>
    void OnEnable()
    {
        if (heart)
        {
            heart.Resume();
        }
    }
    void OnDisable()
    {
        if (heart)
        {
            heart.Pause();
        }
    }
    /// <summary>
    /// Called once every frame
    /// </summary>
    void Update()
    {
        //test graph renderer behaviour
        if (!testing)
        {
            testing = true;
            StartCoroutine(test());
            Debug.Log("new test"); 
        }
        heart.AddValue(GetBeat());
        //this comment counts frames per sec.... some issues here. this update is only called 10 times per sec 
        // (might be hardware related...)
        //Debug.Log("heart update: " + (int)Time.time);
    }
    #endregion

    #region PUBLIC_METHODS

    #endregion

    #region PRIVATE_METHODS
    /// <summary>
    /// coroutine to test graph pause and resume functions / behaviour. pauses and restarts after short amount of waiting time
    /// </summary>
    /// <returns></returns>
    private IEnumerator test()
    {
        yield return new WaitForSeconds(3);
        if (heart.isActiveAndEnabled)
        {
            heart.DisplayForNumberOfSeconds(1);
            heart.SetGraphColour(Color.gray);
        }
        yield return new WaitForSeconds(3);
        if (heart.isActiveAndEnabled)
        {
            heart.DisplayForNumberOfSeconds(6);
            heart.SetGraphColour(Color.cyan);
        }
        yield return new WaitForSeconds(5);
        if (heart.isActiveAndEnabled)
        {
            heart.Pause();
        }
        yield return new WaitForSeconds(3);
        if (heart.isActiveAndEnabled)
        {
            heart.SetManualAdjust(-5, 11);
            heart.Resume();
        }
        //TODO: buggy! way to expensive looking for min/max values in each frame
        //yield return new WaitForSeconds(3);
        //if (heart.isActiveAndEnabled)
        //{
        //    //heart.SetAutomaticAdjust();
        //}
        testing = false;
    }


    /// <summary>
    /// returns heartbeat value of current time step
    /// </summary>
    /// <returns></returns>
    private float GetBeat()
    {

        //create heartbeat function
        float L = 3f; // factor when to loop function again
        float x = Time.time;
        x = x - Mathf.Ceil(x / L - 0.5f) * L;
        /* a lot of adjusting parameters */
        float a = 2f;
        float d = 0.4f;
        float h = 2;
        float s = 0.05f;
        float w = 0.03f;
        float e = Mathf.Exp(1);

        if (L < 2 * d) // must be met
        {
            Debug.Log("some issues with heartbeat period");
        }

        float y_1 = a * Mathf.Pow(e, (-Mathf.Pow(x + d, 2) / (2 * w)));
        float y_2 = Mathf.Pow(e, (-Mathf.Pow(x - d, 2) / (2 * w)));
        float y_3 = h - (Mathf.Abs(x / s) - x) * Mathf.Pow(e, (-Mathf.Pow(7 * x, 2) / 2));
        float y = y_1 + y_2 + y_3;

        return y;

    }
    #endregion
}

