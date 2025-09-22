using UnityEngine;

public interface IActivable
{
    public bool IsActive { get; set; }
    void Activate(bool isTrue);
}
