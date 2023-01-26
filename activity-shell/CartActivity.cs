
namespace activity_shell
{
    public partial class CartActivity : BaseActivity
    {
        public CartActivity() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            labelHome.Font = Glyphs;
            labelHome.Text = "\uE805";
            labelTitle.Text = GetType().Name.Replace("Activity", " Activity");
            labelHome.Click += OnClickHome;
            labelX.Click += OnClickX;
        }
    }
}
