using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NewsByTheMood.MVC.TagHelpers
{
    public class GeneralNotifyMessageTagHelper : TagHelper
    {
        public bool UseAutoHiding { get; set; } = false;
        public int AutoHidingDelay { get; set; } = 5000;

        public GeneralNotifyMessageTagHelper(IHtmlGenerator generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("general-notify-message", "");
            output.Attributes.Add("style", "display: none;");

            var message = new TagBuilder("div");
            message.Attributes.Add("class", "message");
            output.Content.AppendHtml(message);

            var closeButton = new TagBuilder("button");
            closeButton.AddCssClass("close");
            closeButton.AddCssClass("btn-close");
            closeButton.Attributes.Add("type", "button");
            closeButton.Attributes.Add("aria-label", "Close");
            output.Content.AppendHtml(closeButton);

            var script = $@"
                <script>
                    let isSetEventHandlergeneralNotifyButton = false;
                    function showGeneralNotifyMessage(message, type) {{
                        const generalNotifyUseAutoHiding = '{UseAutoHiding}';
                        const generalNotifyUseAutoHidingDelay = {AutoHidingDelay};

                        const generalNotifyContainer = document.querySelector(""div[general-notify-message]"");
                        if (!generalNotifyContainer) {{
                            console.error(""General notify container not found"");
                            return;
                        }}

                        const generalNotifyMessage = document.querySelector(""div[general-notify-message] div.message"");

                        const generalNotifyButton = document.querySelector(""div[general-notify-message] button.close"");

                        if (!isSetEventHandlergeneralNotifyButton) {{
                            generalNotifyButton.addEventListener(""click"", function () {{
                                generalNotifyMessage.textContent = """";
                                generalNotifyContainer.className = """";
                                generalNotifyContainer.style.display = ""none"";
                            }});

                            isSetEventHandlergeneralNotifyButton = true;
                        }}

                        generalNotifyContainer.className = ""alert d-flex justify-content-between"";
                        generalNotifyContainer.style.display = ""block"";

                        generalNotifyMessage.textContent = message;

                        switch (type) {{
                            case ""primary"":
                                generalNotifyContainer.classList.add(""alert-primary"");
                                break;
                            case ""secondary"":
                                generalNotifyContainer.classList.add(""alert-secondary"");
                                break;
                            case ""success"":
                                generalNotifyContainer.classList.add(""alert-success"");
                                break;
                            case ""warning"":
                                generalNotifyContainer.classList.add(""alert-warning"");
                                break;
                            case ""error"":
                                generalNotifyContainer.classList.add(""alert-danger"");
                                break;
                            default:
                                generalNotifyContainer.classList.add(""alert-info"");
                        }}

                        if (generalNotifyUseAutoHiding === 'True') {{
                            setTimeout(() => {{
                                generalNotifyButton.click();
                            }},
                            generalNotifyUseAutoHidingDelay);
                        }}
                    }}
                </script>";

            output.Content.AppendHtml(script);
        }
    }
}
