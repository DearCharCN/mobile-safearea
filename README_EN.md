# mobile-safearea [查看中文 README](README.md)
This is a Unity plugin that helps determine if the current device has a notch and provides information about the notch's height and other details.

## Features
1. Detects if the device has a notch and retrieves the notch height.
2. Supports Android and iOS (HarmonyOS support coming soon).
3. Supports foldable screens (sends notifications when resolution changes).

## Unity Component
The plugin provides a Unity component: SafeAreaComponent.
On devices with a notch, this component automatically adjusts its size.
You can place UI content that should not be obscured under this component’s child nodes.

![](Media~/1.png)

## Code Usage
### Get Notch Height
```csharp
// The height returned is a value between 0.0 and 1.0, representing the proportion of the screen occupied by the notch.
float unit = SafeAreaUtls.GetTopOffsetUnit();
```

### Listen for Resolution Changes
```csharp
private void Start()
{
    sb = new StringBuilder();
    SafeAreaUtls.AddResolutionChanged(OnResolutionChanged);
}

private void OnDestroy()
{
    SafeAreaUtls.RemoveResolutionChanged(OnResolutionChanged);
}

// The callback method runs on Unity's main thread.
void ResolutionChanged()
{
    Debug.LogError("Resolution has Changed");
}
```

