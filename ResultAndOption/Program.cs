using static System.Result<int, MyError>;
using static System.Option<int>;

var someResult = GetSome(3);

switch(someResult)
{
    case Some(int r):
        Console.WriteLine($"Some: {r}");
        break;
        
    case None:
        Console.WriteLine("None");
        break;
}

/*
if(someResult is Some(int r)) 
{
    Console.WriteLine($"Some: {r}");
}
else if (someResult is None)
{
    Console.WriteLine("None");
}
*/

var result = GetResult(0);

switch (result)
{
    case Ok(int a):
        Console.WriteLine($"Ok: {a}");
        break;

    case Error(MyError error):
        Console.WriteLine($"Error: {error}");
        break;
}

Option<int> GetSome(int x)
{
    if (x > 0)
    {
        return new Some(x);
    }
    return new None();
}

Result<int, MyError> GetResult(int x)
{
    if(x > 0)
    {
        return new Ok(x);
    }
    return new Error(new MyError("Ouch!"));
}

public record MyError(string Message);