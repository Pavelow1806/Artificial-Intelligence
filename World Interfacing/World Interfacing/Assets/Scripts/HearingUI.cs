using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HearingUI : MonoBehaviour
{
    public GameObject HearingSphere;

    private EnemyHearing _hearing;

    private Text _uiText;
    StringBuilder _hearingText = new StringBuilder(string.Empty);

    // Use this for initialization
    void Start()
    {
        _hearing = HearingSphere.GetComponent<EnemyHearing>();
        _uiText = GetComponent<Text>();

        _uiText.text = string.Empty;
    }

    // Update is called once per frame
    void OnGUI()
    {
        _hearingText.Length = 0;
        _hearingText.Capacity = 0;

        _hearingText.Append("Hearing threshold: ");
        _hearingText.Append(_hearing.HearingThreshold);
        _hearingText.Append(", Noise: ");
        _hearingText.Append(_hearing.Noise);
        _hearingText.Append("  -  ");

        if (_hearing.HearingToggle)
        {
            if (_hearing.PlayerHeard)
            {
                _hearingText.Append("I can hear you");
            }
            else
            {
                _hearingText.Append("It's too quiet");
            }
        }
        else
        {
            _hearingText.Append("Hearing disabled");
        }

        _uiText.text = _hearingText.ToString();
    }
}
