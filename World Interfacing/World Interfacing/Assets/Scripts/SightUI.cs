using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SightUI : MonoBehaviour
{
    public GameObject SightCone;

    private EnemySights _sight;

    private Text _uiText;
    private StringBuilder _seeingText = new StringBuilder(string.Empty);

    // Use this for initialization
    void Start ()
    {
        _sight = SightCone.GetComponent<EnemySights>();
        _uiText = GetComponent<Text>();

        _uiText.text = string.Empty;
    }
	
	void OnGUI()
	{
	    _seeingText.Length = 0;
	    _seeingText.Capacity = 0;

        _seeingText.Append("Last seen: ");
	    _seeingText.Append(_sight.LastSeenPosition.ToString());
	    _seeingText.Append("  -  ");


        if (_sight.VisionToggle)
	    {
	        if (_sight.PlayerInSight)
	        {
	            _seeingText.Append("I can see you");
	        }
	        else
	        {
	            _seeingText.Append("Where are you?");
	        }
	    }
	    else
	    {
	        _seeingText.Append("Vision disabled");
	    }
        _uiText.text = _seeingText.ToString();
    }
}
