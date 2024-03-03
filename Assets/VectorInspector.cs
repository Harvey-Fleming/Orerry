using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class VectorInspector : MonoBehaviour
{
    [SerializeField] private MyVector3 m_Position;

    public MyVector3 Position { get => m_Position; set => m_Position = value; }
}


