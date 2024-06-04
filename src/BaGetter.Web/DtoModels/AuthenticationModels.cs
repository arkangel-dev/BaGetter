using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaGetter.Web.DtoModels.AuthenticationModels;

public class VerifyTokenReqModel
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }
}

public class AddNewUserReqModel
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("isadmin")]
    public bool IsAdmin { get; set; }   
}
public class AddNewUserRespModel
{
    public readonly string Message = "User has been added successfully";

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }
}

public class UserModel
{
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
    public string[] Packages { get; set; }
}
