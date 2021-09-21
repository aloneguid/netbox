# .NET Utility Library

![](src/icon.png)

It's a utility library. Creating utility libraries is hard, as no one will actually use them, because:

- No one wants a dependency on some **rubbish** utility library.
- It's **exposing a lot of rubbish for external users**, especially if you are using a utility library from another public library.

Therefore, this is IMHO a completely new approach:

- There is **no library**. In the traditional sense. No NuGet package. Just source code.
- In order to use this code, you **reference single `.cs` file**. As easy as that!
- Every single class in this codebase is **private**. Meaning it will be compiled into your codebase, but you can use it all, because it's private to **your codebase**.

This approach is very popular in other languages (C/C++, Golang, Rust) so why not trying it with .NET?

## So How The Hell Do I Use This?

Good question! 
