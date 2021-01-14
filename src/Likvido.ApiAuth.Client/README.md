# Likvido.ApiAuth.Client
Library to simplify sending user info header from a client side
## Usage
### WebClient
```
using Likvido.ApiAuth.Client;
var userId = Guid.NewGuid().ToString();
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Role, "Admin"),
    new Claim(ClaimTypes.Role, "Creditor"),
    new Claim(ClaimTypes.NameIdentifier, userId),
    new Claim("whatever", "1"),
    new Claim("namespace/whatever", "2"),
};

//optional if aren't happy with default mapping or want to add some speicifcmaaping
//if no mapping exists original claim will be used in token
var mapping = TokenClaims.Default;
mapping.AddTokenClaimToClaim("role", ClaimTypes.Role);

var client = new WebClient();
//The overload method without "userId" is this case "ClaimTypes.NameIdentifier" will be used to detect a user id
client.Headers.SetUserInfoHeader(userId, claims, mapping);
```

### HttpClient
```
using Likvido.ApiAuth.Client;
//same as WebClient apart from a couple of lines
var client = new HttpClient();
client.DefaultRequestHeaders.SetUserInfoHeader(userId, claims, mapping);
```
### HttpRequestMessage
```
using Likvido.ApiAuth.Client;
//same as WebClient apart from a couple of lines
var message = new HttpRequestMessage();
message.Headers.SetUserInfoHeader(userId, claims, mapping);
```
## Default mapping
//TODO: add link after merge