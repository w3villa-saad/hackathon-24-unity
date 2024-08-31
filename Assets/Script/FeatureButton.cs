using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FeatureButton : MonoBehaviour
{
    [SerializeField] Button _button;

    public Button featureButton { get { return _button; } }
    [SerializeField] Image _clickedIcon;

    public Image ClickedIcon { get { return _clickedIcon; } }

}
