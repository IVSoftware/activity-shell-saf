
using System.Drawing.Text;
using System.Reflection;
namespace activity_shell
{
    public partial class HomeActivity : BaseActivity
    {
        public HomeActivity() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BaseActivity.MainWnd = this;
            #region G L Y P H
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Fonts",
                "glyphs.ttf");
            privateFontCollection.AddFontFile(path);
            var fontFamily = privateFontCollection.Families[0];
            Glyphs = new Font(fontFamily, 20F);
            #endregion G L Y P H

            labelTitle.Text = GetType().Name.Replace("Activity", " Activity");
            labelMenu.Font = Glyphs;
            labelMenu.Text = "\uE801";
            labelIcon.Font = Glyphs;
            labelIcon.Text = "\uE805";
            labelMenu.Click += (sender, e) => {
                contextMenuStrip.Show(labelMenu.PointToScreen(
                    new Point(
                        -100,
                        labelMenu.Height
                    )));
            };
            contextMenuStrip.ItemClicked += (sender, e) =>
                MenuItemClicked(e.ClickedItem.Text);
        }

        protected PrivateFontCollection privateFontCollection = new PrivateFontCollection();

    }
}