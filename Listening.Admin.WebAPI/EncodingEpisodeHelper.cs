
using StackExchange.Redis;
using ZHZ.Tools;

namespace Listening.Admin.WebAPI;
public class EncodingEpisodeHelper
{
    private readonly IConnectionMultiplexer redisConn;

    public EncodingEpisodeHelper(IConnectionMultiplexer redisConn)
    {
        this.redisConn = redisConn;
    }

    //一个kv对中保存这个albumId下所有的转码中的episodeId
    private static string GetKeyForEncodingEpisodeIdsOfAlbum(string albumName)
    {
        return $"Listening.EncodingEpisodeIdsOfAlbum.{albumName}";
    }
    private static string GetStatusKeyForEpisode(string episodeName)
    {
        string redisKey = $"Listening.EncodingEpisode.{episodeName}";
        return redisKey;
    }

    /// <summary>
    /// 增加待转码的任务的详细信息
    /// </summary>
    /// <param name="albumId"></param>
    /// <param name="episode"></param>
    /// <returns></returns>
    public async Task AddEncodingEpisodeAsync(string episodeName, EncodingEpisodeInfo episode)
    {
        string redisKeyForEpisode = GetStatusKeyForEpisode(episodeName);
        var db = redisConn.GetDatabase();
        await db.StringSetAsync(redisKeyForEpisode, episode.ToJsonString());//保存转码任务详细信息，供完成后插入数据库
        string keyForEncodingEpisodeIdsOfAlbum = GetKeyForEncodingEpisodeIdsOfAlbum(episode.AlbumName);
        await db.SetAddAsync(keyForEncodingEpisodeIdsOfAlbum, episodeName.ToString());//保存这个album下所有待转码的episodeId
    }

    /// <summary>
    /// 获取这个albumId下所有转码任务
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Guid>> GetEncodingEpisodeIdsAsync(string albumName)
    {
        string keyForEncodingEpisodeIdsOfAlbum = GetKeyForEncodingEpisodeIdsOfAlbum(albumName);
        var db = redisConn.GetDatabase();
        var values = await db.SetMembersAsync(keyForEncodingEpisodeIdsOfAlbum);
        return values.Select(v => Guid.Parse(v));
    }

    /// <summary>
    /// 删除一个Episode任务
    /// </summary>
    /// <param name="db"></param>
    /// <param name="episodeId"></param>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task RemoveEncodingEpisodeAsync(string episodeName, string albumName)
    {
        string redisKeyForEpisode = GetStatusKeyForEpisode(episodeName);
        var db = redisConn.GetDatabase();
        await db.KeyDeleteAsync(redisKeyForEpisode);
        string keyForEncodingEpisodeIdsOfAlbum = GetKeyForEncodingEpisodeIdsOfAlbum(albumName);
        await db.SetRemoveAsync(keyForEncodingEpisodeIdsOfAlbum, episodeName.ToString());
    }

    /// <summary>
    /// 修改Episode的转码状态
    /// </summary>
    /// <param name="db"></param>
    /// <param name="episodeId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task UpdateEpisodeStatusAsync(string episodeName, string status)
    {
        string redisKeyForEpisode = GetStatusKeyForEpisode( episodeName);
        var db = redisConn.GetDatabase();
        string json = await db.StringGetAsync(redisKeyForEpisode);
        EncodingEpisodeInfo episode = json.ParseJson<EncodingEpisodeInfo>()!;
        episode = episode with { Status = status };
        await db.StringSetAsync(redisKeyForEpisode, episode.ToJsonString());
    }

    /// <summary>
    /// 获得Episode的转码状态
    /// </summary>
    /// <param name="db"></param>
    /// <param name="episodeId"></param>
    /// <returns></returns>
    public async Task<EncodingEpisodeInfo> GetEncodingEpisodeAsync(string episodeName)
    {
        string redisKey = GetStatusKeyForEpisode(episodeName);
        var db = redisConn.GetDatabase();
        string json = await db.StringGetAsync(redisKey);
        EncodingEpisodeInfo episode = json.ParseJson<EncodingEpisodeInfo>()!;
        return episode;
    }
}
