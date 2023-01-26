namespace activity_shell
{
    public partial class CarsActivity : BaseActivity
    {
        public CarsActivity() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            labelHome.Font = Glyphs;
            labelHome.Text = "\uE805";
            labelIcon.Font = Glyphs;
            labelIcon.Text = "\uE804";
            labelTitle.Text = GetType().Name.Replace("Activity", " Activity");
            labelHome.Click += OnClickHome;
            labelX.Click += OnClickX;
        }
    }
}
