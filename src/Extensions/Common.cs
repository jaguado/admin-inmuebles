﻿using System;
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

        public static string StringOrEmpty(this object value)
        {
            return value.ToString() ?? string.Empty;
        }
        public static int IntOrDefault(this object value)
        {
            return value != null && !string.IsNullOrEmpty(value.ToString()) ? int.Parse(value.ToString()) : 0;
        }
    }
}
