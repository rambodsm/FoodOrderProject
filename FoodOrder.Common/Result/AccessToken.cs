using System;
using System.IdentityModel.Tokens.Jwt;

namespace FoodOrder.Common.Result
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        ///<summary>
        ///With Out Refresh Token
        ///(JWT Token PayLoad Will Be Encrypt)
        ///</summary>
        public AccessToken(JwtSecurityToken securityToken)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
        }
        ///<summary>
        ///With Refresh Token
        ///(JWT Token PayLoad Will Be Encrypt)
        ///</summary>
        public AccessToken(JwtSecurityToken securityToken,string refreshToken)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
            refresh_token = refreshToken;
        }
    }
}
