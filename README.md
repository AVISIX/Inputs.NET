

# Inputs.NET
Inputs.NET is an easy-to-use, extendible, free and open-source library for the .NET ecosystem. It implements several methods to manipulate the mouse and keyboard on the windows operating system. 

If you like this project and want to support further development, please consider leaving a star.

**So far only tested on Windows 11!**

## "Why though?"
After getting annoyed about how tedious it can be to reliably and securely manipulate the mouse & keyboard on windows, I decided to take this matter into my own hands. Hence why I created this project.  

I originally wrote this for myself, but after seeing how useful it could be for others, I decided to make it public.

## Features
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
- Keyboard-macro recorder & player-classes
- Usermode keyboard & mouse spoofer
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
```csharp 
// placeholder
```

For more examples, see the demo project.

## Custom methods
Besides the built-in input-methods, you may add your own using the interfaces provided by the library.

You may look at the implementations of the built-in methods for reference.

Here is a quick example as to how the implementation of a custom input-method could look:
```csharp
// placeholder
```

## Contributing
If you found or created a new method to manipulate either the mouse or keyboard and feel like sharing it with the public, please open a pull request and I will look at it.

Any help is greatly appreciated.

## Thanks to
- [ddxoft](https://github.com/ddxoft) for creating his virtual mouse & keyboard-driver.
- [Zpes](https://github.com/Zpes) for his project utilizing the undocumented NtUserInjectMouseInput  function.

## Please Note
As I am developing this library in my free time, I simply cannot provide warranty that this project will be maintained in the future. Also I do not take responsibility for any misuse of this library. 

## License
This project is licensed under the Apache 2.0 license.   
