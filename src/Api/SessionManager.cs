using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace KostalApiClient.Api
{
    public class Session
    {      
        private const string UserType = "user";
        private const string AuthStartEndpoint = "/auth/start";
        private const string AuthFinishEndpoint = "/auth/finish";
        private const string AuthCreateSessionEndpoint = "/auth/create_session";
        private readonly KostalRestClient _client;
        private string _sessionId;

        public AuthEndpoint Auth { get; }
        public EventsEndpoint Events { get; }
        public InfoEndpoint Info { get; }
        public LogDataEndpoint LogData { get; }
        public ProcessDataEndpoint ProcessData { get; }
        public ModulesEndpoint Modules { get; }
        public SettingsEndpoint Settings { get; }
        public SystemEndpoint System { get; }
        

        /// <summary>
        /// Start point of the API. Create a session object, from which you can reach all the API endpoints under Auth, Events, Info, LogData, ProcessData, Modules and Settings properties. You can login or not, but keep in mind that some of endpoints requires authenfication.
        /// </summary>
        /// <param name="host"></param>
        public Session(string host)
        {
            _client = new KostalRestClient($"http://{host}/api/v1");
            LogData = new LogDataEndpoint(_client);
            Auth = new AuthEndpoint(_client);
            Modules = new ModulesEndpoint(_client);
            Info = new InfoEndpoint(_client);
            ProcessData = new ProcessDataEndpoint(_client);
            Events = new EventsEndpoint(_client);
            Settings = new SettingsEndpoint(_client);
            System = new SystemEndpoint(_client);
        }

        /// <summary>
        /// API Login 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task Login(string password)
        {
            string nonce = Base64Encode(RandomString(12));
            AuthStartData authStartData = await AuthStart(nonce);
            AuthFinishData authFinishData = await AuthFinish(authStartData, nonce, password);
            _sessionId = CreateSession(authFinishData);
            if (!string.IsNullOrEmpty(_sessionId))
            {
                _client.IsAuthenticated = true;
                _client.AddDefaultHeader("authorization", "Session " + _sessionId);
            }
            else
            {
                _client.IsAuthenticated = false;
            }
        }

        private static readonly Random Random = new();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static byte[] HashHmac(byte[] key, byte[] message)
        {
            HMACSHA256 hash = new(key);
            return hash.ComputeHash(message);
        }

        private async Task<AuthStartData> AuthStart(string nonce)
        {
            RestRequest authStartRequest = new (AuthStartEndpoint) {RequestFormat = DataFormat.Json};
            authStartRequest.AddJsonBody(new { username = UserType, nonce }); // uses JsonSerializer
            return await _client.PostAsync<AuthStartData>(authStartRequest);
        }

        private async Task<AuthFinishData> AuthFinish(AuthStartData authStartData, string nonce, string password)
        {
            byte[] key;
            using (Rfc2898DeriveBytes pbkdf2 = new(Encoding.UTF8.GetBytes(password), Convert.FromBase64String(authStartData.Salt), authStartData.Rounds, HashAlgorithmName.SHA256))
            {
                key = pbkdf2.GetBytes(32);
            }

            byte[] clientKey = HashHmac(key, "Client Key"u8.ToArray());
            byte[] serverKey = HashHmac(key, "Server Key"u8.ToArray());
            byte[] clientKeyHash = SHA256.HashData(clientKey);

            string authString = $"n=user,r={nonce},r={authStartData.Nonce},s={authStartData.Salt},i={authStartData.Rounds},c=biws,r={authStartData.Nonce}";

            byte[] clientAuthHash = HashHmac(clientKeyHash, Encoding.UTF8.GetBytes(authString));

            string authProof = Convert.ToBase64String(clientKey.Zip(clientAuthHash).Select(valueTuple => Convert.ToByte(valueTuple.First ^ valueTuple.Second)).ToArray());
            RestRequest authFinishRequest = new(AuthFinishEndpoint) {RequestFormat = DataFormat.Json};
            authFinishRequest.AddJsonBody(new { transactionId = authStartData.TransactionId, proof = authProof }); // uses JsonSerializer
            AuthFinishData authFinishData = await _client.PostAsync<AuthFinishData>(authFinishRequest);
            authFinishData.ClientKeyHash = clientKeyHash;
            authFinishData.ClientKey = clientKey;
            authFinishData.AuthString = authString;
            authFinishData.TransactionId = authStartData.TransactionId;
            return authFinishData;
        }

        private string CreateSession(AuthFinishData authFinishData)
        {
            byte[] protocolKey = HashHmac(authFinishData.ClientKeyHash, "Session Key"u8.ToArray().Concat( Encoding.UTF8.GetBytes(authFinishData.AuthString)).Concat(authFinishData.ClientKey).ToArray());
            // Get bytes of plaintext string
            byte[] plainBytes = Encoding.UTF8.GetBytes(authFinishData.Token);
    
            // Get parameter sizes
            int cipherSize = plainBytes.Length;
    
            // We write everything into one big array for easier encoding
            Span<byte> encryptedData = plainBytes.Length < 1024
                ? stackalloc byte[plainBytes.Length]
                : new byte[plainBytes.Length].AsSpan();

            Span<byte> tag = AesGcm.TagByteSizes.MaxSize < 1024
                ? stackalloc byte[AesGcm.TagByteSizes.MaxSize]
                : new byte[AesGcm.TagByteSizes.MaxSize].AsSpan();

            Span<byte> cipherBytes = encryptedData.Slice( 0, cipherSize);

            byte[] nonce = Encoding.UTF8.GetBytes(RandomString(12));

            // Generate secure nonce
            RandomNumberGenerator.Fill(nonce);

            // Encrypt
            using AesGcm aes = new(protocolKey, 16);
            aes.Encrypt(nonce, plainBytes.AsSpan(), cipherBytes, tag);

            RestRequest createSessionRequest = new(AuthCreateSessionEndpoint) {RequestFormat = DataFormat.Json};
            createSessionRequest.AddJsonBody(new { transactionId = authFinishData.TransactionId, iv = Convert.ToBase64String(nonce.ToArray()), tag = Convert.ToBase64String(tag.ToArray()), payload = Convert.ToBase64String(encryptedData.ToArray()) }); // uses JsonSerializer
            RestResponse<SessionData> createSessionResponse = _client.ExecutePost<SessionData>(createSessionRequest);
            return createSessionResponse.IsSuccessful ? createSessionResponse.Data.SessionId : null;
        }


        private class AuthStartData  
        {
            [JsonProperty("rounds")]
            public int Rounds { get; set; } 
            [JsonProperty("salt")]
            public string Salt { get; set; } 
            [JsonProperty("transactionId")]
            public string TransactionId { get; set; } 
            [JsonProperty("nonce")]
            public string Nonce { get; set; } 
        }

        private class AuthFinishData  
        {
            [JsonProperty("signature")]
            public string Signature { get; set; } 
            [JsonProperty("token")]
            public string Token { get; set; }

            public byte[] ClientKeyHash { get; set; }
            public byte[] ClientKey { get; set; }
            public string AuthString { get; set; }
            public string TransactionId { get; set; }
        }

        private class SessionData
        {
            [JsonProperty("sessionId")] 
            public string SessionId { get; set; }
        }
    }
}