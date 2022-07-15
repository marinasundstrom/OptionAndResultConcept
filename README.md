# Option and Result concept for .NET

Defines the discriminated unions ```Result<T, TError>```och ```Option<T>```, and demonstrates how to produce and consume them in C# using existing features like pattern matching.


## Unit

Since ```void``` cannot be passed as a type parameter, to represent an empty value, consider using ```Unit```type such as the one in MediatR.
