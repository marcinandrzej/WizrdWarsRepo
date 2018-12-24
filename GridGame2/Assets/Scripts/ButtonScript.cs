using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private int x;
    private int y;
    private int colorIndex;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public int ColorIndex
    {
        get
        {
            return colorIndex;
        }

        set
        {
            colorIndex = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetIndexes(int _x, int _y)
    {
        X = _x;
        Y = _y;
    }

    public void Move(int _x, int _y)
    {
        float tempX = (_x + 0.5f) * Mathf.Abs(transform.GetComponent<RectTransform>().sizeDelta.x);
        float tempY = (_y + 0.5f) * (-Mathf.Abs(transform.GetComponent<RectTransform>().sizeDelta.y));
        Vector2 dest = new Vector2(tempX, tempY);
        StartCoroutine(MoveCoroutine(dest));
    }

    public IEnumerator MoveCoroutine(Vector2 destination)
    {
        while (gameObject.GetComponent<RectTransform>().anchoredPosition != destination)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(
                gameObject.GetComponent<RectTransform>().anchoredPosition, destination, 300.0f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void DestroyBut()
    {
        transform.GetComponent<Button>().enabled = false;
        StartCoroutine(DestroyCoroutine());
    }

    public IEnumerator DestroyCoroutine()
    {
        for (int i = 20; i > 1; i--)
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(i * 0.05f, i * 0.05f, i * 0.05f);
            transform.GetComponent<RectTransform>().Rotate(new Vector3(0,0,15.0f));
            yield return new WaitForSeconds(0.025f);
        }
        Destroy(gameObject);
    }
}
