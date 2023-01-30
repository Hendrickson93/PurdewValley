using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    //Constant variable for the total number of seconds in a day
    const float secondsInDay = 86400f;

    //Singleton instance of the DayTimeController class
    public static DayTimeController instance;

    //Serializable fields that can be set in the Unity editor
    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;

    //Variable to keep track of the time in the game
    float time;
    
    //Variable for the time scale
    [SerializeField] float timeScale = 60f;

    //UI text element to display the current time
    [SerializeField] Text text;
    
    //Global light to control the color of
    [SerializeField] UnityEngine.Rendering.Universal.Light2D globalLight;
    
    //Variable to count the number of days that have passed
    private int days;

    //Property to get the current time in hours
    float Hours
    {
        get { return time / 3600f; }
    }

  
    private void Update()
    {
        //Increment time based on delta time and time scale
        time += Time.deltaTime * timeScale;
        
        //Update the UI text element to display the current time in hours
        text.text = Hours.ToString();
        
        //Evaluate the color curve based on the current time
        float v = nightTimeCurve.Evaluate(Hours);
        
        //Lerp between the day and night light colors based on the color curve evaluation
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        
        //Set the global light color
        globalLight.color = c;
        
        //Check if the time has exceeded a full day
        if(time > secondsInDay)
        {
            //If it has, call the NextDay() method
            NextDay();
        }
    }

    //Method to progress to the next day
    private void NextDay()
    {
        //Reset the time to 0
        time = 0;
        //Increment the days variable
        days += 1;
    }
}
