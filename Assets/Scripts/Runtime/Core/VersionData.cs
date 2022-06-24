using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(fileName = "New VersionData", menuName = "Game/Version")]
    public class VersionData : ScriptableObject
    {
        [SerializeField] private int _major;
        [SerializeField] private int _minor;
        [SerializeField] private int _patch;
        [SerializeField] private string _hotfix;

        public override string ToString()
        {
            return $"v{_major}.{_minor}.{_patch}{_hotfix}";
        }
    }
}
