using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace activity_shell
{
    public class BaseActivity : Form
    {
        public BaseActivity() => FormClosing += OnActivityClosing;
        protected void OnActivityClosing(object? sender, FormClosingEventArgs e) { 
            e.Cancel = true; // Prevent destruction of handle if `Close` is called.
            Hide();
        }

        // "When I click on the main menu all the old pages that were opened are closed."
        internal void OnClickHome(object? sender, EventArgs e){
            foreach (var form in VisibleChildForms) form.Hide();
        }
        public static Font? Glyphs { get; protected set; }
        public static HomeActivity? MainWnd { get; protected set; }
        protected virtual void MenuItemClicked(string activity)
        {
            if (activity == "Exit")
            {
                // Remove the handler. Allow close.
                foreach (var open in Application.OpenForms.OfType<BaseActivity>())
                {
                    open.FormClosing -= open.OnActivityClosing;
                }
                Application.Exit();
            }
            else
            {
                var name = $"{activity}Activity";
                BaseActivity? form = 
                    VisibleChildForms.FirstOrDefault(_ => _.GetType().Name.Equals(name));
                if (form == null)
                {   // Create if NOT EXIST
                    switch (activity)
                    {
                        case "Flights": form = new FlightsActivity(); break;
                        case "Cars": form = new CarsActivity(); break;
                        case "Cart": form = new CartActivity(); break;
                        default:
                            throw new NotImplementedException($"{activity}");
                    }
                }
                if (!form.Visible)
                {  // Show if NOT VISIBLE
                    var x = MainWnd.Location.X;
                    foreach (var open in VisibleForms)
                    {
                        x += 10;
                        x += open.Width;
                    }
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(x, MainWnd.Location.Y);
                    form.Show();
                }
            }
        }

        // Slide the window positions over (animated).
        internal void OnClickX(object? sender, EventArgs e)
        {
            Close();
            var x = MainWnd.Location.X;
            foreach (var open in VisibleChildForms)
            {
                x += 10;
                x += open.Width;
                int animate = open.Location.X;
                while(animate > x)
                {
                    open.Location = new Point(animate, MainWnd.Location.Y);
                    animate--;
                }
                open.Location = new Point(x, MainWnd.Location.Y);
            }
        }

        internal static BaseActivity[] VisibleForms => Application
            .OpenForms
            .Cast<BaseActivity>()
            .Where(_ => _.Visible)
            .ToArray();
        internal static BaseActivity[] VisibleChildForms => Application
            .OpenForms
            .Cast<BaseActivity>()
            .Where(_ => !_.GetType().Name.Equals(nameof(HomeActivity)))
            .Where(_ => _.Visible)
            .ToArray();
    }
}
