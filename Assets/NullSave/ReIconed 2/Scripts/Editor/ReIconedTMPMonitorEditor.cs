﻿using UnityEditor;

namespace NullSave
{

    [CustomEditor(typeof(ReIconedEditor))]
    public class ReIconedTMPMonitorEditor : CogEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("playerId");
            SimpleProperty("notFoundText");
            SimpleProperty("tint", "Sprite Tint");

            MainContainerEnd();
        }

        #endregion

    }
}