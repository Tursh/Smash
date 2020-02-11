using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace GitHub.Unity
{
    [InitializeOnLoad]
    public class UnityApiWrapper : ScriptableSingleton<UnityApiWrapper>
    {
        static UnityApiWrapper()
        {
#if UNITY_2018_2_OR_NEWER
            Editor.finishedDefaultHeaderGUI += editor => {
                UnityShim.Raise_Editor_finishedDefaultHeaderGUI(editor);
            };
#endif
        }
    }
}