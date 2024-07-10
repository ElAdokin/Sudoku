using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour, IFader
{
    private Optional<IEnumerator> _coroutine = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;

    private Image _fader;

    private float _fadeTime = 0;
    private float _frameTime;
    private float _alphaAmount;

    private Yielders _yielders = new Yielders();

    public event Action<bool, bool> OnFadeEnd;

    private bool _fadeEnd;
    private bool _isFadeIn;

    private void Awake()
    {
        _fader = GetComponent<Image>();
        _frameTime = 1f / Application.targetFrameRate;
    }
    
    public void SetFade(bool isFadeIn, float duration)
    {
        if (_coroutine.IfPresent()) StopBehaviour();

        if(_fader == null)
            _fader = GetComponent<Image>();

        _frameTime = 1f / Application.targetFrameRate;

        _fadeTime = 0;
        _fadeEnd = false;

        _isFadeIn = isFadeIn;

        if (_isFadeIn)
            _coroutine = new Optional<IEnumerator>(FadeIn(duration));
        else
            _coroutine = new Optional<IEnumerator>(FadeOut(duration));

        _coroutineController = new CoroutineController(_coroutine, this);
        _coroutineController.StartCurrentCoroutine();
    }

    private IEnumerator FadeIn(float duration)
    {
        Debug.Log("Fade In");

        _fader.color = Color.white;
        _fader.raycastTarget = true;

        _alphaAmount = 1;

        while (_fadeTime < duration)
        {
            _fadeTime += _frameTime;
            _alphaAmount -= _frameTime / duration;
            _fader.color = new Color(1, 1, 1, _alphaAmount);
            
            yield return _yielders.EndOfFrame;
        }

        _fader.color = new Color(1, 1, 1, 0);
        _fader.raycastTarget = false;

        _fadeEnd = true;
        Notify();

        StopBehaviour();
        yield break;
    }

    public IEnumerator FadeOut(float duration)
    {
        Debug.Log("Fade Out");

        _fader.raycastTarget = true;

        _fader.color = new Color(1, 1, 1, 0);
        _alphaAmount = 0;

        while (_fadeTime < duration)
        {
            _fadeTime += _frameTime;
            _alphaAmount += _frameTime / duration;
            _fader.color = new Color(1, 1, 1, _alphaAmount);
            
            yield return _yielders.EndOfFrame;
        }

        _fader.color = Color.white;
        _fader.raycastTarget = true;

        _fadeEnd = true;
        Notify();

        StopBehaviour();
        yield break;
    }

    private void StopBehaviour()
    {
        if (_coroutine.IfPresent())
        {
            _coroutineController.StopCurrentCoroutine();
            _coroutine = new Optional<IEnumerator>();
        }
    }

    private void Notify()
    {
        OnFadeEnd?.Invoke(_isFadeIn, _fadeEnd);
    }
}
