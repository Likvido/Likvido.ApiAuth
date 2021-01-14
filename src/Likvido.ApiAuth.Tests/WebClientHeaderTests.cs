using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Xunit;
using Likvido.ApiAuth.Client;
using Likvido.ApiAuth.Common;
using Shouldly;

namespace Likvido.ApiAuth.Tests
{
    public class WebClientHeaderTests
    {
        [Fact]
        public void Should_Set_WebClient_Header()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            using var client = new WebClient();
            client.Headers.SetUserInfoHeader(userId, claims);

            var header = client.Headers.Get(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims);
            header.ShouldBe(serialized);
        }

        [Fact]
        public void Should_Set_WebClient_Header_With_Custom_Mapping()
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

            var client = new WebClient();
            client.Headers.SetUserInfoHeader(userId, claims, mapping);

            var header = client.Headers.Get(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(userId, claims, mapping);
            header.ShouldBe(serialized);
        }

        [Fact]
        public void Should_Set_WebClient_Header_Without_User_Id()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            using var client = new WebClient();
            client.Headers.SetUserInfoHeader(claims);

            var header = client.Headers.Get(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(claims);
            header.ShouldBe(serialized);
        }

        [Fact]
        public void Should_Set_WebClient_Header_Without_User_Id_With_Custom_Mapping()
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

            using var client = new WebClient();
            client.Headers.SetUserInfoHeader(claims, mapping);

            var header = client.Headers.Get(AuthConstants.UserInfoHeader);
            var serialized = UserInfoHeaderSerializer.Serialize(claims, mapping);
            header.ShouldBe(serialized);
        }
    }
}
