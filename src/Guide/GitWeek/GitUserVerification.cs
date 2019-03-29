using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Guide.Json;
using Guide.Logging;
using Newtonsoft.Json;
using Octokit;

namespace Guide.GitWeek
{
    public class GitUserVerification : IGitUserVerification
    {
        private readonly IJsonStorage _storage;
        private readonly GitVerification _verification;
        private readonly GitHubClient _github;

        public GitUserVerification(IJsonStorage storage, GitHubClient github)
        {
            _storage = storage;
            _verification = _storage.Get<GitVerification>("verification.json");
            _github = github;

            if (!(_verification is null)) return;
            
            _verification = new GitVerification
            {
                VerifiedUsers = new Dictionary<ulong, string>(),
                VerificationStrings = new Dictionary<string, string>()
            };
            
            _storage.Store(_verification, "verification.json", Formatting.Indented);
        }
        
        public async Task<string> GetVerificationStringFor(ulong userId, string githubUsername)
        {
            try
            {
                var user = await _github.User.Get(githubUsername);
            }
            catch (Exception)
            {
                throw new Exception("A GitHub user with this name doesn't exist.");
            }
            
            if(_verification.VerifiedUsers.ContainsKey(userId))
                throw new Exception("You already have a verified GitHub account.");
            
            if(_verification.VerifiedUsers.ContainsValue(githubUsername))
                throw new Exception("This username is already verified by someone else.");

            if (_verification.VerificationStrings.ContainsKey(githubUsername))
                return _verification.VerificationStrings[githubUsername];
            
            _verification.VerificationStrings.Add(githubUsername, GetHashString(githubUsername));
            _storage.Store(_verification, "verification.json", Formatting.Indented);
            return _verification.VerificationStrings[githubUsername];
        }

        public async Task<bool> Verify(ulong userId, string githubUsername)
        {
            var success = _verification.VerificationStrings.TryGetValue(githubUsername, out var expectedHash);
            
            if(!success)
                throw new Exception($"This user is not registered. Try registering it with:\n```\ngithub register {githubUsername}\n```");
            
            var user = await _github.User.Get(githubUsername);

            if(user.Bio is null)
                throw new Exception("Your Bio is empty.");
            
            if (!user.Bio.Contains(expectedHash))
                throw new Exception("Your GitHub bio does not contain the expected hash.");

            _verification.VerificationStrings.Remove(githubUsername);
            _verification.VerifiedUsers.Add(userId, githubUsername);
            _storage.Store(_verification, "verification.json", Formatting.Indented);
            return true;
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GetHashString(string inputString)
        {
            var sb = new StringBuilder();
            foreach (var b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}