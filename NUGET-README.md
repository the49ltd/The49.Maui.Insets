## What is Maui.Insets?

Maui.Insets is a .NET MAUI library allowing developers to extend their application's content to the edge of the device and apply the window insets wherever they need to.

## Setup

Enable this plugin by calling `UseInsets()` in your `MauiProgram.cs`


```cs
using Maui.Insets;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		
		// Initialise the plugin
		builder
            .UseMauiApp<App>()
            .UseInsets();

		// the rest of your logic...
	}
}
```

### XAML usage

In order to make use of the plugin within XAML you can use this namespace:

```xml
xmlns:insets="https://schemas.the49.com/dotnet/2023/maui"
```


## API

This plugin offers a set of tools to help with device insets

### Attached Properties

The following properties are available to use:

Name          |  Type | Description |
:-------------------------|:-------------------------|:----
EdgeToEdge | `bool` | Expands the content of the page to fill the device's window
StatusBarStyle | `StatusBarStyle` | Sets the style of the status bar to fit the content drawn underneath. This is ignored if EdgeToEdge is not enabled

#### Example:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:insets="https://schemas.the49.com/dotnet/2023/maui"
             x:Class="The49.Maui.Insets.Sample.MainPage"
             Shell.NavBarIsVisible="False"
             insets:Insets.EdgeToEdge="True"
             insets:Insets.StatusBarStyle="LightContent"
             Title="Main Page">
             <!-- ... -->
</ContentPage>
```

### View model

A static view model is available under `Insets.Current` with the following properties:

Name          |  Type | Description |
:-------------------------|:-------------------------|:----
InsetsThickness | `Thickness` | Contains the insets for all sides of the window. This is 0 if EdgeToEdge is disabled for the current page
TopInsetThickness | `Thickness` | Contains the insets for the top of the window. This is 0 if EdgeToEdge is disabled for the current page
BottomInsetThickness | `Thickness` | Contains the insets for the bottom of the window. This is 0 if EdgeToEdge is disabled for the current page
DeviceInsetsThickness | `Thickness` | Contains the insets for all sides of the window. This will always contain the insets, even if EdgeToEdge is disabled

These properties react to window insets changes and to the `EdgeToEdge` page property, they can be used as bindings, adjusting your content to avoid overlapping with the status bar for example.

#### Example:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:insets="https://schemas.the49.com/dotnet/2023/maui"
             x:Class="The49.Maui.Insets.Sample.SalmonPage"
             insets:Insets.EdgeToEdge="True"
             Shell.NavBarIsVisible="False"
             Title="SalmonPage">
    <Grid RowDefinitions="Auto, Auto, *, Auto, Auto">
        <ContentView Background="Salmon" Padding="{Binding TopInsetThickness, Source={x:Static insets:Insets.Current}}">
            <Label Text="Main Page" Padding="16" FontSize="22" TextColor="White" HorizontalOptions="Center" />
        </ContentView>
        <Label Text="^^^ This view is padded to avoid colliding with the status bar ^^^" FontSize="12" HorizontalTextAlignment="Center" Grid.Row="1" />
        <Label Text="vvv This view is padded to avoid colliding with navigation bar vvv" FontSize="12" HorizontalTextAlignment="Center" Grid.Row="3" />
        <ContentView Background="Salmon" Padding="{Binding BottomInsetThickness, Source={x:Static insets:Insets.Current}}" Grid.Row="4">
            <Label Text="Footer" FontSize="22" TextColor="White" HorizontalOptions="Center" />
        </ContentView>
    </Grid>
</ContentPage>
```


### Views

3 views are available, matching the `InsetsThickness` `TopInsetThickness` and `BottomInsetThickness`. These are convenience views that are the equivalent of setting the padding to an insets property on a `ContentView`

The previous example can be simplified with the following for example:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:insets="https://schemas.the49.com/dotnet/2023/maui"
             x:Class="The49.Maui.Insets.Sample.SalmonPage"
             insets:Insets.EdgeToEdge="True"
             Shell.NavBarIsVisible="False"
             Title="SalmonPage">
    <Grid RowDefinitions="Auto, Auto, *, Auto, Auto">
        <insets:TopInsetView Background="Salmon">
            <Label Text="Main Page" Padding="16" FontSize="22" TextColor="White" HorizontalOptions="Center" />
        </insets:TopInsetView>
        <Label Text="^^^ This view is padded to avoid colliding with the status bar ^^^" FontSize="12" HorizontalTextAlignment="Center" Grid.Row="1" />
        <Label Text="vvv This view is padded to avoid colliding with navigation bar vvv" FontSize="12" HorizontalTextAlignment="Center" Grid.Row="3" />
        <insets:BottomInsetView Background="Salmon" Grid.Row="4">
            <Label Text="Footer" FontSize="22" TextColor="White" HorizontalOptions="Center" />
        </insets:BottomInsetView>
    </Grid>
</ContentPage>
```
## Issues

On iOS, there is en open issue with the `IgnoreSafeArea` propety of .NET MAUI's Layout views. This means that even with the edge-to-edge enabled, layouts inside the safe area will get a padding added (not a .NET MAUI padding, the bounds calculation at the iOS level is overriden). Unfortunately, there is no easy workaround that can be implemented in this library.

To help with this issue, the `bool` property `CancelIOSPadding` has been added. Set the property to true for all the layouts that are parents of your inset padding:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:insets="https://schemas.the49.com/dotnet/2023/maui"
             x:Class="The49.Maui.Insets.Sample.SalmonPage"
             insets:Insets.EdgeToEdge="True"
             Shell.NavBarIsVisible="False"
             Title="SalmonPage">
    <Grid RowDefinitions="Auto, Auto, *, Auto, Auto" insets:Insets.CancelIOSPadding="True">
        <insets:TopInsetView Background="Salmon">
            <Label Text="Main Page" Padding="16" FontSize="22" TextColor="White" HorizontalOptions="Center" />
        </insets:TopInsetView>
        <Label Text="^^^ This view is padded to avoid colliding with the status bar ^^^" FontSize="12" HorizontalTextAlignment="Center" Grid.Row="1" />
        <Label Text="vvv This view is padded to avoid colliding with navigation bar vvv" FontSize="12" HorizontalTextAlignment="Center" Grid.Row="3" />
        <insets:BottomInsetView Background="Salmon" Grid.Row="4">
            <Label Text="Footer" FontSize="22" TextColor="White" HorizontalOptions="Center" />
        </insets:BottomInsetView>
    </Grid>
</ContentPage>
```


---

Made within The49

