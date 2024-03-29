# Likvido.ApiAuth.Server
Handy middlewares to make server side auth token easier
## ApiKeyAuthMiddleware
Checks that any request has valid API key provided and set the request identity. Identity have `AuthenticationType` equal to the header name and will have a claim with `Type=ClaimTypes.Name` and value equal to `GetUserName` method result
Default header name `X-API-Key`.

**SHOULD go before UseAuthentication or UserInfoMiddlewareExtensions**

```
    public class ApiKeyAuthOptions : ApiKeyAuthOptionsBase
    {
        public override string GetUserName(string apiKey)
        {
            //Add your key - consuming app mapping logic
        }
    }
    app.UseRouting();

    app.UseApiKeyAuthMiddleware();

    app.UseEndpoints(...)
```
### Hint
if you want to skip api key check for some routes use the following
```
app.UseWhen(
    context =>
    {
        //your conditions gohere
        return true;
    },
    b => b.UseApiKeyAuthMiddleware());
```
## UserInfoHeaderMiddleware
Doesn't do any auth check but just sets or adds identity passed in user info header

```
    app.UseRouting();

    app.UseApiKeyAuthMiddleware();
    app.UseUserInfoMiddleware();

    app.UseEndpoints(...)
```
## UserInfoHeaderAuthenticationHandler
Use it if you decide to use built-in auth capabilities such as `[Authorize]` attribute
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddOptions();
    services.AddSingleton<IApiKeyAuthOptions, ApiKeyAuthOptions>();
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = AuthConstants.UserInfoHeader;
    }).AddScheme<UserInfoHeaderAuthenticationSchemeOptions, UserInfoHeaderAuthenticationHandler>(
        AuthConstants.UserInfoHeader,
        op => { });
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //omitted for brevity
    app.UseRouting();

    app.UseApiKeyAuthMiddleware();

    app.UseAuthentication();
    app.UseAuthorization();

    //omitted for brevity
}
```
