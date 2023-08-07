using Utils;

namespace Environment.TriggerBoxTypes
{
    /// <summary>
    /// Workaround, since MonoBehaviours do not allow generics. Hence, this pre-defines the type we pass
    /// as generic.
    /// </summary>
    public class TriggerBoxInteractable : TriggerBox<Interactable>
    {
        
    }
}