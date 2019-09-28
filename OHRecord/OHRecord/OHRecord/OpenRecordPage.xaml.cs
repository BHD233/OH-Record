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
            string currtext = "";
            //add text to record text
            for (int i = 0; i < text.Length; i++)
            {
                if(text[i] == '\n')
                {
                    Entry currEntry = new Entry
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Text = currtext,
                    };
                    recordEntry.Add(currEntry);
                    currtext = "";
                }
                else
                {
                    currtext += text[i];
                }
            }
            //add the last text
            Entry lastEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = currtext,
            };
            recordEntry.Add(lastEntry);

            //add to stack and add stack to scroll
            for (int i = 0; i < recordEntry.Count; i++)
            {
                //StackLayout currStack = new StackLayout
                //{
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //};
                //Entry record = new Entry
                //{
                //    Text = recordEntry[i].Text,
                //};
                recordStack.Children.Add(recordEntry[i]);
            }
        }

        void OnTickButtonClick(object sender, EventArgs args)
        {

        }

        async void OnSaveButtonClick(object sender, EventArgs args)
        {
            string textToWrite = "";
            //get text from label
            for (int i = 0; i < recordEntry.Count; i++)
            {
                textToWrite = textToWrite + recordEntry[i].Text + '\n';
            }
            

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