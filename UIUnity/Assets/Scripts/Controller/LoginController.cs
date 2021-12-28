using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Interface;
using Common.Scriptable;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Model.Login.Token;
using Newtonsoft.Json;
using RestSharp;

namespace Controller
{
    public class LoginController : ILoginController
    {
        private readonly LoginInfoModel _loginInfoModel;

        private bool IsRoleNote { get; set; }
        public string Token { get; private set; }
        
        public LoginController(LoginInfoModel loginInfoModel)
        {
            _loginInfoModel = loginInfoModel;
        }
        
        public async Task<(bool, string)> Login(string userName, string password)
        {
            var client = new RestClient(_loginInfoModel.urlAccesToken)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter(
                "application/x-www-form-urlencoded",
                $"client_id={_loginInfoModel.clientId}&grant_type=password&scope=openid&username={userName}&password={password}", 
                ParameterType.RequestBody);
            
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
                return (false, response.StatusDescription);
            
            var keycloakToken = JsonConvert.DeserializeObject<KeycloakTokenModel>(response.Content);
            if (keycloakToken == null)
                return (false, "No token model");

            var certs = await GetCerts();
            foreach (var key in certs.Keys)
            {
                var result = GetToken(keycloakToken.AccessToken, key);
                if (result == null) continue;
                return !SetParams(result, keycloakToken.AccessToken) ? (false, "I don't have a permition") : (true, "");
            }

            return (false, "I don't have a permition");
        }

        private bool SetParams(JWTokenModel token, string accessToken)
        {
            if (!Guid.TryParse(token.Subject, out var userId))
                return false;
            IsRoleNote = token.Roles.Roles.Any(n => n == _loginInfoModel.role);
            if (IsRoleNote) Token = accessToken;
            return IsRoleNote;
        }
        
        private static JWTokenModel GetToken(string token, KeycloakCertModel key)
        {
            try
            {
                var urlEncoder = new JwtBase64UrlEncoder();
                var rsaKey = RSA.Create();
                rsaKey.ImportParameters(new RSAParameters
                {
                    Modulus = urlEncoder.Decode(key.PublicKey),
                    Exponent = urlEncoder.Decode(key.Exponent)
                });
                var result = new JwtBuilder()
                    .WithAlgorithm(new RS256Algorithm(rsaKey))
                    .MustVerifySignature()
                    .Decode<JWTokenModel>(token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<Certs> GetCerts()
        {
            var client = new RestClient(_loginInfoModel.urlCerts);
            var request = new RestRequest(Method.GET);
            var result = await client.ExecuteAsync(request);
            if (result.StatusCode != HttpStatusCode.OK)
                throw new RestException("");

            return JsonConvert.DeserializeObject<Certs>(result.Content);
        }
    }
}