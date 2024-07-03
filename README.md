### What are Results
In its base implementation a Result\<T\> is a merely a wrapper around an object, that allows us to return errors instead of throwing them, and have them be handled as part of the control flow.

Without making any changes to the base implementation of C#, the only way of achieving this would be to make every method return a tuple of `(object, Exception)` which would be horrible.

With the Result type it is very simple to handle both the value we're looking for, but also to handle the exception correctly, by its very implementation we're forced to handle it. The same can be said for the Option.

### What are Options
Options alleviates this problem, since we have to handle both of the potential states. An Option can be two things. A `Some` or a `None`. 

Options are how we avoid the problem of null values. In C# we do have nullable now, which allows us to check .HasValue, and then .Value, or just use the ? operator to see if we wall call underlying methods or not, or by checking it like this:
```csharp
int? x = 42;
if (x is int value)
{
	Console.WriteLine($"The value is: {value}");
}
else
...
```

We are however only certain that all potential nullable values are handled as long as we enable nullable for the project, and enforce warnings as errors. It is far too easy to just put a ! on it and enforce that it cannot be null at this point.

Often this is used for testing, but it has a tendency to linger.


### What is OneOf
OneOf is a [discriminated union](https://blog.maartenballiauw.be/post/2023/09/18/discriminated-unions-in-csharp.html), which makes it trivial to pass one of several objects or variables around.

You might have a method/function that should do an operation on all kinds of weather, but each type of weather is slightly different. OneOf is very neat for these situations as you can pass in OneOf\<InclementWeather, NiceWeather, ...\> or return it, without having to worry about only accepting the base IWeather interface, or inherited Weather base class, and then doing a GetType to be certain you're working with the correct child. 

### LanguageExt
This nuget package has a whole slew of features that enables us to take parts of the functional world more effortlessly without going full functional.

### Resources
- [What is a Monad](https://en.wikipedia.org/wiki/Monad_(functional_programming))
- [Technical definition of Monad](https://miro.medium.com/v2/resize:fit:1276/1*bgUaZlCVQxPNfkvJu0qNXw.jpeg)
- [Function programming can be frustrating](https://www.youtube.com/watch?v=dNi__BckudQ)
- [LINQ is function](https://learn.microsoft.com/en-us/dotnet/standard/linq/functional-vs-imperative-programming)
- [Discriminated Unions are neat](https://blog.maartenballiauw.be/post/2023/09/18/discriminated-unions-in-csharp.html)
- [Github LanguageExt](https://github.com/louthy/language-ext?tab=readme-ov-file)
- [Github OneOf](https://github.com/mcintyre321/OneOf)
