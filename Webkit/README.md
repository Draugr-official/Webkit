# Webkit
Webkit is a SDK for ASP.NET applications. This SDK provides a wide range of useful tools for data management, logging, authentication, authorization, security, sessions, telemetry and more.

## Table of Content
1. [Extensions](#webkit.extensions)
	* [Logging](#webkit.extensions.logging)
	* [Console](#webkit.extensions.console)
	* [Data management](#Webkit.Extensions.DataConversion)
1. [Security](#webkit.security)

# Webkit.Extensions
Webkit contains a couple extension methods to make data conversion and management a lot easier.

## Webkit.Extensions.Logging

### `Action<object> DefaultLog(object data)`
The method used when Log methods are called. This is defaulted to `Console.WriteLine`
```cs
LoggingExtensions.DefaultLog = (object data) =>
{
	Console.WriteLine(data);
}
```

___
### `<void> <T>.Log(T value)`
Performs a .ToString() on value and calls the logging action.
```cs
int number = 3;
number.Log(); // 3
```

### `<void> <T>.Log(T value, string prependText)`
Performs a .ToString() on value, prepends text then calls the logging action.
```cs
int number = 3;
number.Log("Number: "); // Number: 3
```

### `<void> <T>.Log(T value, string prependText, string appendText)`
Performs a .ToString() on value, prepends and appends text then calls the logging action.
```cs
int number = 3;
number.Log("There are ", " items left"); // There are 3 items left
```

___

### `<void> <T>.LogAsJson(T value)`
Performs a Json.Serialize on value and calls the logging action.
```cs
int number = 3;
number.Log(); // 3
```

### `<void> <T>.LogAsJson(T value, string prependText)`
Performs a Json.Serialize on value, prepends text then calls the logging action.
```cs
int number = 3;
number.Log("Number: "); // Number: 3
```

### `<void> <T>.LogAsJson(T value, string prependText, string appendText)`
Performs a Json.Serialize on value, prepends and appends text then calls the logging action.
```cs
int number = 3;
number.Log("There are ", " items left"); // There are 3 items left
```

___

### `<void> <T>.LogAsXml(T value)`
Converts value to a XML sheet then calls the logging action.
```cs
int number = 3;
number.Log(); // 3
```

### `<void> <T>.LogAsXml(T value, string prependText)`
Converts value to a XML sheet, prepends text then calls the logging action.
```cs
int number = 3;
number.Log("Number: "); // Number: 3
```

### `<void> <T>.LogAsXml(T value, string prependText, string appendText)`
Converts value to a XML sheet, prepends and appends text then calls the logging action.
```cs
int number = 3;
number.Log("There are ", " items left"); // There are 3 items left
```

___

## Webkit.Extensions.Console
Webkit has a couple extensions to make console logging with color quicker and easier.

### `<void> ConsoleColor.WriteLine(object value)`
Writes value and appends Environment.NewLine to the console with specified console color.
```cs
ConsoleColor.Yellow.WriteLine("[Warning] Service is disabled, this may impact user experience."); // [Warning] Service is disabled, this may impact user experience.\r\n
```


### `<void> ConsoleColor.Write(object value)`
Writes value to the console with specified console color.
```cs
ConsoleColor.Yellow.Write("[Warning] Service is disabled, this may impact user experience."); // [Warning] Service is disabled, this may impact user experience.
```

___

## Webkit.Extensions.DataConversion
These extensions help convert and manage data to different formats.

### `<string> Stream.AsString()`
Converts a stream to a string.

### `<string> string.Collapse()`
Collapses the newlines in the string, making it one line.
```cs
@"Lorem ipsum
dolor sit
amet".Collapse();

// Lorem ipsum dolor sit amet
```

### `<string> IHeaderDictionary.AsString()`
Converts a dictionary of headers to a string.
```cs
Request.Headers.AsString();

/*
Host: example.com
Content-Type: text/plain
Content-Length: 57
*/
```

### `<string> <T>.AsJson()`
Converts any value to a json string
```cs
User user = new User()
{
	Name = "John",
	Email = "john@example.com"
};

user.AsJson();

/*
{
	"Name": "John",
	"Email": "john@example.com"
}
*/
```

### `<string> <T>.AsXml()`
Converts any value to a XML sheet
```cs
User user = new User()
{
	Name = "John",
	Email = "john@example.com"
};

user.AsXml();

/*
<User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	<Name>John</Name>
	<Email>john@example.com</Email>
</User>
*/
```

___

## Webkit.Security
Webkit contains a couple methods to manage data that needs to be cryptographically secure.

### `<string> CryptographicGenerator.Seed(int length)`
Generates a cryptographically safe and universally unique seed. The seed will always be 9 characters longer than specified.
```cs
CryptographicGenerator.Seed(40); // Ujg2LLlbu1BspM6kmPk6dT7D71KpmoxbP3uNwJGP-8DC39DE328E6BD3
```

### `<string> CryptographicGenerator.UnicodeSeed(int length)`
Generates a cryptographically safe and universally unique seed using the unicode range. The seed will always be 9 characters longer than specified.
```cs
CryptographicGenerator.UnicodeSeed(40); // ㉔ᩰ⩠ᇤみ᪸ㅇ┗᪄⯿㏹᰷┚₎ḇ◱⡈⚺ᅗ❽⁊ᐬ⫴⏿⺢ሱᴥᙵᙳᓃṵ⯱ᮈ⺶⽢᤹❽㏡⚐㊯-8DC39DE94A69998
```