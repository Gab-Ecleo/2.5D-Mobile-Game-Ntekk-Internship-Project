using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject imageContent;

    [SerializeField] private GameObject _scrollBar;
    [SerializeField] private float _scrollPos;
    [SerializeField] private float[] pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            _scrollPos = _scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if(_scrollPos < pos[i]+(distance / 2) && _scrollPos > pos[i] - (distance/2))
                {
                    _scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(_scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (_scrollPos < pos[i] + (distance / 2) && _scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f,1f), 0.1f);
                imageContent.transform.GetChild(i).localScale = Vector2.Lerp(imageContent.transform.GetChild(i).localScale, new Vector2(0.38f, 1.9f), 0.1f); // size of the circle when focused
                imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                for (int a = 0; a < pos.Length; a++)
                {
                    if(a != i)
                    {
                        imageContent.transform.GetChild(a).GetComponent<Image>().color = colors[0];
                        imageContent.transform.GetChild(a).localScale = Vector2.Lerp(imageContent.transform.GetChild(a).localScale, new Vector2(0.32f, 1.6f), 0.5f); // size of the circle when unfocused
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f,0.8f), 0.5f);
                    }
                }
            }
        }
    }
}
