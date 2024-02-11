﻿using Newtonsoft.Json;

namespace Startup.Api.Models.Authorization;

public class OauthToken
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    public string Scope { get; set; }
}