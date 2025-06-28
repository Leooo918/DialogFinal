using System;
using UnityEngine;


namespace Dialog.Animation
{
    [Serializable]
    public class CharacterData
    {
        public bool isVisible;
        public float timer;

        public MeshData source;
        public MeshData current;

        public CharacterData()
        {
            isVisible = false;
            timer = 0;

            source.positions = new Vector3[4];
            source.colors = new Color32[4];

            current.positions = new Vector3[4];
            current.colors = new Color32[4];
        }
    }

    [Serializable]
    public struct MeshData
    {
        public Vector3[] positions;
        public Color32[] colors;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < positions.Length; i++)
            {
                sb.Append(positions[i].ToString());
                sb.Append(" ");
                sb.Append(colors[i].ToString());
                sb.Append(" - ");
            }
            return sb.ToString();
        }
    }
}
