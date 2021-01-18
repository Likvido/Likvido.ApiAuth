# Likvido.ApiAuth.Client
Library to simplify sending user info header from a client side
## Usage
### WebClient
```
using System.Security.Claims;
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

//optional if you aren't happy with default mapping or you want to add some specific mapping
//if no mapping exists, then the original claim will be used in the token
var mapping = TokenClaims.Default;
mapping.AddTokenClaimToClaim("role", ClaimTypes.Role);

var client = new WebClient();
//Using the overload method without the "userId" will result in using the "ClaimTypes.NameIdentifier" to detect the user id
client.Headers.SetUserInfoHeader(userId, claims, mapping);
```

### HttpClient
```
using Likvido.ApiAuth.Client;
//the same as with the WebClient except for a couple of lines
var client = new HttpClient();
client.DefaultRequestHeaders.SetUserInfoHeader(userId, claims, mapping);
```
### HttpRequestMessage
```
using Likvido.ApiAuth.Client;
//the same as with the WebClient except for a couple of lines
var message = new HttpRequestMessage();
message.Headers.SetUserInfoHeader(userId, claims, mapping);
```
## Default mapping
//TODO: add link after merge
