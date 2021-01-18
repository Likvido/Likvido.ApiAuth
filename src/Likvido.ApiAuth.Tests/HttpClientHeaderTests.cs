using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Likvido.ApiAuth.Client;
using Likvido.ApiAuth.Common;
using Shouldly;
using Xunit;

namespace Likvido.ApiAuth.Tests
{
    public class HttpClientHeaderTests
    {
        [Fact]
        public void Should_Set_HttpClient_Header()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.SetUserInfoHeader(userId, claims);

            var header = client.DefaultRequestHeaders.GetValues(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims);
            header.ShouldBe(new[] { serialized });
        }

        [Fact]
        public void Should_Set_HttpClient_Header_With_Custom_Mapping()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };
            var mapping = TokenClaims.Default;
            mapping.AddTokenClaimToClaim("role", ClaimTypes.Role);
            var client = new HttpClient();
            client.DefaultRequestHeaders.SetUserInfoHeader(userId, claims, mapping);

            var header = client.DefaultRequestHeaders.GetValues(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims, mapping);
            header.ShouldBe(new[] { serialized });
        }

        [Fact]
        public void Should_Set_HttpClient_Header_Without_User_Id()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.SetUserInfoHeader(claims);

            var header = client.DefaultRequestHeaders.GetValues(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims);
            header.ShouldBe(new[] { serialized });
        }

        [Fact]
        public void Should_Set_HttpClient_Header_Without_User_Id_With_Custom_Mapping()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };
            var mapping = TokenClaims.Default;
            mapping.AddTokenClaimToClaim("role", ClaimTypes.Role);
            var client = new HttpClient();
            client.DefaultRequestHeaders.SetUserInfoHeader(claims, mapping);

            var header = client.DefaultRequestHeaders.GetValues(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims, mapping);
            header.ShouldBe(new[] { serialized });
        }

        [Fact]
        public void Should_Set_HttpRequestMessage_Header_With_Custom_Mapping()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var message = new HttpRequestMessage();
            message.Headers.SetUserInfoHeader(userId, claims);

            var header = message.Headers.GetValues(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims);
            header.ShouldBe(new[] { serialized });
        }

        [Fact]
        public void Should_Set_HttpRequestMessage_Header()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var mapping = TokenClaims.Default;
            mapping.AddTokenClaimToClaim("role", ClaimTypes.Role);

            var message = new HttpRequestMessage();
            message.Headers.SetUserInfoHeader(userId, claims, mapping);

            var header = message.Headers.GetValues(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims, mapping);
            header.ShouldBe(new[] { serialized });
        }
    }
}
