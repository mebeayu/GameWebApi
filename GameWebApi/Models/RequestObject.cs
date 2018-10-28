using System;

namespace GameWebApi.Models
{
    #region 基础请求对象
    /// <summary>
    /// 基础令牌请求
    /// </summary>
    public class BaseTokenRequest
    {
        /// <summary>
        /// 短效token
        /// </summary>
        public string token { get; set; }
    }

    /// <summary>
    /// <para>包含基础令牌的分页请求</para>
    /// <para>当需要获取与指定用户相关的分页数据时使用</para>
    /// </summary>
    public class BasePaginationRequest : BaseTokenRequest
    {
        /// <summary>
        /// 页索引（即，显示第几页）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小（即，一页显示多少行）
        /// </summary>
        public int PageSize { get; set; }
    }
    #endregion

    /// <summary>
    /// <para>用于获取用户游戏赢了的数据请求</para>
    /// <para>包含基础令牌、游戏ID</para>
    /// </summary>
    public class GameWinDataRequest : BaseTokenRequest
    {
        /// <summary>
        /// 游戏ID
        /// </summary>
        public string gameId { get; set; }

        /// <summary>
        /// 获取多少条数据
        /// </summary>
        public int topNumber { get; set; }
    }
}