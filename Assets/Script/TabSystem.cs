using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    // UI elements for the buttons
    public FeatureButton _home;
    public FeatureButton _libary;
    public FeatureButton _share;

    // Panels corresponding to each tab
    public GameObject _homePanel;
    public GameObject _libaryPanel;
    public GameObject _sharePanel;

    // Color settings for active and inactive states (optional)
    public Color activeColor = Color.white;
    public Color inactiveColor = Color.gray;

    void Start()
    {
        // Assign listeners to each button
        _home.featureButton.onClick.AddListener(() => ActivateTab(1));
        _libary.featureButton.onClick.AddListener(() => ActivateTab(2));
        _share.featureButton.onClick.AddListener(() => ActivateTab(3));

        // Start by activating the first tab
        ActivateTab(1);
    }

    // Method to activate the tab and corresponding panel
    void ActivateTab(int tabNumber)
    {
        // Deactivate all panels initially
        _homePanel.SetActive(false);
        _libaryPanel.SetActive(false);
        _sharePanel.SetActive(false);

        // Reset all button colors to inactive
        _home.ClickedIcon.gameObject.SetActive(false);
        _libary.ClickedIcon.gameObject.SetActive(false);
        _share.ClickedIcon.gameObject.SetActive(false);

        // Activate the selected panel and set the button to active color
        switch (tabNumber)
        {
            case 1:
                _homePanel.SetActive(true);
                _home.ClickedIcon.gameObject.SetActive(true);
                break;
            case 2:
                _libaryPanel.SetActive(true);
                _libary.ClickedIcon.gameObject.SetActive(true);
                break;
            case 3:
                _sharePanel.SetActive(true);
                _share.ClickedIcon.gameObject.SetActive(true);
                break;
        }
    }
}

