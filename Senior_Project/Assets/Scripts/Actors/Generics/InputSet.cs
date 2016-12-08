using UnityEngine;
/// <summary>
/// Classes for representing input packets
/// </summary>
public abstract class InputSet {
    public Actor actor;
    public Input source;
    /// <summary>
    /// stores directional inputs
    /// </summary>
    public System.IComparable DirectionalIn;
    /// <summary>
    /// stores command inputs
    /// </summary>
    public System.IComparable[] CommandIn;
    /// <summary>
    /// stores any priority/urgent commands
    /// </summary>
    public System.IComparable Overrule;
}
