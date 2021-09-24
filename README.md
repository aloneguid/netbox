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

Good question! Installing it is pretty easy, just run in the folder you want it to be downloaded.

```bash
curl --location https://github.com/aloneguid/netbox/releases/download/x/NetBox.cs --output NetBox.cs
```

where `x` is the release number, like `4.0.2` This is also how you update it as well. Simple! Just put it in your build script or elsewhere, or just commit the `.cs` file locally, you know what to do!

## Contributing

To contribute, either add a method in a relevant class (tests are required!) or add a new file with corresponding tests. Run the tests as usual. Then raise a PR as usual.
