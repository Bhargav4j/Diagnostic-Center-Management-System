using System.Collections.Specialized;

namespace System.Web.UI
{
    public class Page
    {
        public System.Web.SessionState.HttpSessionState Session { get; set; }
        public System.Web.HttpResponse Response { get; set; }
        public System.Web.HttpRequest Request { get; set; }
        public StateBag ViewState { get; set; } = new StateBag();
        public bool IsPostBack { get; set; }

        protected virtual void Page_Load(object sender, EventArgs e) { }
    }

    public class StateBag : System.Collections.Generic.Dictionary<string, object>
    {
        public new object this[string key]
        {
            get => ContainsKey(key) ? base[key] : null;
            set => base[key] = value;
        }
    }
}
