using CoreHtmlToImage;
using Microsoft.AspNetCore.Mvc;
using TriageConfiguration.Drawer;
using TriageConfiguration.ImageDrawer;
using TriageConfiguration.TextDrawer;
using TriageConfiguration.TriageElements;

namespace TriageConfigurationWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisualizerController : Controller
    {
        /// <summary>
        /// Import a new json string. If successful, string will be converted into desired file that can be downloaded.
        /// </summary>
        /// <param name="triageConfig">Triage configuration to be visualized</param>
        /// <param name="outputType">Type of the output</param>
        /// <returns></returns>

        [HttpPost("generateVisualizedFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetVisualizedFile(TriageConfig? triageConfig, OutputTypeEnum outputType)
        {
            var result = string.Empty;
            var output = string.Empty;
            try
            {
                switch (outputType)
                {
                    case OutputTypeEnum.HtmlImage:
                        result = TriageDrawer.Draw(triageConfig, new HtmlImageTriageDrawer(), OutputTypeEnum.HtmlImage);
                        output = "text/html";
                        break;
                    case OutputTypeEnum.PngImage:
                        var htmlPngImage = TriageDrawer.Draw(triageConfig, new HtmlImageTriageDrawer(), OutputTypeEnum.PngImage);
                        var pngBytes = ImageTriageDrawer.ConvertHtmlToImage(htmlPngImage, ImageFormat.Png);
                        return File(pngBytes, "image/png", "reponse.png");
                    case OutputTypeEnum.JpgImage:
                        var htmlJpgImage = TriageDrawer.Draw(triageConfig, new HtmlImageTriageDrawer(), OutputTypeEnum.JpgImage);
                        var jpgBytes = ImageTriageDrawer.ConvertHtmlToImage(htmlJpgImage, ImageFormat.Jpg);
                        return File(jpgBytes, "image/jpg", "reponse.jpg");
                    case OutputTypeEnum.HtmlText:
                        result = TriageDrawer.Draw(triageConfig, new HtmlTextTriageDrawer(), OutputTypeEnum.HtmlText);
                        output = "text/html";
                        break;
                    case OutputTypeEnum.Text:
                        result = TriageDrawer.Draw(triageConfig, new TextTriageDrawer(), OutputTypeEnum.Text);
                        output = "text/txt";
                        break;
                    default:
                        break;
                }
                return Content(result, output);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
