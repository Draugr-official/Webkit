# Webkit
Webkit is a SDK for ASP.NET applications. This SDK provides a wide range of useful tools for data management, logging, authentication, authorization, security, sessions, telemetry and more.

## Table of Content
1. [Extensions](#webkit.extensions)
	* [Logging](#webkit.extensions.logging)
		* void \<T>.Log(T value)
		* void \<T>.Log(T value, string prependText)
		* void \<T>.Log(T value, string prependText, string appendText)
		* void \<T>.LogAsJson(T value)
		* void \<T>.LogAsJson(T value, string prependText)
		* void \<T>.LogAsJson(T value, string prependText, string appendText)
		* void \<T>.LogAsXml(T value)
		* void \<T>.LogAsXml(T value, string prependText)
		* void \<T>.LogAsXml(T value, string prependText, string appendText)
	* [Console](#webkit.extensions.console)
		* void ConsoleColor.Write(object value)
		* void ConsoleColor.WriteLine(object value)
	* [Data conversion](#Webkit.Extensions.DataConversion)
		* string Stream.AsString()
		* string string.Collapse()
		* string IHeaderDictionary.AsString()
		* string \<T>.AsJson()
		* string \<T>.AsXml()
		* string byte[].AsString()
		* string byte[].AsString(Encoding encoding)
		* byte[] string.AsByteArray()
		* byte[] string.AsByteArray(Encoding encoding)
	* [Compression](#Webkit.Extensions.Compression)
		* byte[] byte[].GZipCompress(CompressionLevel level)
		* byte[] byte[].GZipDecompress()
		* byte[] byte[].BrotliCompress(CompressionLevel level)
		* byte[] byte[].BrotliDecompress()
1. [Security](#webkit.security)
	* CryptographicGenerator
		* string Seed(int length = 40)
		* string UnicodeSeed(int length = 40)
	* JsonSecurityToken
	* Password.PasswordHandler
		* string Hash(string password)
		* bool Validate(string password, string hash)

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

___

## Authentication
Webkit has a couple useful features to make authentication and authorization easier

### `AuthenticateAttribute : ActionFilterAttribute`
This attribute will check the validity of a user token before executing the action. The user token is stored in cokies under `token`.
`AuthenticateAttribute` uses an in-memory cache to store sessions. These can be found in `UserSessionCache`.
Set the `AuthenticateAttribute.Validate` action to implement your own method.

### `AuthorizeAttribute : ActionFilterAttribute`
This attribute will check if a user has a specified role but will NOT determine if the user is authenticated. Use the `AuthenticateAttribute` and `AuthorizeAttribute` on the same endpoint to check both.
You can specify what role is required for the endpoint by setting `Role` in the attribute.

Here is an example of how webkit authentication should be implemented.
```cs
[HttpPost("login")]
public ActionResult Login([FromBody] LoginDto loginDto)
{
    using(MockDatabase mockDb = new MockDatabase())
    {
        List<User> users = mockDb.Users.Where(user => user.Username == loginDto.Username && PasswordHandler.Validate(loginDto.Password, user.Password)).ToList();
        if(!users.Any())
        {
            return NotFound();
        }

        User user = users.First();
        JsonSecurityToken jsonToken = new JsonSecurityToken(user.Id, user.Roles);

        DateTime tokenExpiration = DateTime.Now.AddDays(30);
        string token = UserSessionCache.Register(jsonToken, tokenExpiration);
        user.Token = token;

        mockDb.SaveChanges();

        Response.Cookies.Append("token", token, new CookieOptions
        {
            Expires = tokenExpiration,
        });
        return Ok();
    }
}

public class LoginDto
{
    public string Username { get; set; } = "";

    public string Password { get; set; } = "";
}
```

___

## Telemetry