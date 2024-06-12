using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZeroGravity.Constants;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Models.StreamContent;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class VideoStreamService : IVideoStreamService
    {
        private readonly ShowHeroesSettings _settings;
        private readonly VimeoSettings _vimeoSettings;
        private readonly string _videoSource;
        private readonly IExternalApiTokenService _externalApiTokenService;

        public VideoStreamService(IOptions<AppSettings> appSettings, IExternalApiTokenService externalApiTokenService)
        {
            _settings = appSettings.Value.ShowHeroesSettings;
            _vimeoSettings = appSettings.Value.VimeoSettings;
            _videoSource = appSettings.Value.VideoSource;
            _externalApiTokenService = externalApiTokenService;
        }

        public async Task<IEnumerable<StreamContentDto>> GetAvailableContentAsync(StreamContentType type)
        {
            var videoList = new List<StreamContentDto>();

            if (_videoSource == VideoSettings.VimeoName)
            {
                string playlistUrl;
                if (type == StreamContentType.Meditation)
                {
                    playlistUrl = _vimeoSettings.Meditation.ApiUrl;
                }
                else
                {
                    playlistUrl = _vimeoSettings.Stream.ApiUrl;
                }

                var url = $"{_vimeoSettings.BaseUrl}{playlistUrl}";

                using var httpClient = new HttpClient();
                try
                {
                    // Add Bearer Token to Call
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _vimeoSettings.token);

                    HttpResponseMessage resp = await httpClient.GetAsync(url);
                    if (resp != null && resp.Content != null)
                    {
                        var jsonData = await resp.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ViemoModels.ViemoRoot>(jsonData);
                        return ViemoModels.ViemoHelper.Convert(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (_videoSource == VideoSettings.ShowHerosName)
            {
                string playlistUrl;
                if (type == StreamContentType.Meditation)
                {
                    playlistUrl = string.Format(_settings.Meditation.ApiUrl, _settings.Meditation.PlaylistId);
                }
                else
                {
                    playlistUrl = string.Format(_settings.Stream.ApiUrl, _settings.Stream.PlaylistId);
                }

                var url = $"{_settings.BaseUrl}{playlistUrl}";

                using var httpClient = new HttpClient();
                try
                {
                    // Add Bearer Token to Call
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _externalApiTokenService.FetchToken());

                    HttpResponseMessage resp = await httpClient.GetAsync(url);
                    if (resp != null && resp.Content != null)
                    {
                        var jsonData = await resp.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ShowHeros.ShowHerosRoot>(jsonData);
                        return ShowHeros.ShowHeroHelper.Convert(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return videoList;
        }
    }

    namespace ViemoModels
    {
        public static class ViemoHelper
        {
            public static IEnumerable<StreamContentDto> Convert(ViemoRoot videoData)
            {
                var result = new List<StreamContentDto>();
                try
                {
                    foreach (var data in videoData?.data)
                    {
                        var vd = data.files.Where(x => x.public_name.Contains("720p")).FirstOrDefault();
                        var thum = data.pictures.sizes.Where(x => x.width == 1280 || x.height == 1280).FirstOrDefault();
                        if (data.is_playable && vd != null && !string.IsNullOrEmpty(vd?.link))
                        {
                            result.Add(new StreamContentDto
                            {
                                Id = Guid.NewGuid(),
                                Title = data.name,
                                Description = data.description?.ToString(),
                                VideoUrl = vd?.link,
                                VideoId = data.uri,
                                ThumbnailUrl = thum?.link,
                                Categories = new List<string>(),
                                VideoPlayerId = string.Empty,
                                Published = data.created_time,
                                Duration = data.duration
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return result;
            }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Paging
        {
            public object next { get; set; }
            public object previous { get; set; }
            public string first { get; set; }
            public string last { get; set; }
        }

        public class Buttons
        {
            public bool like { get; set; }
            public bool watchlater { get; set; }
            public bool share { get; set; }
            public bool embed { get; set; }
            public bool hd { get; set; }
            public bool fullscreen { get; set; }
            public bool scaling { get; set; }
        }

        public class Custom
        {
            public bool active { get; set; }
            public object url { get; set; }
            public object link { get; set; }
            public bool sticky { get; set; }
        }

        public class Logos
        {
            public bool vimeo { get; set; }
            public Custom custom { get; set; }
        }

        public class Title
        {
            public string name { get; set; }
            public string owner { get; set; }
            public string portrait { get; set; }
        }

        public class Live
        {
            public bool streaming { get; set; }
            public bool archived { get; set; }
        }

        public class StaffPick
        {
            public bool normal { get; set; }
            public bool best_of_the_month { get; set; }
            public bool best_of_the_year { get; set; }
            public bool premiere { get; set; }
        }

        public class Badges
        {
            public bool hdr { get; set; }
            public Live live { get; set; }
            public StaffPick staff_pick { get; set; }
            public bool vod { get; set; }
            public bool weekend_challenge { get; set; }
        }

        public class Embed
        {
            public Buttons buttons { get; set; }
            public Logos logos { get; set; }
            public Title title { get; set; }
            public List<object> end_screen { get; set; }
            public bool playbar { get; set; }
            public bool volume { get; set; }
            public bool speed { get; set; }
            public string color { get; set; }
            public bool event_schedule { get; set; }
            public object uri { get; set; }
            public string html { get; set; }
            public Badges badges { get; set; }
        }

        public class Privacy
        {
            public string view { get; set; }
            public string embed { get; set; }
            public bool download { get; set; }
            public bool add { get; set; }
            public string comments { get; set; }
        }

        public class Size
        {
            public int width { get; set; }
            public int height { get; set; }
            public string link { get; set; }
            public string link_with_play_button { get; set; }
        }

        public class Pictures
        {
            public string uri { get; set; }
            public bool active { get; set; }
            public string type { get; set; }
            public List<Size> sizes { get; set; }
            public string resource_key { get; set; }
            public bool default_picture { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Stats
        {
            public int plays { get; set; }
        }

        public class Uploader
        {
            public Pictures pictures { get; set; }
        }

        public class Comments
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Likes
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Texttracks
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Related
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Albums
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class AvailableAlbums
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Versions
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
            public string current_uri { get; set; }
            public string resource_key { get; set; }
        }

        public class Connections
        {
            public Comments comments { get; set; }
            public object credits { get; set; }
            public Likes likes { get; set; }
            public Pictures pictures { get; set; }
            public Texttracks texttracks { get; set; }
            public Related related { get; set; }
            public object recommendations { get; set; }
            public Albums albums { get; set; }
            public AvailableAlbums available_albums { get; set; }
            public Versions versions { get; set; }
            public Appearances appearances { get; set; }
            public Categories categories { get; set; }
            public Channels channels { get; set; }
            public Feed feed { get; set; }
            public Followers followers { get; set; }
            public Following following { get; set; }
            public Groups groups { get; set; }
            public Membership membership { get; set; }
            public ModeratedChannels moderated_channels { get; set; }
            public Portfolios portfolios { get; set; }
            public Videos videos { get; set; }
            public Watchlater watchlater { get; set; }
            public Shared shared { get; set; }
            public WatchedVideos watched_videos { get; set; }
            public FoldersRoot folders_root { get; set; }
            public Folders folders { get; set; }
            public Teams teams { get; set; }
            public Block block { get; set; }
            public Items items { get; set; }
            public List<object> ancestor_path { get; set; }
        }

        public class Watchlater
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public bool added { get; set; }
            public object added_time { get; set; }
            public int total { get; set; }
        }

        public class Report
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public List<string> reason { get; set; }
        }

        public class ViewTeamMembers
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Edit
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class EditContentRating
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public List<string> content_rating { get; set; }
        }

        public class Delete
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class CanUpdatePrivacyToPublic
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Trim
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Interactions
        {
            public Watchlater watchlater { get; set; }
            public Report report { get; set; }
            public ViewTeamMembers view_team_members { get; set; }
            public Edit edit { get; set; }
            public EditContentRating edit_content_rating { get; set; }
            public Delete delete { get; set; }
            public CanUpdatePrivacyToPublic can_update_privacy_to_public { get; set; }
            public Trim trim { get; set; }
            public View view { get; set; }
            public Invite invite { get; set; }
            public DeleteVideo delete_video { get; set; }
            public AddSubfolder add_subfolder { get; set; }
        }

        public class Metadata
        {
            public Connections connections { get; set; }
            public Interactions interactions { get; set; }
            public bool is_vimeo_create { get; set; }
            public bool is_screen_record { get; set; }
        }

        public class Capabilities
        {
            public bool hasLiveSubscription { get; set; }
            public bool hasEnterpriseLihp { get; set; }
            public bool hasSvvTimecodedComments { get; set; }
        }

        public class Appearances
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Categories
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Channels
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Feed
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Followers
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Following
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Groups
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Membership
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class ModeratedChannels
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Portfolios
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Videos
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
            public List<string> rating { get; set; }
            public Privacy privacy { get; set; }
        }

        public class Shared
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class WatchedVideos
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class FoldersRoot
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Folders
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Teams
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class Block
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class LocationDetails
        {
            public string formatted_address { get; set; }
            public object latitude { get; set; }
            public object longitude { get; set; }
            public object city { get; set; }
            public object state { get; set; }
            public object neighborhood { get; set; }
            public object sub_locality { get; set; }
            public object state_iso_code { get; set; }
            public object country { get; set; }
            public object country_iso_code { get; set; }
        }

        public class Preferences
        {
            public Videos videos { get; set; }
        }

        public class Space
        {
            public object free { get; set; }
            public object max { get; set; }
            public int used { get; set; }
            public string showing { get; set; }
        }

        public class Periodic
        {
            public object free { get; set; }
            public object max { get; set; }
            public int used { get; set; }
            public DateTime reset_date { get; set; }
        }

        public class Lifetime
        {
            public object free { get; set; }
            public object max { get; set; }
            public object used { get; set; }
        }

        public class UploadQuota
        {
            public Space space { get; set; }
            public Periodic periodic { get; set; }
            public Lifetime lifetime { get; set; }
        }

        public class User
        {
            public string uri { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public Capabilities capabilities { get; set; }
            public string location { get; set; }
            public string gender { get; set; }
            public object bio { get; set; }
            public object short_bio { get; set; }
            public DateTime created_time { get; set; }
            public Pictures pictures { get; set; }
            public List<object> websites { get; set; }
            public Metadata metadata { get; set; }
            public LocationDetails location_details { get; set; }
            public List<object> skills { get; set; }
            public bool available_for_hire { get; set; }
            public bool can_work_remotely { get; set; }
            public Preferences preferences { get; set; }
            public List<string> content_filter { get; set; }
            public UploadQuota upload_quota { get; set; }
            public string resource_key { get; set; }
            public string account { get; set; }
        }

        public class Items
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public int total { get; set; }
        }

        public class View
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Invite
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class DeleteVideo
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
        }

        public class Property
        {
            public string name { get; set; }
            public bool required { get; set; }
            public string value { get; set; }
        }

        public class AddSubfolder
        {
            public string uri { get; set; }
            public List<string> options { get; set; }
            public bool can_add_subfolders { get; set; }
            public bool subfolder_depth_limit_reached { get; set; }
            public string content_type { get; set; }
            public List<Property> properties { get; set; }
        }

        public class ParentFolder
        {
            public DateTime created_time { get; set; }
            public DateTime modified_time { get; set; }
            public DateTime last_user_action_event_date { get; set; }
            public string name { get; set; }
            public Privacy privacy { get; set; }
            public string resource_key { get; set; }
            public string uri { get; set; }
            public string link { get; set; }
            public object pinned_on { get; set; }
            public bool is_pinned { get; set; }
            public bool is_private_to_user { get; set; }
            public User user { get; set; }
            public object access_grant { get; set; }
            public Metadata metadata { get; set; }
        }

        public class ReviewPage
        {
            public bool active { get; set; }
            public string link { get; set; }
        }

        public class File
        {
            public string quality { get; set; }
            public string type { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string link { get; set; }
            public DateTime created_time { get; set; }
            public double fps { get; set; }
            public int size { get; set; }
            public string md5 { get; set; }
            public string public_name { get; set; }
            public string size_short { get; set; }
        }

        public class Download
        {
            public string quality { get; set; }
            public string type { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public DateTime expires { get; set; }
            public string link { get; set; }
            public DateTime created_time { get; set; }
            public double fps { get; set; }
            public int size { get; set; }
            public string md5 { get; set; }
            public string public_name { get; set; }
            public string size_short { get; set; }
        }

        public class App
        {
            public string name { get; set; }
            public string uri { get; set; }
        }

        public class Upload
        {
            public string status { get; set; }
            public object link { get; set; }
            public object upload_link { get; set; }
            public object complete_uri { get; set; }
            public object form { get; set; }
            public object approach { get; set; }
            public object size { get; set; }
            public object redirect_url { get; set; }
        }

        public class Transcode
        {
            public string status { get; set; }
        }

        public class Datum
        {
            public string uri { get; set; }
            public string name { get; set; }
            public object description { get; set; }
            public string type { get; set; }
            public string link { get; set; }
            public int duration { get; set; }
            public int width { get; set; }
            public object language { get; set; }
            public int height { get; set; }

            // public Embed embed { get; set; }
            public DateTime created_time { get; set; }

            public DateTime modified_time { get; set; }
            public DateTime release_time { get; set; }

            //public List<string> content_rating { get; set; }
            //public string content_rating_class { get; set; }
            //public bool rating_mod_locked { get; set; }
            //public object license { get; set; }
            //public Privacy privacy { get; set; }
            public Pictures pictures { get; set; }

            //public List<object> tags { get; set; }
            //public Stats stats { get; set; }
            //public List<object> categories { get; set; }
            //public Uploader uploader { get; set; }
            //public Metadata metadata { get; set; }
            //public string manage_link { get; set; }
            //public User user { get; set; }
            //  public ParentFolder parent_folder { get; set; }
            //public DateTime last_user_action_event_date { get; set; }
            //public ReviewPage review_page { get; set; }
            public List<File> files { get; set; }

            //  public List<Download> download { get; set; }
            //public App app { get; set; }
            //public string status { get; set; }
            //public string resource_key { get; set; }
            public Upload upload { get; set; }

            public Transcode transcode { get; set; }
            public bool is_playable { get; set; }
            public bool has_audio { get; set; }
        }

        public class ViemoRoot
        {
            public int total { get; set; }
            public int page { get; set; }
            public int per_page { get; set; }
            public Paging paging { get; set; }
            public List<Datum> data { get; set; }
        }
    }

    namespace ShowHeros
    {
        public static class ShowHeroHelper
        {
            public static IEnumerable<StreamContentDto> Convert(ShowHerosRoot videoData)
            {
                var result = new List<StreamContentDto>();
                try
                {
                    foreach (var data in videoData?.data?.video_items?.data)
                    {
                        result.Add(new StreamContentDto
                        {
                            Id = Guid.NewGuid(),
                            Title = data.video_file.title,
                            Description = data.video_file.description,
                            VideoUrl = data.video_file.url,
                            VideoId = data.video_file.id.ToString(),
                            ThumbnailUrl = data.video_file.preview_image,
                            Categories = new List<string>(),
                            VideoPlayerId = string.Empty,
                            Published = data.created_at,
                            Duration = data.video_file.duration_seconds
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return result;
            }
        }

        // Show Hero's Data Structure
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Meta
        {
            public int status_code { get; set; }
            public string message { get; set; }
        }

        public class TechnicalDetails
        {
            public int team_id { get; set; }
            public int user_id { get; set; }
        }

        public class Properties
        {
            public bool is_new { get; set; }
            public bool is_private { get; set; }
            public bool is_playable { get; set; }
            public bool is_editable { get; set; }
            public bool is_translatable { get; set; }
            public bool is_total_buyout { get; set; }
            public bool has_xml_uploaded { get; set; }
            public bool has_alternates { get; set; }
            public bool license { get; set; }
            public bool licensing_state { get; set; }
            public TechnicalDetails technical_details { get; set; }
            public bool is_downloadable { get; set; }
        }

        public class VideoSet
        {
            public int id { get; set; }
            public string title { get; set; }
            public List<int> sentiments { get; set; }
            public string sentiments_text { get; set; }
            public List<int> iab_categories { get; set; }
            public string iab_categories_text { get; set; }
            public int audience { get; set; }
            public string audience_text { get; set; }
            public int duration_seconds { get; set; }
            public string preview_image { get; set; }
            public string preview_image_full_size { get; set; }
            public Properties properties { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class VideoFile
        {
            public int id { get; set; }
            public string hash { get; set; }
            public int video_set_id { get; set; }
            public int status { get; set; }
            public string status_text { get; set; }
            public int type { get; set; }
            public string type_text { get; set; }
            public string title { get; set; }
            public string video_title { get; set; }
            public int locale { get; set; }
            public string locale_text { get; set; }
            public string locale_slug { get; set; }
            public int format { get; set; }
            public string format_text { get; set; }
            public int branding { get; set; }
            public string branding_text { get; set; }
            public int license_type { get; set; }
            public string license_type_text { get; set; }
            public string keywords { get; set; }
            public bool keywords_localized { get; set; }
            public string description { get; set; }
            public bool description_localized { get; set; }
            public int duration_seconds { get; set; }
            public string duration_formatted { get; set; }
            public string preview_image { get; set; }
            public string preview_image_full_size { get; set; }
            public string preview_sprite { get; set; }
            public object embed_video_url { get; set; }
            public object embed_video_width { get; set; }
            public object embed_video_height { get; set; }
            public string url { get; set; }
            public string view_url { get; set; }
            public string edit_url { get; set; }
            public string register_url { get; set; }
            public string download_url { get; set; }
            public string backlink_url { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public DateTime published_at { get; set; }
            public Properties properties { get; set; }
            public VideoSet video_set { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public int video_id { get; set; }
            public int fill_rate { get; set; }
            public int status { get; set; }
            public string status_text { get; set; }
            public int order { get; set; }
            public int pinned { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public VideoFile video_file { get; set; }
            public string title { get; set; }
            public int type { get; set; }
            public string external_mrss_url { get; set; }
            public int external_mrss_status { get; set; }
            public string external_mrss_status_text { get; set; }
            public int locale { get; set; }
            public string keywords { get; set; }
            public List<object> iab_categories { get; set; }
            public int max_videos { get; set; }
            public int update_frequency { get; set; }
            public int video_format { get; set; }
            public int video_freshness_type { get; set; }
            public int team_id { get; set; }
            public int user_id { get; set; }
            public string edit_url { get; set; }
            public string total_duration_formatted { get; set; }
            public int total_videos_count { get; set; }
            public int total_players_count { get; set; }
            public object items_auto_updated_at { get; set; }
            public VideoItems video_items { get; set; }
        }

        public class VideoItems
        {
            public List<Datum> data { get; set; }
        }

        public class ShowHerosRoot
        {
            public Meta meta { get; set; }
            public Datum data { get; set; }
        }
    }
}