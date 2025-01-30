/*kjthao*/
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
// note that we dont need the monobehavior here

public class SoundSettingsTestA
{
    /*
        this unit test is only testing the logic of the slider and how it behaves
    */
    private Slider volumeSlider;

    [SetUp] // reminder tht these attributes/methods needs to be properly marked with the []!
    public void SetUp()
    {
        var go = new GameObject("VolumeSlider");
        volumeSlider = go.AddComponent<Slider>();
        volumeSlider.minValue = 0f; // setting up the ranges (0 to 1)
        volumeSlider.maxValue = 1f;
    }

    [Test]
    public void TestSliderValueChanges()
    {
        // note: this is testign if the slider is changing/responding to it. (going up and down)

        volumeSlider.value = 0.5f; // set the value to .5 (which is halfway)
        Assert.AreEqual(0.5f, volumeSlider.value);
        volumeSlider.value = 0.9f; // then it will chnge to .9 to see if the volume got louder/increased correctly.
        Assert.AreEqual(0.9f, volumeSlider.value);
    }

    public void TestSliderMinMaxValue()
    {
        // note: this checks if the slider is staying within the min and max range. (We dont want it to go over)

        volumeSlider.value = 1.5f; // setting it a lil over 1.0 (which is the max)
        Assert.AreEqual(1f, volumeSlider.value);
        volumeSlider.value = -0.5f; // setting it a lil below 0f (which is the min)
        Assert.AreEqual(0f, volumeSlider.value);
    }

}

