# Option and Result concept for .NET

A concept for an alternative to Exceptions and ```null``` values. Taking concepts from the realm of functional programming.

Defines the discriminated unions ```Result<T, TError>```och ```Option<T>```, and demonstrates how to produce and consume them in C# using existing features like pattern matching.

Inspired by [Beef programming language](https://www.beeflang.org/), a language inspired by C#, and others like Rust.

The syntax is quite verbose, but can be simplified with ```using static``` as shown in source code. Read more below.

## Result

Either has an ```Ok``` value of ```T```, or an ```Error``` value of ```TError```.

```csharp
public abstract record Result<T> : IDisposable
{
    public sealed record Ok(T Value) : Result<T>;

    public sealed record Error() : Result<T>;
}
```

Used like so:

```csharp
return new System.Result<string, Exception>.Ok("Test");

return new System.Result<string, Exception>.Error(new InvalidOperationException());
```

## Option

Either has ```Some``` value of ```T```, or ```None```.

```csharp
public abstract record Option<T> : IDisposable
{
    public sealed record Some(T Value) : Option<T>;

    public sealed record None : Option<T>;
}
```

Used like so:

```csharp
return new System.Option<string>.Ok("Test");

return new System.Option<string>.None();

// NOTE: Would have been nice if the parentheses of constructors were optional when having no arguments. There is little risk of confusion with fields.
```

## Pattern matching

To query a ```Result<T, MyError>``` using pattern matching you do this:

```csharp
var result = GetResult(3);

switch(result)
{
    case Result<int, MyError>.Ok(int r):
        Console.WriteLine($"Ok: {r}");
        break;
        
    case Result<int, MyError>.Error(MyError error):
        Console.WriteLine("Error");
        break;
}
```

And for ```Option<T>```:

```csharp
var someResult = GetSome(3);

switch(someResult)
{
    case Option<int>.Some(int r):
        Console.WriteLine($"Some: {r}");
        break;
        
    case Option<int>.None:
        Console.WriteLine("None");
        break;
}
```

This also works:

```csharp
var someResult = GetSome(3);

if(someResult is Option<int>.Some(int r))
{
    // Do something with result
}
else if(someResult is Option<int>.None)
{
    // Do something with None
}
```

## Considerations

Here we explain some of the quirks of these types.

### ```Unit``` type

Since ```void``` cannot be passed as a type parameter, to represent an empty value, consider using a ```Unit``` type such as the one found in MediatR.

### Why not ```record structs```?

Because we the types are dependant on inheritance.

### Using ```new``` when creating ```Ok```, ```Error``` etc.

Since ```Ok```, ```Error```, ```Some``` etc are types, you have to use the ```new``` keyword when creating them.

```csharp
return new System.Result<string, Exception>.Ok("Test");
```

### Having to specify generic type parameters
The C# compiler does not infer the type parameters based on the target type.

So you must specify what type like so everytime you use ```Result```, ```Option```, or any of their derived nested types.

```csharp
new Result<string, Exception>.Ok("Success");
```

This can be avoided to certain extent by ```
using static System.Result<int, Exception>```. But it doesn't always work.

### Import static members and avoid prefixing with ```Result<int, MyError>```

In conjuntion with the previous point you can import the static members to simplify your code:

```csharp
using static System.Result<string, Exception>;
```

The name of the type must be fully qualified.

You can then just:

```csharp
return new Ok("Test");
```

As noted before, it might not always work depending on the type resolution.

### Implicit conversions from ```Result``` or ```Option``` to value

It would be nice to just me able to:

```csharp
int result = GetResult(3);
```

If there was an error it would obviously fail, but at least it is opt in.

Code exists but it messes with pattern matching which used this conversion when it should not.

Perhaps an explicit conversion is prefered here?

```csharp
int result = (int)GetResult(3);
```

Again, this could fail due to being an error.

### Roslyn Analyzer (Proposal)
It would be nice to have an analyzer that checks whether a result has been fully handled and warns that it has not.