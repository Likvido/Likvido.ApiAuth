using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Likvido.ApiAuth.Common;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Likvido.ApiAuth.Tests
{
    public class SerializationDeserializationTests
    {
        [Fact]
        public void Serialization_Should_Work()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var header = UserInfoHeaderSerializer.Serialize(userId, claims);
            var rawString = Encoding.UTF8.GetString(Convert.FromBase64String(header));

            var root = JObject.Parse(rawString);
            root.Value<string>("userId").ShouldBe(userId);
            var jClaims = root.SelectToken("claims");
            jClaims.ShouldNotBeNull();
            jClaims.Value<JArray>("roles").ShouldNotBeNull();
            jClaims.Value<string>("sub").ShouldBe(userId);
            jClaims.Value<string>("Test").ShouldBe("1234");
        }

        [Fact]
        public void Serialization_Should_Work_Without_User_Id()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var header = UserInfoHeaderSerializer.Serialize(claims);
            var rawString = Encoding.UTF8.GetString(Convert.FromBase64String(header));

            var root = JObject.Parse(rawString);
            root.Value<string>("userId").ShouldBe(userId);
            var jClaims = root.SelectToken("claims");
            jClaims.ShouldNotBeNull();
            jClaims.Value<JArray>("roles").ShouldNotBeNull();
            jClaims.Value<string>("sub").ShouldBe(userId);
            jClaims.Value<string>("Test").ShouldBe("1234");
        }

        [Fact]
        public void Serialization_Should_Throw_When_UserId_Empty()
        {
            var claims = new List<Claim>();
            Should.Throw<ArgumentException>(() => UserInfoHeaderSerializer.Serialize("", claims));
        }

        [Fact]
        public void Serialization_Should_Throw_When_UserId_Null()
        {
            var claims = new List<Claim>();
            Should.Throw<ArgumentException>(() => UserInfoHeaderSerializer.Serialize("", claims));
        }

        [Fact]
        public void Serialization_Should_Throw_When_UserId_WhiteSpace()
        {
            var claims = new List<Claim>();
            Should.Throw<ArgumentException>(() => UserInfoHeaderSerializer.Serialize("   ", claims));
        }

        [Fact]
        public void Serialization_Should_Throw_No_Identifier_Claim_And_No_User_Id()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim("Test", "1234"),
            };

            Should.Throw<ArgumentException>(() => UserInfoHeaderSerializer.Serialize(Guid.NewGuid().ToString(), claims));
        }

        [Fact]
        public void Serialization_Should_Work_Throw_When_Identifier_Claim_Do_Not_Match_User_Id()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim("Test", "1234"),
            };

            Should.Throw<ArgumentException>(() => UserInfoHeaderSerializer.Serialize(claims));
        }

        [Fact]
        public void Serialization_Should_Work_With_Custom_Mapping()
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
            var header = UserInfoHeaderSerializer.Serialize(userId, claims, mapping);
            var rawString = Encoding.UTF8.GetString(Convert.FromBase64String(header));

            var root = JObject.Parse(rawString);
            root.Value<string>("userId").ShouldBe(userId);
            var jClaims = root.SelectToken("claims");
            jClaims.ShouldNotBeNull();
            jClaims.Value<JArray>("role").ShouldNotBeNull();
            jClaims.Value<string>("sub").ShouldBe(userId);
            jClaims.Value<string>("Test").ShouldBe("1234");
        }

        [Fact]
        public void Serialization_And_Deserialization_Should_Work()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var header = UserInfoHeaderSerializer.Serialize(userId, claims);
            var identity = UserInfoHeaderSerializer.Deserialize(header);

            identity.Claims.Select(c => new { c.Type, c.Value })
                .ShouldBe(claims.Select(c => new { c.Type, c.Value }));

            identity.FindFirst(ClaimTypes.NameIdentifier).Value.ShouldBe(userId);
        }

        [Fact]
        public void Serialization_And_Deserialization_Should_Work_With_Custom_Mapping()
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

            var header = UserInfoHeaderSerializer.Serialize(userId, claims, mapping);
            var identity = UserInfoHeaderSerializer.Deserialize(header, mapping);

            identity.Claims.Select(c => new { c.Type, c.Value })
                .ShouldBe(claims.Select(c => new { c.Type, c.Value }));

            identity.FindFirst(ClaimTypes.NameIdentifier).Value.ShouldBe(userId);
        }

        [Fact]
        public void Serialization_And_Deserialization_Single_Role_Should_Work()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Test", "1234"),
            };

            var header = UserInfoHeaderSerializer.Serialize(userId, claims);
            var identity = UserInfoHeaderSerializer.Deserialize(header);

            identity.Claims.Select(c => new { c.Type, c.Value })
                .ShouldBe(claims.Select(c => new { c.Type, c.Value }));

            identity.FindFirst(ClaimTypes.NameIdentifier).Value.ShouldBe(userId);
        }

        [Fact]
        public void Serialization_And_Deserialization_Without_NameIdentifier_Should_Work()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Creditor"),
                new Claim("Test", "1234"),
            };

            var header = UserInfoHeaderSerializer.Serialize(userId, claims);
            var identity = UserInfoHeaderSerializer.Deserialize(header);

            identity.Claims.Where(c => c.Type != ClaimTypes.NameIdentifier).Select(c => new { c.Type, c.Value })
                .ShouldBe(claims.Select(c => new { c.Type, c.Value }));

            identity.FindFirst(ClaimTypes.NameIdentifier).Value.ShouldBe(userId);
        }
    }
}
