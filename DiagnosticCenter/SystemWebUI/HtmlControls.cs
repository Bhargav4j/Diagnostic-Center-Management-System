namespace System.Web.UI.HtmlControls
{
    public class HtmlForm : Control
    {
        public string Method { get; set; }
        public string Action { get; set; }
    }

    public class HtmlGenericControl : Control
    {
        public HtmlGenericControl() { }
        public HtmlGenericControl(string tag) { }
    }

    public class HtmlInputControl : Control
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class HtmlInputText : HtmlInputControl
    {
        public HtmlInputText() { Type = "text"; }
    }

    public class HtmlInputButton : HtmlInputControl
    {
        public HtmlInputButton() { Type = "button"; }
    }

    public class HtmlInputGenericControl : HtmlInputControl
    {
        public HtmlInputGenericControl() { }
        public HtmlInputGenericControl(string type) { Type = type; }
    }

    public class HtmlTableRow : Control
    {
    }

    public class HtmlTableCell : Control
    {
    }

    public class HtmlTable : Control
    {
    }

    public class Control
    {
        public string ID { get; set; }
        public bool Visible { get; set; } = true;
        public string InnerHtml { get; set; }
        public string InnerText { get; set; }
    }
}
