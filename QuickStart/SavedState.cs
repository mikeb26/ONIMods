// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using KSerialization;

namespace QuickStart;

[SerializationConfig(MemberSerialization.OptIn)]
public class SavedState : KMonoBehaviour, ISaveLoadable
{
    [Serialize]
    public bool GameWasModified;

    [Serialize]
    public string version;
    
    [Serialize]
    public QuickStartOptions opts;
}
