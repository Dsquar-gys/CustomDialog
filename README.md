# Custom Dialog
Custom Dialog is a dialog that can provide extra custom logic to files reading.
Core sense included in `ISpecificFileViewModel` implementation and injecting it to View Model. Otherwise it is regular dialog.
Library already contains **Views** and **View Models**
## Tech
- .NET Core
- [Avalonia UI](https://avaloniaui.net/)
- [Semi.Avalonia](https://github.com/irihitech/Semi.Avalonia)
- [Reactive UI](https://www.reactiveui.net/)
- [DynamicData](https://github.com/reactivemarbles/DynamicData)

## Usage
All **View Models** have their own **Views** inside library, so if you are not going to implement own views see **View Locator**
### GeneralViewModel
GeneralViewModel is main view model, that shold be created only once. Creation is simple:
```cs
public GeneralViewModel MainViewModel => new(); // with default ISpecificFileViewModel
```
or
```cs
public class PhotoFileSender : ISpecificFileViewModel { ... }
    
public GeneralViewModel MainViewModel => new(new PhotoFileSender()); // with custom ISpecificFileViewModel
```
To use `GeneralViewModel` in xml:
```xml
<TransitioningContentControl Content="{Binding MainViewModel}" />
```

### View Locator
If you are using `ViewLocator` and not goint to create own views, refactor code like this:
```cs
public Control? Build(object? data)
    {
        ...
        var type = data.GetType().Assembly.GetType(name);
        ...
    }
```
That is necessary for your project to see library's views.
