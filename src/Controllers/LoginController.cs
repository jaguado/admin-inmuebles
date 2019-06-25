using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminInmuebles.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("/health")]
        public IActionResult Get()
        {
            return new OkResult();
        }

        /// <summary>
        /// Login endpoint (doesn't check auth headers)
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>Valid JWT with authorization and customer information to load home screen</returns>
        [HttpPost("/login")]
        public async Task<IActionResult> PostAsync([FromBody] Models.Credentials credentials)
        {
            var args = new Dictionary<string, string>
            {
                { "MAIL", credentials.email },
                { "PASS", credentials.password }
            };
            var getUserInfo = await Helpers.Sql.Execute("[desoincl_inmueble].[SP_USUARIO_VALIDA_CREDENCIALES]", args);
            if (getUserInfo == null || getUserInfo.Tables.Count == 0 || getUserInfo.Tables[0].Rows.Count == 0)
                return new UnauthorizedResult();
            //check credentials and TODO return a valid response
            var userInfo = getUserInfo.Tables[0].Select()[0];
            var userData = new List<dynamic>();
            getUserInfo.Tables[0].Select().ToList().ForEach(row =>
            {
                userData.Add(new
                {
                    Rut = row["RUT"],
                    RazonSocial = row["RAZON_SOCIAL"],
                    Tipo = row["ID_TIPO_CONDOMINIO"],
                    Vigencia = row["VIGENTE"]
                });
            });
            return new OkObjectResult(new
            {
                authToken = "ya29.GlwyB9JcWBQCIkq0SHHzWRZpS8ozvdZ1i1AyLh_LgqOlIhJHgJ6aL3iHFJ0LUbV2ACOoqpNfmAh36Pjs31DMvxazvkGOCg4KRcJwjTrUvR-wK8Mmvce5nifhp_cLKA",
                credentials.email,
                firstName = userInfo["NOMBRE"].ToString(),
                id = "105560751972558300957",
                idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjExOGRmMjU0YjgzNzE4OWQxYmMyYmU5NjUwYTgyMTEyYzAwZGY1YTQiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiNTQzMzk4NTE4MDgyLTY5dHJxc2lzanZiN2t2NWZsdGE1cWlobzU1ZTFma2JyLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiNTQzMzk4NTE4MDgyLTY5dHJxc2lzanZiN2t2NWZsdGE1cWlobzU1ZTFma2JyLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTA1NTYwNzUxOTcyNTU4MzAwOTU3IiwiZW1haWwiOiJqYWd1YWRvbUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6Ik5vNHZXYUM5UnpkTk8tWHFjRXU2QmciLCJuYW1lIjoiSm9yZ2UgWFIiLCJwaWN0dXJlIjoiaHR0cHM6Ly9saDQuZ29vZ2xldXNlcmNvbnRlbnQuY29tLy1EYXpjTVNBNGlSNC9BQUFBQUFBQUFBSS9BQUFBQUFBQUF5TS9EWEVHUlBFTHU4OC9zOTYtYy9waG90by5qcGciLCJnaXZlbl9uYW1lIjoiSm9yZ2UiLCJmYW1pbHlfbmFtZSI6IlhSIiwibG9jYWxlIjoiZXMiLCJpYXQiOjE1NjE0Mzc1MDQsImV4cCI6MTU2MTQ0MTEwNCwianRpIjoiNTllZGJlODY5M2FmMjU3ZDA0MTg4ZDRkYjRhOWMyZmNlOTdjMjQ0OCJ9.d-c7nzy4K82OhxrvX7NZn_VKQ32VoDCqfajYm0diixzDtoB5LRC0n7DqhJBhUhPxq4bkyeq7Kmzqf3Dqx9nohWrSWUg9VGlhLX7URSHsYZSzGUM9gxyoi0mco2z9vvmBJL63QVHY6ZFwT6SRYO4j7kvzl2TgJnpXicAofaVDV822bLcuZ1YA6205YiznYfAKJO8Zgd2USs3y4XvAQ9mYDpXNAoscyr56-lHNr6QOEM56Glex1Wi701ovq69YUNKp9ew3-mRxJPDUVlfMAqMYzoMYq-oRsoyLZz1V0l55qnAf_jRCOfbKzXlRl04VtMEgXtB_A1qKo-wLCp7LiKJWsA",
                lastName = "",
                name = userInfo["NOMBRE"].ToString(),
                photoUrl = "https://lh4.googleusercontent.com/-DazcMSA4iR4/AAAAAAAAAAI/AAAAAAAAAyM/DXEGRPELu88/s96-c/photo.jpg",
                provider = "internal",
                state = 1, 
                data = userData
            });
        }
    }
}
