# Xperience by Kentico Video Widget

The **Video Widget** for Xperience by Kentico allows you to seamlessly embed Vimeo and YouTube videos into your website. The widget provides options for video source selection, whether via URL or embed code, and allows customization through CSS classes.

## Download and Installation

- Download the source code for the widget.
- Copy the `VideoWidget` folder, which includes the `VideoWidget.cs` and `_VideoWidget.cshtml` files, and paste it into the `Components/Widgets` directory of your project.
- Rebuild your solution to integrate the widget, and it will be ready for use in the Kentico Page Builder.

## Adding Video Widget to Your Pages

The **Video Widget** can be added as a standard Page Builder widget to any page where the Page Builder is enabled, and editable area restrictions and widget zone restrictions are adjusted accordingly.

Alternatively, the widget can be added directly to your views:


## Widget Properties

The widget provides several configurable properties:

- **Visible**: Determines if the video is visible on the live site.
- **Source**: Specifies the source type for the video, allowing selection between an embed code or a video URL.
- **Video Url**: The URL of the video, which can be from YouTube or Vimeo. This property is only visible when the `Source` property is set to "videoUrl".
- **Embed Code**: The embed code for the video, which can be used to directly embed the video into the page. This property is only visible when the `Source` property is set to "embedCode".
- **Css classes**: Custom CSS classes to apply to the video's containing DIV.

## Contributions

We welcome your feedback and contributions! If you encounter any issues, bugs, or challenges with the video widget, please feel free to open an Issue.

If you're interested in fixing a bug, enhancing the code, or adding new features, we encourage you to submit a Pull Request.