One of many ways to do this is to make a common base `Form` class and use the `Application.OpenForms` collection to make an ad hoc manager for forms that inherit it. The one tricky thing when using `Show()` in repeated visibility cycles is that the `Close()` method will destroy the window handle if called. For this reason, the `BaseActivity` class below will attach a canceler method to the `FormClosing` event and remove it when the application is shutting down.

***
**Minimal example with Flow Layout**
- New forms will open to the right of visible forms.
- Click [HomeIcon] to close _all_ child windows, leaving only the main window open.
- Click [X] to close single window and slide remaining visible forms to the left (animated)

This way, when you **click on the main menu all the old pages that were opened are closed**.


[![screenshot][1]][1]

***

    public class BaseActivity : Form
    {
        public static HomeActivity? MainWnd { get; protected set; }

**Utility collections made from `Application.OpenForms`**

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


**"When I click on the main menu all the old pages that were opened are closed."**

        internal void OnClickHome(object? sender, EventArgs e){
            foreach (var form in VisibleChildForms) form.Hide();
        }

**Any form can open other windows.**

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
    }

**After closing single window with [X] slide the window positions over (animated) to fill in the gap.**

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
    }


  [1]: https://i.stack.imgur.com/wqL44.png