using CMS.Core;
using CMS.Helpers;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using System;
using System.Text.RegularExpressions;
using System.Linq;

using DotStark.XBK.VideoWidget;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: VideoWidgetViewComponent.IDENTIFIER, typeof(VideoWidgetViewComponent),
    name: "Video",
    propertiesType: typeof(VideoWidgetProperties),
    Description = "A video widget that allows embedding Vimeo and YouTube videos seamlessly.",
    IconClass = "icon-triangle-right",
    AllowCache = true)]

namespace DotStark.XBK.VideoWidget;

/// <summary>
/// Class which constructs the <see cref="VideoWidgetViewModel"/> and renders the widget.
/// </summary>
public class VideoWidgetViewComponent : ViewComponent
{
    /// <summary>
    /// The internal identifier of the Video widget.
    /// </summary>
    public const string IDENTIFIER = "DotStark.XBK.Widget.Video";

    private readonly IEventLogService eventLogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoWidgetViewComponent"/> class.
    /// </summary>
    public VideoWidgetViewComponent(IEventLogService eventLogService)
    {
        this.eventLogService = eventLogService;
    }

    /// <summary>
    /// Populates the <see cref="VideoWidgetViewModel"/> and returns the appropriate view.
    /// </summary>
    /// <param name="widgetProperties">User populated properties from the page builder or view.</param>
    public IViewComponentResult Invoke(ComponentViewModel<VideoWidgetProperties> widgetProperties)
    {
        try
        {
            if (widgetProperties is null)
            {
                LogWidgetLoadError("Widget properties were not provided.");
                return Content(String.Empty);
            }


            var vm = new VideoWidgetViewModel
            {
                IsVisible = widgetProperties.Properties.IsVisible,
                VideoUrl = ValidationHelper.GetString(GetVideoUrl(widgetProperties.Properties.VideoUrl), ""),
                EmbedCode = ValidationHelper.GetString(widgetProperties.Properties?.EmbedCode, ""),
                CssClass = ValidationHelper.GetString(widgetProperties.Properties?.CssClass, "")
            };

            return View("~/Components/Widgets/VideoWidget/_VideoWidget.cshtml", vm);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            LogWidgetLoadError($"Error occurred while rendering the video widget: {ex.Message}");
            return Content(String.Empty);
        }
    }

    /// <summary>
    /// Extracts and formats the video Url based on whether it's from YouTube or Vimeo.
    /// </summary>
    /// <param name="properties">The properties containing the video Url.</param>
    /// <returns>The formatted video URL or null if not recognized.</returns>
    private string GetVideoUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }
        try
        {
            const string youtubePattern = @"(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})";
            const string youtubeEmbedUrl = "https://www.youtube.com/embed/";
            const string vimeoEmbedUrl = "https://player.vimeo.com/video/";

            return url switch
            {
                var u when u.Contains("youtube") || u.Contains("youtu.be")
                    => $"{youtubeEmbedUrl}{Regex.Match(u, youtubePattern).Groups[1].Value}",
                var u when u.Contains("vimeo")
                    => $"{vimeoEmbedUrl}{new Uri(u).Segments.Last()}",
                _ => null
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void LogWidgetLoadError(string description)
    {
        eventLogService.LogError("Video Widget",
                "Load",
                description,
                new LoggingPolicy(TimeSpan.FromMinutes(1)));
    }
}

/// <summary>
/// The configurable properties for the Video widget.
/// </summary>
public class VideoWidgetProperties : IWidgetProperties
{
    /// <summary>
    /// Indicates if widget is Visible on live site or not.
    /// </summary>
    [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 0, Label = "Visible", DefaultValue = true)]
    public bool IsVisible
    {
        get;
        set;
    }

    /// <summary>
    /// Specifies the source type for the video, allowing selection between an embed code or a video URL.
    /// </summary>
    [EditingComponent(DropDownComponent.IDENTIFIER, Order = 1, Label = "Source")]
    [EditingComponentProperty(nameof(DropDownProperties.DataSource), "videoUrl;Video Url\r\nembedCode;Embed Code")]
    public string Source
    {
        get;
        set;
    }

    /// <summary>
    /// The URL of the video, which can be from YouTube or Vimeo.
    /// This property is only visible when the <c>Source</c> property is set to "videoUrl".
    /// </summary>
    [EditingComponent(TextInputComponent.IDENTIFIER, ExplanationText = "Enter the URL of the video. This can be a YouTube or Vimeo link.", Order = 2, Label = "Video Url")]
    [VisibilityCondition(nameof(Source), ComparisonTypeEnum.IsEqualTo, "videoUrl")]
    public string VideoUrl
    {
        get;
        set;
    }

    /// <summary>
    /// The embed code for the video, which can be used to directly embed the video into the page.
    /// This property is only visible when the <c>Source</c> property is set to "embedCode".
    /// </summary>
    [EditingComponent(TextAreaComponent.IDENTIFIER, ExplanationText = "Paste the embed code of your video here. This code can usually be obtained from the video hosting platform (e.g., YouTube, Vimeo).", Order = 3, Label = "Embed Code")]
    [VisibilityCondition(nameof(Source), ComparisonTypeEnum.IsEqualTo, "embedCode")]
    public string EmbedCode
    {
        get;
        set;
    }

    /// <summary>
    /// The CSS class(es) added to the Video widget's containing DIV.
    /// </summary>
    [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Css classes", ExplanationText = "Enter any number of CSS classes to apply to the Video, e.g. 'video'")]
    public string CssClass
    {
        get;
        set;
    } = "video";
}

/// <summary>
/// The properties to be set when rendering the widget on a view.
/// </summary>
public class VideoWidgetViewModel
{
    /// <summary>
    /// Indicates if widget is Visible on live site or not.
    /// </summary>
    public bool IsVisible
    {
        get;
        set;
    }

    /// <summary>
    /// The url of the video.
    /// </summary>
    public string VideoUrl
    {
        get;
        set;
    }

    /// <summary>
    /// The embed code of the video.
    /// </summary>
    public string EmbedCode
    {
        get;
        set;
    }

    /// <summary>
    /// The Custom Css Class of the video.
    /// </summary>
    public string CssClass
    {
        get;
        set;
    }
}