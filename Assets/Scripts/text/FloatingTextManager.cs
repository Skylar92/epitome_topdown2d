using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : Stateful
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> _floatingTexts = new();

    private void Update()
    {
        _floatingTexts.ForEach(text => text.Update());
    }

    public void Show(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        var floatingText = GetFloatingText();
        
        floatingText.text.text = message;
        floatingText.text.fontSize = fontSize;
        floatingText.text.color = color;
        
        floatingText.gameObject.transform.position = Camera.main.WorldToScreenPoint(position);

        floatingText.motion = motion;
        floatingText.duration = duration;
        
        floatingText.Show();
    }
    
    private FloatingText GetFloatingText()
    {
        var floatingText = _floatingTexts.Find(text => !text.active);
        if (floatingText == null)
        {
            floatingText = new FloatingText();
            floatingText.gameObject = Instantiate(textPrefab, textContainer.transform, true);
            floatingText.text = floatingText.gameObject.GetComponent<Text>();
            
            _floatingTexts.Add(floatingText);
        }
        return floatingText;
    }
}
