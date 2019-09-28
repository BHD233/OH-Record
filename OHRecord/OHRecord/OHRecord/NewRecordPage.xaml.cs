using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OHRecord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRecordPage : ContentPage
    {
        FileHelper fileHelper = new FileHelper();
        int num = 1;
        int startTime = 0;
        string fileName = null;
        bool isGetedName = false;

        public NewRecordPage()
        {
            Title = "Create New Record";
            InitializeComponent();
            entry.Text = num++.ToString();
            entry.Text += "  ";
            startTime = GetTimeInSec();
        }

        async void OnEntryChange(object sender, EventArgs args)
        {
            //fix text of OhAp
            if (OhAp.Text == "oh" || OhAp.Text == "Oh" || OhAp.Text == "oH")
            {
                OhAp.Text = "OH";
            }
            if (OhAp.Text == "ap" || OhAp.Text == "Ap" || OhAp.Text == "aP")
            {
                OhAp.Text = "AP";
            }

            if (!isGetedName && (OhAp.Text == "OH" || OhAp.Text == "AP"))
            {
                fileName = OhAp.Text + DateTime.Today.ToString("dd-MM");
                isGetedName = true;
            }

            Editor editor = (Editor)sender;

            if (editor.Text[editor.Text.Length - 1] == '\n')
            {
                string saveText = editor.Text.Remove(editor.Text.Length - 1);
                int timer = GetTimeInSec() - startTime;
                saveText = saveText + ' ' + timer.ToString() + 's' + '\n' + num++.ToString() + ' ';
                if (num - 1 < 10)
                {
                    saveText += ' ';
                }
                editor.Text = saveText /* + ' ' + timer.ToString() + 's' + '\n' + num++.ToString() + ' '*/;
                startTime = GetTimeInSec();
                //save every time press enter
                if (isGetedName)
                {
                    string errorMessage = null;
                    try
                    {
                        fileHelper.WriteText(fileName, entry.Text);
                    }
                    catch (Exception exc)
                    {
                        errorMessage = exc.Message;
                    }
                    if (errorMessage != null)
                    {
                        await DisplayAlert("Create error", errorMessage, "OK");
                    }
                }
            }
        }

        public int GetTimeInSec()
        {
            return DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 360;
        }

        async void OnButtonSaveClick(object sender, EventArgs args)
        {
            string errorMessage = null;
            try
            {
                fileHelper.WriteText(fileName, entry.Text);
            }
            catch(Exception exc)
            {
                errorMessage = exc.Message;
            }
            if(errorMessage != null)
            {
                await DisplayAlert("Create error", errorMessage, "OK");
            }
            entry.Text = "Wrote";
            await Navigation.PopAsync();
        }
    }
}