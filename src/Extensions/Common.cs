using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Extensions
{
    public static class Common
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public static JwtSecurityToken ToJwt(this string jwt)
        {
            try
            {
                return new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            }
            catch
            {
                return null;
            }
        }
    }
}
