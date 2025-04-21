using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}