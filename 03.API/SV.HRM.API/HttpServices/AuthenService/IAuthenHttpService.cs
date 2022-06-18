using SV.HRM.Models;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IAuthenHttpService
    {
        /// <summary>
        /// Thằng này để ra access token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<Ws02IS_ResponseTokenModel> GetTokenInfo(string code);

        /// <summary>
        /// Thằng này để lấy userinfo từ ws02, cũng là để check session login luôn
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public Task<string> GetUserInfo(string access_token);

        /// <summary>
        /// Thằng này để ra access token từ refreshtoken khi đã hết hạn
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public Task<Ws02IS_ResponseTokenModel> GetTokenFromRefreshToken(string refresh_token);
    }
}
