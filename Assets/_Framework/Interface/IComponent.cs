using UnityEngine;
using System.Collections;

public interface IComponent : IUpdate, IDispose {
    void Init();
    void FixedUpdate();
}
