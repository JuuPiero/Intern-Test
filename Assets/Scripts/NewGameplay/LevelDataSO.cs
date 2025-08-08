using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewGameplay
{
    [CreateAssetMenu(fileName = "newLevelData", menuName = "Data/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Serializable]
        public struct BoardLayer
        {
            public int rows;
            public int columns;
        }

        [Header("Fixed Bottom Layer")]
        public BoardLayer bottomLayer;

        [Header("Random Layer Settings")]
        public int randomMinRows = 2;
        public int randomMaxRows = 6;
        public int randomMinColumns = 2;
        public int randomMaxColumns = 6;

        [Header("Gameplay Settings")]
        public int randomLayerCount = 2; // số layer random phía trên
        public int matchesMin = 3;
        public int maxPickedCell = 5;

        public float timeLimit = 60f; 

    }
}