using System.Collections;
using UnityEngine;

public class Profiler : MonoBehaviour
{
    private float _accum; // FPS accumulated over the interval
    public bool AllowDrag = true; // Do you want to allow the dragging of the FPS window
    private Color _color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
    private int _frames; // Frames drawn over the interval
    public float Frequency = 0.5F; // The update frequency of the fps
    public int NbDecimal = 1; // How many decimal do you want to display
    private string _sFps = ""; // The fps formatted into a string.
    public Rect StartRect = new Rect(5, 5, 75, 30); // The rect the window is initially displayed at.
    private GUIStyle _style; // The style the text will be displayed at, based en defaultSkin.label.
    public bool UpdateColor = true; // Do you want the color to change if the FPS gets low

    private void Start()
    {
        StartCoroutine(Fps());
    }

    private void Update()
    {
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;
    }

    private IEnumerator Fps()
    {
        // Infinite loop executed every "frenquency" secondes.
        while (true)
        {
            // Update the FPS
            var fps = _accum / _frames;
            _sFps = fps.ToString("f" + Mathf.Clamp(NbDecimal, 0, 10));

            //Update the color
            _color = fps >= 30 ? Color.green : (fps > 10 ? Color.red : Color.yellow);

            _accum = 0.0F;
            _frames = 0;

            yield return new WaitForSeconds(Frequency);
        }
    }

    private void OnGUI()
    {
        // Copy the default label skin, change the color and the alignement
        if (_style == null)
        {
            _style = new GUIStyle(GUI.skin.label);
            _style.normal.textColor = Color.white;
            _style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = UpdateColor ? _color : Color.white;
        StartRect = GUI.Window(0, StartRect, DoMyWindow, "");
    }

    private void DoMyWindow(int windowId)
    {
        var label = _sFps + " FPS";

        GUI.Label(new Rect(0f,0f, StartRect.width, StartRect.height), label, _style);
        if (AllowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
}