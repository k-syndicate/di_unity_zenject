using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour 
{
    public float Duration = 1f;
    public float Speed;

    private TextMesh textMesh;
    private float startTime;

	void Awake () 
    {
        textMesh = GetComponent<TextMesh>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Time.time - startTime < Duration)
        {
            //Scroll up
            transform.LookAt(Camera.main.transform);
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        else
        {
            //Destroy
            Destroy(gameObject);
        }
	}

    public void SetText(string text)
    {
        textMesh.text = text;    
    }

    public void SetColor(Color color)
    {
        textMesh.color = color;
    }
}
