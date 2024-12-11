using System;
using _Project.Scripts.LogicModule;
using _Project.Scripts.LogicModule.Views;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private Graphic _graphic;
    [SerializeField] private PooledView _pooled;
    
    [SerializeField] private float _distance;
    [SerializeField] private float _duration;
    
    private void OnEnable()
    {
        var color = _graphic.color;
        color.a = 1f;
        _graphic.color = color;

        Tween.Alpha(_graphic, 0f, _duration);
        Tween.LocalPositionY(transform, _distance, _duration)
            .OnComplete(EndAnimation);
        
    }

    private void EndAnimation()
    {
        if (_pooled != null)
            _pooled.ReturnToPool();
    }
}
