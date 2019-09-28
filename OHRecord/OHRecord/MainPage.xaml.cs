using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OHRecord
{
    public class RecordTitle
    {
        public Button summaryButton = new Button
        {
            Text = "...",
            WidthRequest = Device.GetNamedSize(NamedSize.Small, typeof(Button)) * 3,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
        };
        public Button viewButton = new Button
        {
            Text = "View",
            WidthRequest = Device.GetNamedSize(NamedSize.Small, typeof(Button)) * 4,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
        };
        public Button deleteButton = new Button
        {
            Text = "X",
            WidthRequest = Device.GetNamedSize(NamedSize.Small, typeof(Button)) * 3,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
        };
        public Label name = new Label
        {
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
        };
        public StackLayout GetTitle()
        {
            StackLayout mainStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            mainStack.Children.Add(deleteButton);
            mainStack.Children.Add(name);
            mainStack.Children.Add(summaryButton);
            mainStack.Children.Add(viewButton);
            return mainStack;
        }
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        IFileHelper fileHelper = DependencyService.Get<IFileHelper>();
        public MainPage()
        {
            Title = "Main Page";
            InitializeComponent();
            //List<RecordTitle> title = new List<RecordTitle>();
            //Title a = new Title();
            //a.name.Text = "BHD";
            //title.Add(a);
            //listStack.Children.Add(a.GetTitle());
            RefreshList();
        }
        void RefreshList()
        {
            listStack.Children.Clear();
            IEnumerable<string> titleStrings = fileHelper.GetFiles();
            foreach (string titleString in titleStrings)
            {
                RecordTitle currTitle = new RecordTitle();
                string temp = titleString.ToString();
                string titleText = "";
                for(int i = 6; i >= 0;i--)
                {
                    titleText += temp[temp.Length - i - 1];
                }
                //create handle and ID for button
                currTitle.summaryButton.StyleId = titleText;
                currTitle.viewButton.StyleId = titleText;
                currTitle.deleteButton.StyleId = titleText;
                currTitle.summaryButton.Clicked += OnSummaryButtonClick;
                currTitle.viewButton.Clicked += OnViewButtonClick;
                currTitle.deleteButton.Clicked += OnDeleteButtonClick;

                //get tile to lable
                currTitle.name.Text = titleText;

                //add to stack
                listStack.Children.Add(currTitle.GetTitle());
            }
        }

        async void OnAddButtonClick(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewRecordPage());
            RefreshList();
        }

        async void OnSummaryButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SummaryPage(button.StyleId));
            RefreshList();
        }

        async void OnViewButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new OpenRecordPage(button.StyleId));
            RefreshList();
        }

        void OnDeleteButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string filename = button.StyleId;
            fileHelper.Delete(filename);
            RefreshList();
        }
        
        void OnRefreshButtonClick(object sender, EventArgs args)
        {
            RefreshList();
        }
    }
}