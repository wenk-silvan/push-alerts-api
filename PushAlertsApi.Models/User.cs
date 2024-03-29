﻿using System.Text.Json.Serialization;

namespace PushAlertsApi.Models
{
    /// <summary>
    /// Represents a user that is using the app in an authenticated way. Certain properties are not part of the JSON serialization.
    /// </summary>
    public class User
    {
        [JsonIgnore] public int Id { get; set; }

        public Guid Uuid { get; set; }

        public string Email { get; set; }

        [JsonIgnore] public byte[] PasswordHash { get; set; }

        [JsonIgnore] public byte[] PasswordSalt { get; set; }

        [JsonIgnore] public virtual List<Project>? Projects { get; set; } = new();

        public User()
        {
            PasswordHash = Array.Empty<byte>();
            PasswordSalt = Array.Empty<byte>();
            Uuid = Guid.Empty;
            Email = string.Empty;
        }

        public User(string email)
        {
            PasswordHash = Array.Empty<byte>();
            PasswordSalt = Array.Empty<byte>();
            Uuid = Guid.NewGuid();
            Email = email;
        }

        public User(string email, byte[] passwordHash, byte[] passwordSalt)
        {
            Uuid = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}