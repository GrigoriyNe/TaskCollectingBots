using UnityEngine;

public class PanelRotator : MonoBehaviour
{
    private GameObject _canvas;

    void OnEnable()
    {
        _canvas = GameObject.Find("Canvas").gameObject;
    }

    void Update()
    {
        _canvas.transform.rotation = Camera.main.transform.rotation;
    }
}