using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cozy.WebUI.AppCode.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("rate")]
    public class RateStarTagHelper : TagHelper
    {
        [HtmlAttributeName("rate-value")]
        public double RateValue { get; set; }

        [HtmlAttributeName("rate-product-id")]
        public int ProductId { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            string additionalClass = "";

            if (this.RateValue >= 4.8)
            {
                additionalClass = "rate-5";
            }
            else if (this.RateValue > 4)
            {
                additionalClass = "rate-half5";
            }
            else if (this.RateValue >= 3.8)
            {
                additionalClass = "rate-4";
            }
            else if (this.RateValue > 3)
            {
                additionalClass = "rate-half4";
            }
            else if (this.RateValue >= 2.8)
            {
                additionalClass = "rate-3";
            }
            else if (this.RateValue > 2)
            {
                additionalClass = "rate-half3";
            }
            else if (this.RateValue >= 1.8)
            {
                additionalClass = "rate-2";
            }
            else if (this.RateValue > 1)
            {
                additionalClass = "rate-half2";
            }
            else if (this.RateValue >= 0.8)
            {
                additionalClass = "rate-1";
            }
            else if (this.RateValue > 0)
            {
                additionalClass = "rate-half1";
            }

            output.Attributes.Add("class", $"rate {additionalClass}");

            output.Content.SetHtmlContent(@$"<li data-rate='1' data-product-id='{this.ProductId}'></li>
        <li data-rate='2' data-product-id='{this.ProductId}'></li>
        <li data-rate='3' data-product-id='{this.ProductId}'></li>
        <li data-rate='4' data-product-id='{this.ProductId}'></li>
        <li data-rate='5' data-product-id='{this.ProductId}'></li>");
        }
    }
}
