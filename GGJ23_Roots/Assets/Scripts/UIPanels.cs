using System.Collections;
using UnityEngine;

public class UIPanels : MonoBehaviour
{
    [SerializeField] private CanvasGroup _whitePanel, _blackPanel;
    
    public void RevealWhitePanel(float time = 0)
        => StartCoroutine(RevealingWhitePanel(time));

    public void HideWhitePanel(float time = 0)
        => StartCoroutine(HidingWhitePanel(time));

    public void RevealBlackPanel(float time = 0)
        => StartCoroutine(RevealingBlackPanel(time));

    public void HideBlackPanel(float time = 0)
        => StartCoroutine(HidingBlackPanel(time));

    private IEnumerator RevealingWhitePanel(float time)
    {
        if (time == 0) _whitePanel.alpha = 1;

        else 
        {
            float elapsedTime = 0;

            while (_whitePanel.alpha < 1)
            {
                _whitePanel.alpha = Mathf.Lerp(0, 1, elapsedTime / time);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator HidingWhitePanel(float time)
    {
        if (time == 0) _whitePanel.alpha = 0;

        else 
        {
            float elapsedTime = 0;

            while (_whitePanel.alpha > 0)
            {
                _whitePanel.alpha = Mathf.Lerp(1, 0, elapsedTime / time);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator RevealingBlackPanel(float time)
    {
        if (time == 0) _blackPanel.alpha = 1;

        else 
        {
            float elapsedTime = 0;

            while (_blackPanel.alpha < 1)
            {
                _blackPanel.alpha = Mathf.Lerp(0, 1, elapsedTime / time);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator HidingBlackPanel(float time)
    {
        if (time == 0) _blackPanel.alpha = 0;

        else 
        {
            float elapsedTime = 0;

            while (_blackPanel.alpha > 0)
            {
                _blackPanel.alpha = Mathf.Lerp(1, 0, elapsedTime / time);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
