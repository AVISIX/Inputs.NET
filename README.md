
# Inputs.NET
Inputs.NET is an easy-to-use, extendible, free and open-source library for the .NET ecosystem. It implements several methods to manipulate the mouse and keyboard on the windows operating system. 

If you like this project and want to support further development, please consider leaving a star.

**So far only tested on Windows 11!**

## "Why though?"
After getting annoyed about how tedious it can be to reliably and securely manipulate the mouse & keyboard on windows, I decided to take this matter into my own hands. Hence why I created this project.  

I originally wrote this for myself, but after seeing how useful it could be for others, I decided to make it public.

## Features

**For detailed documentation, see the [Wiki](https://github.com/AVISIX/Inputs.NET/wiki)!**

#### Mouse manipulation
- [mouse_event](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event)
- NtUserSendInput (*undocumented*)  
- NtUserInjectMouseInput (*undocumented*)
- [ddxoft virtual mouse driver](https://github.com/ddxoft/master)

#### Keyboard manipulation
- [keybd_event](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-keybd_event)
- NtUserSendInput (*undocumented*)
- NtUserInjectKeyboardInput (*undocumented*)
- [ddxoft virtual keyboard driver](https://github.com/ddxoft/master)

#### Other features
- Keyboard &  mouse hooks using the [WinApi](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa)
- [Keyboard-macro recorder & player-classes](https://github.com/AVISIX/Inputs.NET/tree/main/src/Inputs/Macros)
- Automatic keyboard & mouse call-spoofer (*executing process only*)
- WinApi call-spoofer

#### Planned features
- Add [Interception driver](https://github.com/oblitum/Interception) support
- Add mouse easings support
- *Search for and add more methods.*

## Supported Runtimes
- .NET Framework 4.7.1
- .NET Framework 4.7.2
- .NET Framework 4.8
- .NET Core 3.1
- .NET 5
- .NET 6

## Example usage
To use this library, you may go to the Release-page and download the dll.

#### Mouse 
```csharp
// Set the mouse method to use the ddxoft virtual mouse & keyboard driver
Mouse.SetMethodFrom<MouseDD>(); 

// Sets the current position of the cursor
Mouse.SetCursorPos(50, 150); 

// Moves the mouse by 50 pixels relative to its current position
Mouse.Move(50, 50); 

// Will set the cursor position to the center of the given UIElement.
Mouse.SetCursorPos(myButton);

// Will click the left mouse-button with a 1 second delay between pressing & releasing
Mouse.Click(MouseKey.Left, 1); 
```

#### Keyboard 
```csharp 
// Set the keyboard method to use the undocumented "NtUserInjectKeyboardInput"-function
Keyboard.SetMethodFrom<NtUserInjectKeyboardInput>(); 

// Click the "W"-key with a 1 second delay between pressing & releasing
Keyboard.Click(VK.KEY_W, 1); 
```

For more examples, see the demo project.

## Built-in methods
Here you can see how to switch between all the built-in input-methods.

#### Mouse
```csharp
Mouse.SetMethodFrom<MouseEvent>();
Mouse.SetMethodFrom<NtUserInjectMouseInput>();
Mouse.SetMethodFrom<NtUserSendInput>();
Mouse.SetMethodFrom<MouseDD>();
```

#### Keyboard
```csharp
Keyboard.SetMethodFrom<KeyboardEvent>();
Keyboard.SetMethodFrom<NtUserInjectKeyboardInput>();
Keyboard.SetMethodFrom<NtUserSendInput>(); // different namespace than for mouse!
Keyboard.SetMethodFrom<KeyboardDD>();
```

## Custom methods
Besides the built-in input-methods, you can also add your own using the interfaces provided by the library.

You may look at the implementations of the built-in methods for reference.

Here is a quick example as to how an implementation of a custom input-method could look:
```csharp
public class MyMouseMethod : IMouseInput
{
    public string Name => nameof(MyMouseMethod);

    public bool MoveBy(int x = 0, int y = 0)
    {
      // add move logic
    } 

    public bool Press(MouseKey key = MouseKey.Left)
    {
      // add press logic
    }

    public bool Release(MouseKey key = MouseKey.Left)
    {
      // add release logic
    }

    public void Dispose()
    {
      // add dispose logic
      // you can add stuff like releasing the held keys, as the library doesn't take care of it
    }

    ~MyMouseMethod() => Dispose();
}

...

Mouse.SetMethodFrom<MyMouseMethod>(); // reflection will find it automatically
```

## Contributing
If you found or created a new method to manipulate either the mouse or keyboard and feel like sharing it with the public, please open a pull request and I will look at it.

Any help is greatly appreciated.

Please do **NOT** open a issue because you are being detected by some kind of Anti-Cheat etc. I will ignore and close it.

## Thanks to
- [ddxoft](https://github.com/ddxoft) for creating his virtual mouse & keyboard-driver.
- [Zpes](https://github.com/Zpes) for his project utilizing the undocumented NtUserInjectMouseInput  function.

## How to build
To build the project for all its supported frameworks simply run the "Build.bat"-script.

**Please Note**: You must add the directory of Visual Studio (*devenv.exe*) to your PATH-environment variable!

## Please Note
As I am developing this library in my free time, I simply cannot provide warranty that this project will be maintained in the future. Also I do not take responsibility for any misuse of this library. 

## License
This project is licensed under the Apache 2.0 license.   
