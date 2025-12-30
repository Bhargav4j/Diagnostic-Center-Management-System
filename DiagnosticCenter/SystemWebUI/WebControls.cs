using System.Drawing;

namespace System.Web.UI.WebControls
{
    public class TextBox : Control
    {
        public string Text { get; set; }
    }

    public class Label : Control
    {
        public string Text { get; set; }
        public Color ForeColor { get; set; }
    }

    public class Button : Control
    {
        public string Text { get; set; }
    }

    public class DropDownList : Control
    {
        public string SelectedValue { get; set; }
        public int SelectedIndex { get; set; }
        public ListItem SelectedItem => Items.Count > 0 && SelectedIndex >= 0 && SelectedIndex < Items.Count ? Items[SelectedIndex] : null;
        public object DataSource { get; set; }
        public string DataTextField { get; set; }
        public string DataValueField { get; set; }
        public ListItemCollection Items { get; set; } = new ListItemCollection();

        public void DataBind() { }
        public void ClearSelection() 
        { 
            SelectedIndex = -1;
            SelectedValue = null;
        }
    }

    public class GridView : Control
    {
        public object DataSource { get; set; }
        public string DataKeyNames { get; set; }
        public bool AllowPaging { get; set; }
        public int PageSize { get; set; }

        public void DataBind() { }
    }

    public class Control
    {
        public string ID { get; set; }
        public bool Visible { get; set; } = true;
    }

    public class ListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public ListItem() { }
        public ListItem(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }

    public class ListItemCollection : System.Collections.Generic.List<ListItem>
    {
        public void Add(string text, string value)
        {
            Add(new ListItem(text, value));
        }

        public void Insert(int index, string text)
        {
            Insert(index, new ListItem(text, text));
        }

        public void Insert(int index, string text, string value)
        {
            Insert(index, new ListItem(text, value));
        }
    }

    public class RequiredFieldValidator : Control
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationGroup { get; set; }
    }

    public class RegularExpressionValidator : Control
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationExpression { get; set; }
        public string ValidationGroup { get; set; }
    }

    public class ValidationSummary : Control
    {
        public string HeaderText { get; set; }
        public string ValidationGroup { get; set; }
    }

    public class GridViewRowEventArgs : EventArgs
    {
        public GridViewRow Row { get; set; }
    }

    public class GridViewRow : Control
    {
        public int RowIndex { get; set; }
        public DataControlRowType RowType { get; set; }
        public TableCellCollection Cells { get; set; }
        public object DataItem { get; set; }
    }

    public class TableCellCollection : System.Collections.Generic.List<TableCell>
    {
    }

    public class TableCell : Control
    {
        public string Text { get; set; }
    }

    public enum DataControlRowType
    {
        Header,
        Footer,
        DataRow,
        Separator,
        Pager,
        EmptyDataRow
    }

    public static class DataBinder
    {
        public static object Eval(object container, string expression)
        {
            if (container == null) return null;
            
            var prop = container.GetType().GetProperty(expression);
            return prop?.GetValue(container);
        }
    }
}
