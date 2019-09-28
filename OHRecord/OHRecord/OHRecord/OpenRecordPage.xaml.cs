using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OHRecord
{
    public class Detail
    {
        public Button tick = new Button
        {
            BackgroundColor = Color.White,
        };
        public Entry text = new Entry();
        public StackLayout GetDetail()
        {
            StackLayout mainStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
            };

            mainStack.Children.Add(tick);
            mainStack.Children.Add(text);
            return mainStack;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpenRecordPage : ContentPage
    {
        FileHelper fileHelper = new FileHelper();
        string fileToSave = null;
        List<Entry> recordEntry = new List<Entry>();
        public OpenRecordPage(string filename)
        {
            Title = filename;
            fileToSave = filename;
            InitializeComponent();
            LoadText(filename);
        }

        void LoadText(string filename)
        {
            //add text to record text
            //use stack and entry to show the detail instead of editor (for easy scroll)
            string text = fileHelper.ReadText(filename);

            recordArea.Text = text;
        }

        void OnTickButtonClick(object sender, EventArgs args)
        {

        }

        async void OnSaveButtonClick(object sender, EventArgs args)
        {
            string textToWrite = "";

            textToWrite = recordArea.Text;            

            string errorMessage = null;
            try
            {
                fileHelper.WriteText(fileToSave, textToWrite);
            }
            catch (Exception exc)
            {
                errorMessage = exc.Message;
            }
            if (errorMessage != null)
            {
                await DisplayAlert("Create record", errorMessage, "OK");
            }
            await Navigation.PopAsync();
        }
    }
}